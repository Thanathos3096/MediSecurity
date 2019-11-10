﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MediSecurity.Helpers
{
    public class ImageHelper : IImageHelper
    {
        public async Task<string> UploadImageAsync(IFormFile imageFile)
        {
            var guid = Guid.NewGuid().ToString();
            var file = $"{guid}.jpg";
            var path = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot\\images\\Hospitals",
                file);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }

            return $"~/images/Hospitals/{file}";
        }
    }

}
