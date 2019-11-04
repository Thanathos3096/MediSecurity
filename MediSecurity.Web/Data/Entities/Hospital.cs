using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MediSecurity.Web.Data.Entities
{
    public class Hospital
    {
        public int Id { get; set; }

        [Display(Name = "Neighborhood")]
        [MaxLength(50, ErrorMessage = "The {0} field can not have more than {1} characters.")]
        [Required(ErrorMessage = "The field {0} is mandatory.")]
        public string Neighborhood { get; set; }

        [Display(Name = "Address")]
        [MaxLength(50, ErrorMessage = "The {0} field can not have more than {1} characters.")]
        [Required(ErrorMessage = "The field {0} is mandatory.")]
        public string Address { get; set; }

        [Required(ErrorMessage = "The field {0} is mandatory.")]
        public string Remarks { get; set; }

        [Display(Name = "Stratum")]
        [Required(ErrorMessage = "The field {0} is mandatory.")]
        public int Stratum { get; set; }

        [Display(Name = "Has Parking Lot?")]
        public bool HasParkingLot { get; set; }
    }
}
