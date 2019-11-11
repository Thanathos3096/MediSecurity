using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MediSecurity.Data;
using MediSecurity.Data.Entities;
using MediSecurity.Models;
using MediSecurity.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace MediSecurity.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PatientsController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly IUserHelper _userHelper;
        private readonly IMailHelper _mailHelper;

        public PatientsController(
            DataContext datacontext,
            IUserHelper userHelper,
            IMailHelper mailHelper)
        {
            _dataContext = datacontext;
            _userHelper = userHelper;
            _mailHelper = mailHelper;
        }

        // GET: Patients
        public IActionResult Index()
        {
            return View( _dataContext.Patients
                .Include(p=>p.User)
                .Include(p=>p.MedicalOrders));
        }

        // GET: Patients/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patient = await _dataContext.Patients
                .Include(d => d.User)
                .Include(d => d.MedicalOrders)
                .FirstOrDefaultAsync(d => d.Id == id);
            if (patient == null)
            {
                return NotFound();
            }

            return View(patient);
        }

        // GET: Patients/Create
        public IActionResult Create()
        {
            var view = new AddUserViewModel { RoleId = 1 };
            return View();
        }

        // POST: Patients/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AddUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await CreateUserAsync(model);
                if(user !=null)
                {
                    var patient = new Patient
                    {
                      MedicalOrders = new List<MedicalOrder>(),
                      User=user 
                      
                    };
                    _dataContext.Patients.Add(patient);
                    await _dataContext.SaveChangesAsync();

                    var myToken = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
                    var tokenLink = Url.Action("ConfirmEmail", "Account", new
                    {
                        userid = user.Id,
                        token = myToken
                    }, protocol: HttpContext.Request.Scheme);

                    _mailHelper.SendMail(model.Username, "MediSegurity - Email confirmation", $"<h1>Email Confirmation</h1>" +
                    $"To allow the user, " +
                    $"plase click in this link:</br></br><a href = \"{tokenLink}\">Confirm Email</a>");
                    ViewBag.Message = "The instructions to allow your user has been sent to email.";
                    

                    return RedirectToAction("Index");
                }
                ModelState.AddModelError(string.Empty, "User with this email already exist");
            }
            return View(model);
        }

        private async Task<User> CreateUserAsync(AddUserViewModel model)
        {
            var user = new User
            {
                Address = model.Address,
                Document = model.Document,
                Email = model.Username,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.PhoneNumber,
                UserName = model.Username
            };

            var result = await _userHelper.AddUserAsync(user, model.Password);
            if(result.Succeeded)
            {
                user = await _userHelper.GetUserByEmailAsync(model.Username);
                await _userHelper.AddUserToRoleAsync(user, "Patient");
                return user;
            }
            return null;
        }

        // GET: Patients/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patient = await _dataContext.Patients
                .Include(p => p.User)
                .FirstOrDefaultAsync(p => p.Id == id.Value);
            if (patient == null)
            {
                return NotFound();
            }

            var model = new EditUserViewModel
            {
                Address = patient.User.Address,
                Document = patient.User.Document,
                FirstName = patient.User.FirstName,
                Id = patient.Id,
                LastName = patient.User.LastName,
                PhoneNumber = patient.User.PhoneNumber
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var patient = await _dataContext.Patients
                    .Include(p => p.User)
                    .FirstOrDefaultAsync(p => p.Id == model.Id);

                patient.User.Document = model.Document;
                patient.User.FirstName = model.FirstName;
                patient.User.LastName = model.LastName;
                patient.User.Address = model.Address;
                patient.User.PhoneNumber = model.PhoneNumber;

                await _userHelper.UpdateUserAsync(patient.User);
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        // GET: Patients/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patient = await _dataContext.Patients
                .FirstOrDefaultAsync(m => m.Id == id);
            if (patient == null)
            {
                return NotFound();
            }

            return View(patient);
        }

        // POST: Patients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var patient = await _dataContext.Patients.FindAsync(id);
            _dataContext.Patients.Remove(patient);
            await _dataContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PatientExists(int id)
        {
            return _dataContext.Patients.Any(e => e.Id == id);
        }
    }
}
