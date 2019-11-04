using MediSecurity.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediSecurity.Data
{
    public class DataContext : IdentityDbContext<User>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Hospital> Hospitals { get; set; }
        public DbSet<HospitalType> HospitalTypes { get; set; }
        public DbSet<ImageHospital> ImageHospitals { get; set; }
        public DbSet<ImageUser> ImageUsers { get; set; }
        public DbSet<MedicalOrder> MedicalOrders { get; set; }
        public DbSet<Patient> Patients { get; set; }
    }
}
