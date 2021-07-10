using System.ComponentModel.DataAnnotations;

namespace Exam.Models
{
    public class Join
    {
        [Key]
        public int JoinId { get; set; }
        // Foreign Keys
        public int UserId { get; set; }
        public int ActivityId { get; set; }

        // Navigation property
        public User user { get; set; }
        public ActivityT Activity { get; set; }
    }
}