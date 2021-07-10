using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Exam.Models
{
    public class ActivityT
    {
        [Key]
        [Required]
        public int ActivityId { get; set; }
        [Required]
        [Display(Name = "Activity Title")]
        public string Title { get; set; }
        [Required]
        public DateTime ActivityDate { get; set; }
        [Required]
        [Display(Name = "Activity Time")]
        public string ActivityTime { get; set; }
        [Required]
        [Display(Name = "Activity Duration")]
        public string ActivityDuration { get; set; }
        [Required]
        public string Time { get; set; }
        [Required]
        public string Duration { get; set; }
        [Required]
        [Display(Name = "Description")]
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        // FK
        public int UserId { get; set; }
        // Navigation property
        public User Coordinator { get; set; }
        public List<Join> Attends { get; set; }
    }
}