using System.ComponentModel.DataAnnotations;


namespace MediSecurity.Common.Models
{
    public class EmailRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

    }
}
