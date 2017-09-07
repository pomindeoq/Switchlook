using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using WebApi.Utils.ValidationAttributes;

namespace WebApi.Models
{
    public class Account : IdentityUser
    {
        public int TestValue { get; set; }
      

    }

    public class RegisterModel
    {  
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        [Compare("Password", ErrorMessage = "Confirm password doesn't match, Type again !")]
        public string PasswordRepeat { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [BooleanRequired(true, ErrorMessage = "You have to agree to terms of use")]
        public bool TermsOfUse { get; set; }
        public bool ReceiveEmails { get; set; }
    }

    public class LoginModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }

    public interface IFacebookLoginModel
    {
        string Id { get; set; }
        string Name { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        string Email { get; set; }
    }

    public class FacebookLoginModel : IFacebookLoginModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }

    public class FacebookRegisterModel : FacebookLoginModel
    {
        public string Username { get; set; }
    }
}
