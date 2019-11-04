using MediSecurity.Helpers;
using MediSecurity.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediSecurity.Data
{
    public class SeedDb
    {

        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;

        public SeedDb(
            DataContext context,
            IUserHelper userHelper)
        {
            _context = context;
            _userHelper = userHelper;
        }
        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();
            await CheckRoles();
            var admin = await CheckUserAsync("1010", "Pedro", "Zapata", "zapata3096@gmail.com", "350 634 2747", "Calle Luna Calle Sol", "Admin");
            var doctor = await CheckUserAsync("2020", "Juan", "Jaramillo", "jucajama@hotmail.com", "350 634 2747", "Calle Luna Calle Sol", "Doctor");
            var patient = await CheckUserAsync("3030", "Mateo", "Roldan", "mateitoroldan@yahoo.com", "350 634 2747", "Calle Luna Calle Sol", "Patient");
            await CheckHospitalTypeAsync();
            await CheckHospitalAync();
            await CheckAdminAsync(admin);
            await CheckDoctorAsync(doctor);
            await CheckPatientAsync(patient);
            await CheckMedicalOrderAsync();
        }

        private async Task CheckMedicalOrderAsync()
        {
            var doctor = _context.Doctors.FirstOrDefault();
            var patient = _context.Patients.FirstOrDefault();
            var hospital = _context.Hospitals.FirstOrDefault();

            if (!_context.MedicalOrders.Any())
            {
                _context.MedicalOrders.Add(new MedicalOrder
                {
                    StartDate = DateTime.Today,
                    EndDate = DateTime.Today.AddYears(1),
                    Doctor = doctor,
                    Patient = patient,
                    Remarks = "Se le hara una endoscopia",
                    Price = 10M,
                    Hospital = hospital,

                });
                await _context.SaveChangesAsync();
            }
        }

        private async Task CheckHospitalAync()
        {
            var hospitaltype = _context.HospitalTypes.FirstOrDefault();
            if (!_context.Hospitals.Any())
            {
                AddHospital("Calle 43 #23 32", "Poblado", "Aqui se cuidan a los enfermos mentales", 4,hospitaltype);
                AddHospital("Calle 43 #23 32", "Poblado", "Aqui se cuidan a las embarazadas", 3,hospitaltype);
                await _context.SaveChangesAsync();
            }
        }


        private async Task CheckPatientAsync(User user)
        {
            if (!_context.Patients.Any())
            {
                _context.Patients.Add(new Patient { User = user });
                await _context.SaveChangesAsync();
            }
        }

        private async Task CheckDoctorAsync(User user)
        {
            if (!_context.Doctors.Any())
            {
                _context.Doctors.Add(new Doctor { User = user });
                await _context.SaveChangesAsync();
            }
        }

        private async Task CheckAdminAsync(User user)
        {
            if (!_context.Admins.Any())
            {
                _context.Admins.Add(new Admin { User = user });
                await _context.SaveChangesAsync();
            }
        }

        private async Task CheckRoles()
        {
            await _userHelper.CheckRoleAsync("Admin");
            await _userHelper.CheckRoleAsync("Doctor");
            await _userHelper.CheckRoleAsync("Patient");
        }

        private async Task<User> CheckUserAsync(
           string document,
           string firstName,
           string lastName,
           string email,
           string phone,
           string address,
           string role)
        {
            var user = await _userHelper.GetUserByEmailAsync(email);
            if (user == null)
            {
                user = new User
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
                    UserName = email,
                    PhoneNumber = phone,
                    Address = address,
                    Document = document

                };


                await _userHelper.AddUserAsync(user, "123456");
                await _userHelper.AddUserToRoleAsync(user, role);
            }

            return user;
        }
        private async Task CheckHospitalTypeAsync()
        {
            if (!_context.HospitalTypes.Any())
            {
                _context.HospitalTypes.Add(new HospitalType { Name = "Central" });
                _context.HospitalTypes.Add(new HospitalType { Name = "Geriatrico" });
                _context.HospitalTypes.Add(new HospitalType { Name = "Psiquiatrico" });
                await _context.SaveChangesAsync();
            }
        }
        private void AddHospital(
            string address,
            string neighborhood,
            string remarks,
            int stratum,
            HospitalType hospitalType)
        {
            _context.Hospitals.Add(new Hospital
            {
                Address = address,
                HasParkingLot = true,
                HospitalType=hospitalType,
                Neighborhood = neighborhood,
                Remarks = remarks,
                Stratum = stratum
            });
        }
    }
}
