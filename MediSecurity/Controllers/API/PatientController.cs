using MediSecurity.Common.Models;
using MediSecurity.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediSecurity.Controllers.API
{
    [Route("api/[Controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PatientController : ControllerBase
    {
        private readonly DataContext _dataContext;

        public PatientController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        [HttpPost]
        [Route("GetPatientByEmail")]
        public async Task<IActionResult> GetOwnerByEmail(EmailRequest request)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest();
            }
            var patient = await _dataContext.Patients
                .Include(p=>p.User)
                .Include(p=>p.MedicalOrders)
                .FirstOrDefaultAsync(p => p.User.Email.ToLower() == request.Email);

            if(patient==null)
            {
                return NotFound();
            }
            return Ok();
        }
    }
}
