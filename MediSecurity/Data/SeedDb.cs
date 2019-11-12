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
            var admin = await CheckUserAsync("1010", "Mateo", "Zapata", "zapata3096@gmail.com", "350 634 2747", "Calle Luna Calle Sol", "Admin");
            var doctor = await CheckUserAsync("2020", "Juan", "Jaramillo", "jucajama@hotmail.com", "350 634 2747", "Calle Luna Calle Sol", "Doctor");
            var doctor2 = await CheckUserAsync("5050", "Maria", "Velez", "velez@hotmail.com", "350 899 5000", "Calle Milagrosa", "Doctor");
            var doctor3 = await CheckUserAsync("2020", "Diego", "Restrepo", "diegitores@hotmail.com", "350 444 5049", "Calle La Vencida", "Doctor");
            var patient = await CheckUserAsync("3030", "Mateo", "Roldan", "mateitoroldan@yahoo.com", "350 634 2747", "Calle Luna Calle Sol", "Patient");
            var patient2 = await CheckUserAsync("3950", "Carlos", "Zapata", "carloszapata@yahoo.com", "350 454 5789", "Calle 45 99 ", "Patient");
            var patient3 = await CheckUserAsync("4004", "Ana", "Cardona", "anavelez@yahoo.com", "398 457 8999", "Calle Buena Vida ", "Patient");
            await CheckHospitalTypeAsync();
            await CheckHospitalTypeAsync();
            await CheckHospitalAync();
            await CheckAdminAsync(admin);
            await CheckDoctorAsync(doctor);
            await CheckDoctorAsync2(doctor2);
            await CheckDoctorAsync3(doctor3);
            await CheckPatientAsync(patient);
            await CheckPatientAsync2(patient2);
            await CheckPatientAsync3(patient3);
            await CheckMedicalOrderAsync();
        }

        private async Task CheckMedicalOrderAsync()
        {
            var doctor = _context.Doctors.FirstOrDefault();
            var doctor2 = _context.Doctors.FirstOrDefault();
            var patient = _context.Patients.FirstOrDefault();
            var patient2 = _context.Patients.FirstOrDefault();
            var hospital = _context.Hospitals.LastOrDefault();

            if (!_context.MedicalOrders.Any())
            {
                _context.MedicalOrders.Add(new MedicalOrder
                {
                    StartDate = DateTime.Today,
                    EndDate = DateTime.Today.AddDays(1),
                    Doctor = doctor,
                    Patient = patient,
                    Remarks = "Se le hara una endoscopia",
                    Price = 10M,
                    Hospital = hospital,

                });
                _context.MedicalOrders.Add(new MedicalOrder
                {
                    StartDate = DateTime.Today,
                    EndDate = DateTime.Today.AddDays(1),
                    Doctor = doctor,
                    Patient = patient,
                    Remarks = "Se le hara examenes generales",
                    Price = 5M,
                    Hospital = hospital,

                });
                _context.MedicalOrders.Add(new MedicalOrder
                {
                    StartDate = DateTime.Today,
                    EndDate = DateTime.Today.AddDays(1),
                    Doctor = doctor,
                    Patient = patient,
                    Remarks = "Se le hara una limpieza de boca",
                    Price = 8M,
                    Hospital = hospital,

                });
                _context.MedicalOrders.Add(new MedicalOrder
                {
                    StartDate = DateTime.Today,
                    EndDate = DateTime.Today.AddDays(1),
                    Doctor = doctor2,
                    Patient = patient2,
                    Remarks = "Tratamiento para poder caminar de nuevo",
                    Price = 8M,
                    Hospital = hospital,

                });
                await _context.SaveChangesAsync();
            }
        }

        private async Task CheckHospitalAync()
        {
            var hospitaltype = _context.HospitalTypes.FirstOrDefault();
            var hospitaltype2 = _context.HospitalTypes.LastOrDefault();

            if (!_context.Hospitals.Any())
            {
                AddHospital("Calle 43 #23 32", "Poblado", "Aqui se cuidan a los quemados", 4,hospitaltype2);
                AddHospital("Calle 50 #50-50", "San Ignacio", "Aqui se cuidan a las embarazadas", 3,hospitaltype);
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
        private async Task CheckPatientAsync2(User user)
        {
            if (!_context.Patients.Any())
            {
                _context.Patients.Add(new Patient { User = user });
                await _context.SaveChangesAsync();
            }
        }
        private async Task CheckPatientAsync3(User user)
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
        private async Task CheckDoctorAsync2(User user)
        {
            if (!_context.Doctors.Any())
            {
                _context.Doctors.Add(new Doctor { User = user });
                await _context.SaveChangesAsync();
            }
        }
        private async Task CheckDoctorAsync3(User user)
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

                var token = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
                await _userHelper.ConfirmEmailAsync(user, token);
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
