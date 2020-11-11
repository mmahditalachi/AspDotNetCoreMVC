using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dashboard.Models;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Dashboard.Utilities;

namespace Dashboard.ViewModels
{
    public class RegisterViewModel
    {

        [Required]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        [Remote(action: "IsEmailInUse",controller:"Account")]
        //custom validation
        [ValidEmailDomain(allowedDomain: "test.com")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "password and confirme password must be match")]
        public string ConfrimPassword { get; set; }

        public string City { get; set; }
    }
}
