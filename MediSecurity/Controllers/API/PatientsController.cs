using System.Linq;
using System.Threading.Tasks;
using MediSecurity.Common.Models;
using MediSecurity.Data;
using MediSecurity.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace MediSecurity.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientsController : ControllerBase
    {
        private readonly DataContext _dataContext;

        public PatientsController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpPost]
        [Route("GetPatientByEmail")]
        public async Task<IActionResult> GetPatientByEmailAsync(EmailRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var patient = await _dataContext.Patients
                .Include(o => o.User)
                .Include(m => m.MedicalOrders)
                .FirstOrDefaultAsync(o => o.User.Email.ToLower() == request.Email.ToLower());

            if (patient == null)
            {
                return NotFound();
            }

            var response = new PatientResponse
            {
                Id = patient.Id,
                FirstName = patient.User.FirstName,
                LastName = patient.User.LastName,
                Address = patient.User.Address,
                Document = patient.User.Document,
                Email = patient.User.Email,
                PhoneNumber = patient.User.PhoneNumber,
                MedicalOrder = patient.MedicalOrders?.Select(m => new MedicalOrderResponse
                {
                    Id = m.Id,
                    Remarks = m.Remarks,
                    Price = m.Price,
                    StartDate = m.StartDate,
                    EndDate = m.EndDate

                }).ToList()
            };


            return Ok(response);
        }
    }
}
