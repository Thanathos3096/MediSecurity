using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MediSecurity.Data.Entities
{
    public class ImageHospital
    {
        public int Id { get; set; }

        [Display(Name = "Image")]
        public string ImageUrl { get; set; }
        // TODO: Change the path when publish
        public string ImageFullPath => string.IsNullOrEmpty(ImageUrl)
            ? null
            : $"https://medisecurity.azurewebsites.net{ImageUrl.Substring(1)}";

        public Hospital Hospital { get; set; }
    }
}
