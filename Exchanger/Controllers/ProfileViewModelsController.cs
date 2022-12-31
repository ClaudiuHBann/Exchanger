using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Exchanger.Data;
using Exchanger.Models.Profile;

namespace Exchanger.Controllers
{
    public class ProfileViewModelsController : Controller
    {
        private readonly ExchangerContext _context;

        public ProfileViewModelsController(ExchangerContext context)
        {
            _context = context;
        }

        // GET: ProfileViewModels
        public async Task<IActionResult> Index()
        {
              return View(await _context.Profiles.ToListAsync());
        }

        // GET: ProfileViewModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Profiles == null)
            {
                return NotFound();
            }

            var profileViewModel = await _context.Profiles
                .FirstOrDefaultAsync(m => m.Id == id);
            if (profileViewModel == null)
            {
                return NotFound();
            }

            return View(profileViewModel);
        }

        // GET: ProfileViewModels/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ProfileViewModels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,IdAccount,Avatar,Name,Description,Phone,Email,Country,City,Rating")] Profile profileViewModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(profileViewModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(profileViewModel);
        }

        // GET: ProfileViewModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Profiles == null)
            {
                return NotFound();
            }

            var profileViewModel = await _context.Profiles.FindAsync(id);
            if (profileViewModel == null)
            {
                return NotFound();
            }
            return View(profileViewModel);
        }

        // POST: ProfileViewModels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IdAccount,Avatar,Name,Description,Phone,Email,Country,City,Rating")] Profile profileViewModel)
        {
            if (id != profileViewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(profileViewModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProfileViewModelExists(profileViewModel.Id))
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
            return View(profileViewModel);
        }

        // GET: ProfileViewModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Profiles == null)
            {
                return NotFound();
            }

            var profileViewModel = await _context.Profiles
                .FirstOrDefaultAsync(m => m.Id == id);
            if (profileViewModel == null)
            {
                return NotFound();
            }

            return View(profileViewModel);
        }

        // POST: ProfileViewModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Profiles == null)
            {
                return Problem("Entity set 'ExchangerContext.Profile'  is null.");
            }
            var profileViewModel = await _context.Profiles.FindAsync(id);
            if (profileViewModel != null)
            {
                _context.Profiles.Remove(profileViewModel);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProfileViewModelExists(int id)
        {
          return _context.Profiles.Any(e => e.Id == id);
        }
    }
}
