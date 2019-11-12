using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MediSecurity.Data;
using MediSecurity.Data.Entities;

namespace MediSecurity.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
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