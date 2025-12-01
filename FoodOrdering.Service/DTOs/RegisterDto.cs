using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodOrdering.Service.DTOs
{
    public class RegisterDto
    {
        [Required]
        public string FullName { get; set; } = string.Empty;
        [EmailAddress]
        public string? Email {  get; set; }
        [Required]
        public string Password { get; set; } = string.Empty ;
        [Required]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
