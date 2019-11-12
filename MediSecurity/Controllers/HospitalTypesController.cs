using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MediSecurity.Data;
using MediSecurity.Data.Entities;
using Microsoft.AspNetCore.Authorization;

namespace MediSecurity.Controllers
{
    [Authorize(Roles = "Admin")]
    public class HospitalTypesController : Controller
    {
        private readonly DataContext _context;

        public HospitalTypesController(DataContext context)
        {
            _context = context;
        }

        // GET: HospitalTypes
        public async Task<IActionResult> Index()
        {
            return View(await _context.HospitalTypes.ToListAsync());
        }

        // GET: HospitalTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hospitalType = await _context.HospitalTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (hospitalType == null)
            {
                return NotFound();
            }

            return View(hospitalType);
        }

        // GET: HospitalTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: HospitalTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] HospitalType hospitalType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(hospitalType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(hospitalType);
        }

        // GET: HospitalTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hospitalType = await _context.HospitalTypes.FindAsync(id);
            if (hospitalType == null)
            {
                return NotFound();
            }
            return View(hospitalType);
        }

        // POST: HospitalTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] HospitalType hospitalType)
        {
            if (id != hospitalType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(hospitalType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HospitalTypeExists(hospitalType.Id))
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
            return View(hospitalType);
        }

        // GET: HospitalTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hospitalType = await _context.HospitalTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (hospitalType == null)
            {
                return NotFound();
            }

            return View(hospitalType);
        }

        // POST: HospitalTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var hospitalType = await _context.HospitalTypes.FindAsync(id);
            _context.HospitalTypes.Remove(hospitalType);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HospitalTypeExists(int id)
        {
            return _context.HospitalTypes.Any(e => e.Id == id);
        }
    }
}
