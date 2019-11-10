using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace MediSecurity.Helpers
{
    public interface IImageHelper
    {
        Task<string> UploadImageAsync(IFormFile imageFile);
    }

}
