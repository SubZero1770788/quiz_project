using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace quiz_project.Models
{
    public class RegisterViewModel
    {
        public required string UserName { get; set; }
        public required string Email { get; set; }

        [DataType(DataType.Password)]
        public required string Password { get; set; }
        
        [DataType(DataType.Password)]
        [Display(Name = "Confirm your password")]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public required string ConfirmedPassword { get; set; }
    }
}