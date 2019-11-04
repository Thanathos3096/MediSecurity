using MediSecurity.Data.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MediSecurity.Models
{
    public class HospitalViewModel : Hospital
    {
        [Required(ErrorMessage = "The field {0} is mandatory.")]
        [Display(Name = "Property Type")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select a property type.")]
        public int HospitalTypeId { get; set; }

        public IEnumerable<SelectListItem> PropertyTypes { get; set; }
    }

}

