using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Exam.Models
{
    public class Login
    {
        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        public string LogEmail { get; set; }
        [Required]
        [Display(Name = "Password")]
        [MinLength(8,ErrorMessage ="Password Must be At least 8 character!")]
        [DataType(DataType.Password)]
        public string LogPassword { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}