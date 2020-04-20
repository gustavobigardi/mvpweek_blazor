using System;
using System.ComponentModel.DataAnnotations;

namespace MVPWeek.Server.Models
{
    public class Participant
    {
        [Key]
        public Guid Id { get; set; }
        
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(100)]
        public string Name { get; set; }

        public bool Selected { get; set; }
    }
}