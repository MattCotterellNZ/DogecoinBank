using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Geekbank.Web.Models.AccountViewModels
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

		[Display(Name = "Two Factor Token")]
		[StringLength(6, ErrorMessage = "The {0} must be exactly {1} characters long.", MinimumLength = 6)]
		public string TwoFactorToken { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}
