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

namespace MediSecurity.Controllers
{
    public class DoctorsController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly IUserHelper _userHelper;
        private readonly IMailHelper _mailHelper;

        public DoctorsController(
            DataContext datacontext,
            IUserHelper userHelper,
            IMailHelper mailHelper)
        {
            _dataContext = datacontext;
            _userHelper = userHelper;
            _mailHelper = mailHelper;
        }

        // GET: Doctors
        public IActionResult Index()
        {
            return View( _dataContext.Doctors
                .Include(d=>d.User)
                .Include(d=>d.MedicalOrders));
        }

        // GET: Doctors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var doctor = await _dataContext.Doctors
                .Include(d=>d.User)
                .Include(d=>d.MedicalOrders)
                .FirstOrDefaultAsync(d => d.Id == id);
            if (doctor == null)
            {
                return NotFound();
            }

            return View(doctor);
        }

        // GET: Doctors/Create
        public IActionResult Create()
        {
            var view = new AddUserViewModel { RoleId = 2 };
            return View();
        }

        // POST: Doctors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AddUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await CreateUserAsync(model);
                if (user != null)
                {
                    var doctor = new Doctor
                    {
                        MedicalOrders = new List<MedicalOrder>(),
                        User = user

                    };
                    _dataContext.Doctors.Add(doctor);
                    await _dataContext.SaveChangesAsync();

                    var myToken = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
                    var tokenLink = Url.Action("ConfirmEmail", "Account", new
                    {
                        userid = user.Id,
                        token = myToken
                    }, protocol: HttpContext.Request.Scheme);

                    _mailHelper.SendMail(model.Username, "MediSecurity - Email confirmation", $"<h1>Email Confirmation</h1>" +
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
            if (result.Succeeded)
            {
                user = await _userHelper.GetUserByEmailAsync(model.Username);
                await _userHelper.AddUserToRoleAsync(user, "Patient");
                return user;
            }
            return null;
        }
        // GET: Doctors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var doctor = await _dataContext.Doctors
                .Include(d => d.User)
                .FirstOrDefaultAsync(d => d.Id == id.Value);
            if (doctor == null)
            {
                return NotFound();
            }

            var model = new EditUserViewModel
            {
                Address = doctor.User.Address,
                Document = doctor.User.Document,
                FirstName = doctor.User.FirstName,
                Id = doctor.Id,
                LastName = doctor.User.LastName,
                PhoneNumber = doctor.User.PhoneNumber
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var doctor = await _dataContext.Doctors
                    .Include(o => o.User)
                    .FirstOrDefaultAsync(o => o.Id == model.Id);

                doctor.User.Document = model.Document;
                doctor.User.FirstName = model.FirstName;
                doctor.User.LastName = model.LastName;
                doctor.User.Address = model.Address;
                doctor.User.PhoneNumber = model.PhoneNumber;

                await _userHelper.UpdateUserAsync(doctor.User);
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        // GET: Doctors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var doctor = await _dataContext.Doctors
                .FirstOrDefaultAsync(m => m.Id == id);
            if (doctor == null)
            {
                return NotFound();
            }

            return View(doctor);
        }

        // POST: Doctors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var doctor = await _dataContext.Doctors.FindAsync(id);
            _dataContext.Doctors.Remove(doctor);
            await _dataContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DoctorExists(int id)
        {
            return _dataContext.Doctors.Any(e => e.Id == id);
        }


    }
}
