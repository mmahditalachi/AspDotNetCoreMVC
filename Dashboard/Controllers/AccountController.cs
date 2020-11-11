﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Dashboard.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Dashboard.Models;
using System.Security.Claims;
using Microsoft.Extensions.Logging;


namespace Dashboard.Controllers
{
    public class AccountController : Controller
    {

        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly ILogger<AccountController> logger;

        public AccountController(UserManager<ApplicationUser> userManager,
                    SignInManager<ApplicationUser> signInManager, 
                    ILogger<AccountController> logger) 
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }   

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    Email = model.Email,
                    UserName = model.Email,
                    City = model.City
                };

                var result = await userManager.CreateAsync(user,model.Password);

                if(result.Succeeded)
                {

                    var token = await userManager.GenerateEmailConfirmationTokenAsync(user);

                    var ConfrimationLink =  Url.Action("ConfrimEmail","Account", new { userId = user.Id, token = token }, Request.Scheme );

                    logger.Log(LogLevel.Warning, ConfrimationLink);

                    if (signInManager.IsSignedIn(User) && User.IsInRole("Admin"))
                    {
                        return RedirectToAction("ListUsers", "Administration");
                    }
                    //if isPersistent ==> cookie false ==> session
                    // comment this code because user must first confrim email 
                    //await signInManager.SignInAsync(user, isPersistent: false);
                    //return Redirec tToAction("Index", "Home");

                    ViewBag.ErrorTitle = "Regestation successful";
                    ViewBag.ErrorMessage = "before you can Login please confrim " +
                        "your account by clicking on confrimation link we have emailed you ";
                    return View("Error");
                    
                }

                foreach (var error in result.Errors) 
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        //we can use Accept verb for multiple action
        [AcceptVerbs("Get","Post")]
        [AllowAnonymous]
        public async Task<IActionResult> IsEmailInUse(string email)
        {
            var user = await userManager.FindByEmailAsync(email);

            if (user == null)
            {
                //we return json because we check that with ajax
                return Json(true);
            }
            else
                return Json($"Email { email } is already in use");
        }

        [HttpGet]
        public async Task<IActionResult> Login(string returnUrl)
        {

            LoginViewModel model = new LoginViewModel()
            {
                ReturnUrl = returnUrl,
                ExternalLogins = (await signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
            };

            return View(model);
        }   

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model,string returnUrl)
        {

            //for external login 
            model.ExternalLogins = (await signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (ModelState.IsValid)
            {

                var user = await userManager.FindByEmailAsync(model.Email);

                //last condition for preventing hacker to find out user password and user email confrimation
                if(user != null && !user.EmailConfirmed &&
                    ( await userManager.CheckPasswordAsync(user, model.Password)))
                {
                    ModelState.AddModelError(string.Empty, "Email is not confrimed yet");
                    return View(model);
                }

                var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);

                if (result.Succeeded)
                {
                    //we can also use use Url.isLocalUrl(returnUrl) method
                    if (!string.IsNullOrEmpty(returnUrl))
                    {
                        //return Redirect(returnUrl);
                        //above line has a security hole I tell that in notebook 
                        return LocalRedirect(returnUrl);
                    }
                    else
                    {
                        //if isPersistent ==> cookie false ==> session
                        return RedirectToAction("Index", "Home");
                    }

                }

                ModelState.AddModelError(string.Empty, "Invalid login attemp");
                
            }
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult ExternalLogin(string provider, string returnUrl)
        {
            var redirectUrl = Url.Action("ExternalLoginCallBack","Account", new { ReturnUrl = returnUrl });
            var properties = signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);

            return new ChallengeResult(provider, properties);
        }

        public async Task<IActionResult> ExternalLoginCallBack(string returnUrl =null,string remoteError = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");

            LoginViewModel loginViewModel = new LoginViewModel
            {
                ReturnUrl = returnUrl,
                ExternalLogins = (await signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
            };

            if(remoteError != null)
            {
                ModelState.AddModelError(string.Empty, $"Error from External provider : {remoteError}");

                return View("Login",loginViewModel);
            }

            var info = await signInManager.GetExternalLoginInfoAsync();

            if(info == null)
            {
                ModelState.AddModelError(string.Empty, $"Error loading external login information");

                return View("Login", loginViewModel);
            }

            var email = info.Principal.FindFirstValue(ClaimTypes.Email);

            ApplicationUser user = null; 

            if(email != null)
            {
                user = await userManager.FindByEmailAsync(email);

                if(user != null && !user.EmailConfirmed)
                {
                    ModelState.AddModelError(string.Empty, "Email is not confrimed yet");
                    return View("Login",loginViewModel);
                }
            }

            var signInResult = await signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);

            if(signInResult.Succeeded)
            {
                return LocalRedirect(returnUrl);
            }
            else
            {
                if(email != null)
                {         
                    if(user == null)
                    {
                        user = new ApplicationUser
                        {
                            UserName = info.Principal.FindFirstValue(ClaimTypes.Email),
                            Email = info.Principal.FindFirstValue(ClaimTypes.Email)
                        };

                        await userManager.CreateAsync(user);

                        //var token = userManager.CreateSecurityTokenAsync(user);
                        var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
                        var confrimationLink = Url.Action("ConfrimEmail", "Account", new { userId = user.Id, token = token }, Request.Scheme);

                        logger.Log(LogLevel.Warning, confrimationLink);

                        ViewBag.ErrorTitle = "Regestation successful";
                        ViewBag.ErrorMessage = "before you can Login please confrim " +
                            "your account by clicking on confrimation link we have emailed you ";
                        return View("Error");

                    }

                    await userManager.AddLoginAsync(user,info);
                    await signInManager.SignInAsync(user, isPersistent: false);

                    return LocalRedirect(returnUrl);

                }

            }
            ViewBag.ErrorTitle = $"Email claim not received from : {info.LoginProvider}";
            ViewBag.ErrorMessage = $"please contact support on Admin@admin.com";
            return View("Error");
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfrimEmail(string userId, string token)
        {
            if(userId == null || token == null)
            {
                return View("Index", "Home");
            }
            var user = await userManager.FindByIdAsync(userId);

            if(user == null)    
            {
                ViewBag.ErrorMessage = $"user with this user Id : {userId} not found";
                return View("Error");
            }

            var result = await userManager.ConfirmEmailAsync(user, token);
             
            if(result.Succeeded)
            {
                return View();
            }

            ViewBag.ErrorTitle = "Email cannot be confrimed";
            return View("Error");

        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if(ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(model.Email);

                if(user != null && await userManager.IsEmailConfirmedAsync(user))
                {
                    var token = await userManager.GeneratePasswordResetTokenAsync(user);
                    var passwordResetLink = Url.Action("ResetPassword", "Account", new { email = user.Email, token = token }, Request.Scheme);
                    logger.Log(LogLevel.Warning, passwordResetLink);

                    return View("ForgotPasswordConfrimation");
                }
                
                return View("ForgotPassword");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult ResetPassword(string token, string email)
        {
            if (email == null || token == null)
            {
                ModelState.AddModelError("", "Invalid Password token");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if(ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(model.Email);
                if(user != null)
                {
                    var result = await userManager.ResetPasswordAsync(user, model.Token , model.Password);

                    if(result.Succeeded)
                    {
                        return View("ResetPasswordConfrimation");
                    }
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return View(model);
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if(ModelState.IsValid)
            {
                var user = await userManager.GetUserAsync(User);

                if(user == null)
                {
                    return RedirectToAction("Login");
                }

                var result = await userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

                if(!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return View(model);
                }
                await signInManager.RefreshSignInAsync(user);
                return View("ChangePasswordConfrimation");
            }
            return View(model);
        }
    }
}