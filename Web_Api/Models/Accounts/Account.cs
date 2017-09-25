using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using WebApi.Utils.ValidationAttributes;

namespace WebApi.Models.Accounts
{
    
    public class Account : IdentityUser
    {
        public int TestValue { get; set; }

        public void SetUpPoints(WebApiDataContext context)
        {
            context.Points.Add(new Points { Account = this, Value = 0 });
        }

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

    public interface IExternalDataModel
    {
        string Id { get; set; }
        string Name { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        string Email { get; set; }
    }
    public interface IExternalLoginModel
    {
        string AccessToken { get; set; }
    }

    // Facebook
    public class FacebookDataModel : IExternalDataModel
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("first_name")]
        public string FirstName { get; set; }
        [JsonProperty("last_name")]
        public string LastName { get; set; }
        [JsonProperty("email")]
        public string Email { get; set; }
    }

    public class FacebookLoginModel : IExternalLoginModel
    {
        public string AccessToken { get; set; }
    }

    public class FacebookRegisterModel : IExternalLoginModel
    {
        public string AccessToken { get; set; }
        public string Username { get; set; }
    }

    // Google
    public class GoogleDataModel : IExternalDataModel
    {
        [JsonProperty("sub")]
        public string Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("given_name")]
        public string FirstName { get; set; }
        [JsonProperty("family_name")]
        public string LastName { get; set; }
        [JsonProperty("email")]
        public string Email { get; set; }
    }

    public class GoogleLoginModel : IExternalLoginModel
    {
        public string AccessToken { get; set; }
    }

    public class GoogleRegisterModel
    {
        public string AccessToken { get; set; }
        public string Username { get; set; }
    }
}
