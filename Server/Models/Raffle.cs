using System.ComponentModel.DataAnnotations;

namespace MVPWeek.Server.Models
{
    public class Raffle
    {
        [Required]
        public string Password { get; set; }
    }
}