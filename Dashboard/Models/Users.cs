using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Dashboard.Models
{
    public class Users
    {
        [Key]
        public Guid UserID { get; set; }
        [Required(ErrorMessage ="لطفا نام کاربری خود را وارد کنید")]
        [StringLength(maximumLength:20,MinimumLength =5)]
        public string Username { get; set; }
        [Required(ErrorMessage ="لطفا رمز را وارد کنید")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage ="لطفا ایمیل خود را وارد کنید")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required(ErrorMessage ="لطفا شماره ثابت را وارد کنید")]
        [DataType(DataType.PhoneNumber,ErrorMessage ="لطفا شماره تلفن خود را درست وارد کنید")]
        [StringLength(maximumLength:11,MinimumLength =8)]
        public string HomeNumber { get; set; }
        [Required(ErrorMessage ="لطفا شماره موبایل خود را وارد کنید")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "لطفا شماره تلفن خود را درست وارد کنید")]
        [StringLength(maximumLength:11,MinimumLength =11)]
        public string  PhoneNumber { get; set; }
        [Required(ErrorMessage ="لطفا نام خود را وارد کنید")]
        public string FirstName { get; set; }
        [Required(ErrorMessage ="لطفا نام خانوادگی خود را وارد کنید")]
        public string LastName { get; set; }
        
    }
}
