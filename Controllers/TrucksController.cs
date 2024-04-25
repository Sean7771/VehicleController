using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VehicleController.Data;
using VehicleController.Models;

namespace VehicleController.Controllers
{
    public class TrucksController : Controller
    {
        private readonly VehicleDbContext _context;

        public TrucksController(VehicleDbContext context)
        {
            _context = context;
        }

        // GET: Trucks
        public async Task<IActionResult> Index()
        {
            var vehicleDbContext = _context.Trucks.Include(t => t.Vehicle);
            return View(await vehicleDbContext.ToListAsync());
        }

        // GET: Trucks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Trucks == null)
            {
                return NotFound();
            }

            var truck = await _context.Trucks
                .Include(t => t.Vehicle)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (truck == null)
            {
                return NotFound();
            }

            return View(truck);
        }

        // GET: Trucks/Create
        public IActionResult Create()
        {
            ViewData["VehicleId"] = new SelectList(_context.Vehicles, "Id", "Id");
            return View();
        }

        // POST: Trucks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,VehicleId,CargoCapacity,HasTrailer")] Truck truck)
        {
            if (ModelState.IsValid)
            {
                _context.Add(truck);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["VehicleId"] = new SelectList(_context.Vehicles, "Id", "Id", truck.VehicleId);
            return View(truck);
        }

        // GET: Trucks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Trucks == null)
            {
                return NotFound();
            }

            var truck = await _context.Trucks.FindAsync(id);
            if (truck == null)
            {
                return NotFound();
            }
            ViewData["VehicleId"] = new SelectList(_context.Vehicles, "Id", "Id", truck.VehicleId);
            return View(truck);
        }

        // POST: Trucks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,VehicleId,CargoCapacity,HasTrailer")] Truck truck)
        {
            if (id != truck.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(truck);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TruckExists(truck.Id))
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
            ViewData["VehicleId"] = new SelectList(_context.Vehicles, "Id", "Id", truck.VehicleId);
            return View(truck);
        }

        
        private bool TruckExists(int id)
        {
          return (_context.Trucks?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
