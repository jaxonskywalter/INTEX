using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace INTEX.Models
{
    public class CreateUserViewModel
    {
        [Required]
        [Display(Name = "Username")]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at most {1} characters long.", MinimumLength = 6)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required]
        [Display(Name = "Role")]
        public string Role { get; set; }
    }
}
