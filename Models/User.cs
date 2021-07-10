using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Exam.Models
{
    public class User
    {
        [Key]
        [Required]
        public int UserId { get; set; }
        [Required]
        [Display(Name = "First Name")]
        [MinLength(2,ErrorMessage ="First Name Must be At least 2 character!")]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "Last Name")]
        [MinLength(2,ErrorMessage ="Last Name Must be At least 2 character!")]
        public string LastName { get; set; }
        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [Display(Name = "Password")]
        [MinLength(8,ErrorMessage ="Password Must be At least 8 character!")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)\S{6,20}$",  ErrorMessage = "Password Must contain at least 1 number, 1 capital and small letter, and a special character")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [NotMapped]
        [Compare("Password")]
        [Display(Name = "Confirm Password")]
        [MinLength(8,ErrorMessage ="Confirm Must be At least 8 character!")]
        [DataType(DataType.Password)]
        public string Confirm {get;set;}
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        // Navigation property
        public List<ActivityT> Activities { get; set; }
        public List<Join> Joins { get; set; }

    }
}