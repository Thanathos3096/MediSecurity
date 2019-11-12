using MediSecurity.Data;
using MediSecurity.Data.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace MediSecurity.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class HospitalTypesController : ControllerBase
    {
        private readonly DataContext _context;

        public HospitalTypesController(DataContext context)
        {
            _context = context;
        }

        // GET: api/HospitalTypes
        [HttpGet]
        public IEnumerable<HospitalType> GetHospitalTypes()
        {
            return _context.HospitalTypes.OrderBy(hp => hp.Name);
        }
    }
}