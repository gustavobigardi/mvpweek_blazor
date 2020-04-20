using System.ComponentModel.DataAnnotations;

namespace MVPWeek.Client.Models
{
    public class Raffle
    {
        [Required]
        public string Password { get; set; }
    }
}