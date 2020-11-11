using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Dashboard.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Http;
using Dashboard.Security;

namespace Dashboard
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            
            services.AddControllersWithViews();
            //Asp .net core 2.0 
            services.AddDbContextPool<AppDbContext>(option => option.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>(option =>
            {
                option.Password.RequiredLength = 6;
                option.Password.RequireDigit = true;
                option.Password.RequireUppercase = true;

                //email confrimation
                option.SignIn.RequireConfirmedEmail = true;
                option.Tokens.EmailConfirmationTokenProvider = "CustomEmailConfrimation";
            })
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders()
                //for custom Email confrimation Time 
                .AddTokenProvider<CustomEmailConfrimationTokenProvider
                <ApplicationUser>>("CustomEmailConfrimation");

            //change token lifespan of all token type
            services.Configure<DataProtectionTokenProviderOptions>(
                o => o.TokenLifespan = TimeSpan.FromHours(5));

            //change token lifespan of just email confrimation token 
            services.Configure<CustomEmailConfrimationTokenProviderOptions>(
                o => o.TokenLifespan = TimeSpan.FromHours(3));

            //it means user must sign in first
            //services.AddMvc(option =>
            //{
            //    var policy = new AuthorizationPolicyBuilder()
            //                    .RequireAuthenticatedUser()
            //                    .Build();
            //    option.Filters.Add(new AuthorizeFilter(policy));
            //}).AddXmlSerializerFormatters();

            //google Authentication
            services.AddAuthentication()
                 .AddGoogle(options =>
                 {
                     options.ClientId = "374948531686-t9e27bulefvltl2idvjn5iepg383meeh.apps.googleusercontent.com";
                     options.ClientSecret = "uXIgHg2N2M6lYuYp7BBQ-wjI";
                     //options.CallbackPath = "";
                 });


            //services.ConfigureApplicationCookie(option =>
            //{
            //    option.AccessDeniedPath = new PathString("Administration/AccessDenied");
            //});

            services.AddAuthorization(option => 
            {
                option.AddPolicy("DeleteRolePolicy",
                    policy => policy.RequireClaim("delete Role"));


                //creating custom identity 
                option.AddPolicy("EditRolePolicy",
                    policy => policy.AddRequirements(new ManageAdminRolesAndClaimsRequirements()));

                //in bara ine k handler age fail shod handler badi ejra nshe
                //option.InvokeHandlersAfterFailure = false;


                //custom claims ==> agar role admin dashte bashe va claim edit dashte bashe mitune edit kone
                //ya agar role super admin dashte bashe 
                //option.AddPolicy("EditRolePolicy", policy => policy.RequireAssertion(contex =>
                //contex.User.IsInRole("Admin") && 
                //contex.User.HasClaim(claim=>claim.Type == "Edit Role" && claim.Value =="true") ||
                //contex.User.IsInRole("Super Admin")
                //));


                //default claim
                //option.AddPolicy("EditRolePolicy",
                //    policy => policy.RequireClaim("Edit Role"));

                //by defult set editrolepolicy true for every user
                //option.AddPolicy("EditRolePolicy",
                //    policy => policy.RequireClaim("Edit Role"));

                //creting claims for Admin role
                option.AddPolicy("AdminRolePolicy",
                    policy => policy.RequireRole("Admin"));

                //we can add this claim for multi roles
                //option.AddPolicy("AdminRolePolicy",
                //    policy => policy.RequireClaim("Admin, Users"));
            });

            //user from mock repository 
            //services.AddTransient<IUsersRepository, MockUsersRepository>();
            //we use this to access in rntire scope with http request 
            services.AddScoped<IUsersRepository, SQLUsersRepository>();
            //use fromm sql repository 
            //services.AddTransient<IUsersRepository, SQLUsersRepository>();

            //add singelton for custom authorization
            services.AddSingleton<IAuthorizationHandler, CanEditOnlyOtherAdminRolesAndClaimsHandler>();
            services.AddSingleton<IAuthorizationHandler, SuperAdminHandler>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                //to redirect user to controller
                //these to have some diffrence 
                //app.UseStatusCodePagesWithRedirects("/Error/{0}");
                //reexecte pipline and return original status code 
                app.UseStatusCodePagesWithReExecute("/Error/{0}");

                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                //app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
