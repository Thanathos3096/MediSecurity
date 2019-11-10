using MediSecurity.Data.Entities;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace MediSecurity.Models
{
    public class ImageHospitalViewModel : ImageHospital
    {
        [Display(Name = "Image")]
        public IFormFile ImageFile { get; set; }
    }

}
