using System.ComponentModel.DataAnnotations;

namespace MediSecurity.Models
{
    public class RecoverPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
