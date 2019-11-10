﻿using MediSecurity.Data;
using MediSecurity.Data.Entities;
using MediSecurity.Helpers;
using MediSecurity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediSecurity.Controllers
{
    [Authorize(Roles ="Admin")]
    public class HospitalsController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly IUserHelper _userHelper;
        private readonly IImageHelper _imageHelper;

        public HospitalsController(
            DataContext dataContext,
            IUserHelper userHelper,
            IImageHelper imageHelper)
        {
            _dataContext = dataContext;
            _userHelper = userHelper;
            _imageHelper = imageHelper;
        }

        // GET: Hospitals
        public IActionResult Index()
        {
            return View(_dataContext.Hospitals
                .Include(d => d.HospitalType)
                .Include(d => d.ImageHospitals));
        }

        // GET: Hospitals/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hospital = await _dataContext.Hospitals
                .Include(h=>h.ImageHospitals)
                .Include(h=>h.HospitalType)
                .FirstOrDefaultAsync(h => h.Id == id);
            if (hospital == null)
            {
                return NotFound();
            }

            return View(hospital);
        }

        // GET: Hospitals/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Hospitals/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Neighborhood,Address,Remarks,Stratum,HasParkingLot")] Hospital hospital)
        {
            if (ModelState.IsValid)
            {
                _dataContext.Add(hospital);
                await _dataContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(hospital);
        }

        // GET: Hospitals/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hospital = await _dataContext.Hospitals.FindAsync(id);
            if (hospital == null)
            {
                return NotFound();
            }
            return View(hospital);
        }

        // POST: Hospitals/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Neighborhood,Address,Remarks,Stratum,HasParkingLot")] Hospital hospital)
        {
            if (id != hospital.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _dataContext.Update(hospital);
                    await _dataContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HospitalExists(hospital.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(hospital);
        }

        // GET: Hospitals/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hospital = await _dataContext.Hospitals
                .FirstOrDefaultAsync(m => m.Id == id);
            if (hospital == null)
            {
                return NotFound();
            }

            return View(hospital);
        }

        // POST: Hospitals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var hospital = await _dataContext.Hospitals.FindAsync(id);
            _dataContext.Hospitals.Remove(hospital);
            await _dataContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HospitalExists(int id)
        {
            return _dataContext.Hospitals.Any(e => e.Id == id);
        }
        public async Task<IActionResult> AddImage(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hospital = await _dataContext.Hospitals.FindAsync(id.Value);
            if (hospital == null)
            {
                return NotFound();
            }

            var model = new ImageHospitalViewModel
            {
                Id = hospital.Id
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddImage(ImageHospitalViewModel model)
        {
            if (ModelState.IsValid)
            {
                var path = string.Empty;

                if (model.ImageFile != null)
                {
                    path = await _imageHelper.UploadImageAsync(model.ImageFile);
                }

                var hospitalImage = new ImageHospital
                {
                    ImageUrl = path,
                    Hospital = await _dataContext.Hospitals.FindAsync(model.Id)
                };

                _dataContext.ImageHospitals.Add(hospitalImage);
                await _dataContext.SaveChangesAsync();
                return RedirectToAction($"{nameof(Details)}/{model.Id}");
            }

            return View(model);
        }

        public async Task<IActionResult> DeleteImage(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hospitalImage = await _dataContext.ImageHospitals
                .Include(h => h.Hospital)
                .FirstOrDefaultAsync(h => h.Id == id.Value);
            if (hospitalImage == null)
            {
                return NotFound();
            }

            _dataContext.ImageHospitals.Remove(hospitalImage);
            await _dataContext.SaveChangesAsync();
            return RedirectToAction($"{nameof(Details)}/{hospitalImage.Hospital.Id}");
        }

    }
}

