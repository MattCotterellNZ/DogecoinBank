using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Geekbank.Web.Models.AccountViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [StringLength(30, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 2)]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [StringLength(500, ErrorMessage = "The {0} must be at max {1} characters long.")]
        [Display(Name = "Address")]
        [DataType(DataType.MultilineText)]
        public string Address { get; set; }

        [Required]
        [Phone]
        [StringLength(20, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 7)]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Date Of Birth")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [Display(Name = "Gender")]
        public string Gender { get; set; }

        public List<SelectListItem> Genders { get; } = new List<SelectListItem>
        {
            new SelectListItem { Value = "male", Text = "Male" },
            new SelectListItem { Value = "female", Text = "Female" },
        };

        [Required]
        [Display(Name = "Gender")]
        public string MaritalStatus { get; set; }

        public List<SelectListItem> MaritalStatuses { get; } = new List<SelectListItem>
        {
            new SelectListItem { Value = "single", Text = "Single" },
            new SelectListItem { Value = "dating", Text = "Dating" },
            new SelectListItem { Value = "engaged", Text = "Engaged" },
            new SelectListItem { Value = "married", Text = "Married" },
            new SelectListItem { Value = "widowed", Text = "Widowed" },
        };

        [Required]
        [Display(Name = "Height (in cm)")]
        public int Height { get; set; }

        [Required]
        [Display(Name = "Weight (in kg)")]
        public int Weight { get; set; }
    }
}
