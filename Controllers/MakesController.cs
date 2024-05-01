using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VehicleController.Data;
using VehicleController.Models;
using VehicleController.SignalR.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace VehicleController.Controllers
{
    public class MakesController : Controller
    {
        private readonly VehicleDbContext _context;
        private readonly IHubContext<MakesHub> _hubContext;

        public MakesController(VehicleDbContext context, IHubContext<MakesHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        // GET: Makes
        public async Task<IActionResult> Index()
        {
            var makes = await _context.Makes.ToListAsync();
            if (makes == null)
            {
                return Problem("Entity set 'VehicleDbContext.Makes' is null.");
            }
            return View(makes);
        }

        // GET: Makes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var make = await _context.Makes.FirstOrDefaultAsync(m => m.Id == id);
            if (make == null)
            {
                return NotFound();
            }

            return View(make);
        }

        // GET: Makes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Makes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Make make)
        {
            if (ModelState.IsValid)
            {
                _context.Add(make);
                await _context.SaveChangesAsync();

                await _hubContext.Clients.All.SendAsync("MakeAdded", make.Name, make.Id);

                return RedirectToAction(nameof(Index));
            }
            return View(make);
        }

        // GET: Makes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var make = await _context.Makes.FindAsync(id);
            if (make == null)
            {
                return NotFound();
            }
            return View(make);
        }

        // POST: Makes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Make make)
        {
            if (id != make.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(make);
                    await _context.SaveChangesAsync();

                    await _hubContext.Clients.All.SendAsync("MakeEdited", make.Name, make.Id);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MakeExists(make.Id))
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
            return View(make);
        }

        // GET: Makes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var make = await _context.Makes.FirstOrDefaultAsync(m => m.Id == id);
            if (make == null)
            {
                return NotFound();
            }

            return View(make);
        }

        // POST: Makes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var make = await _context.Makes.FindAsync(id);
            if (make == null)
            {
                return NotFound();
            }

            _context.Makes.Remove(make);
            await _context.SaveChangesAsync();

            await _hubContext.Clients.All.SendAsync("MakeDeleted", id);

            return RedirectToAction(nameof(Index));
        }

        private bool MakeExists(int id)
        {
            return _context.Makes.Any(e => e.Id == id);
        }
    }
}
