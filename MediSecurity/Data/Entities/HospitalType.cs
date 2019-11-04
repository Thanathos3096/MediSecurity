using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MediSecurity.Data.Entities
{
    public class HospitalType
    {
        public int Id { get; set; }

        [Display(Name = "Hospital Type")]
        [MaxLength(50, ErrorMessage = "The {0} field can not have more than {1} characters.")]
        [Required(ErrorMessage = "The field {0} is mandatory.")]
        public string Name { get; set; }

        public ICollection<Hospital> Hospitals { get; set; }
    }
}
