using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VehicleController.Data;
using VehicleController.Models;
using VehicleController.SignalR.Hubs;

namespace VehicleController.Controllers
{
    public class CarsController : Controller
    {
        private readonly VehicleDbContext _context;
        

        public CarsController(VehicleDbContext context )
        {
            _context = context;
            
        }

        // GET: Cars
        public async Task<IActionResult> Index()
        {
            var vehicleDbContext = _context.Cars.Include(c => c.Make).Include(c => c.Model).Include(c => c.Vehicle);
            return View(await vehicleDbContext.ToListAsync());
        }

        // GET: Cars/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Cars == null)
            {
                return NotFound();
            }

            var car = await _context.Cars
                .Include(c => c.Make)
                .Include(c => c.Model)
                .Include(c => c.Vehicle)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (car == null)
            {
                return NotFound();
            }

            return View(car);
        }

        // GET: Cars/Create
        public IActionResult Create()
        {
            ViewData["MakeId"] = new SelectList(_context.Makes, "Id", "Id");
            ViewData["ModelId"] = new SelectList(_context.Models, "Id", "Id");
            ViewData["VehicleId"] = new SelectList(_context.Vehicles, "Id", "Id");
            return View();
        }

        // POST: Cars/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,VehicleId,MakeId,ModelId,NumberOfDoors,Color")] Car car)
        {
            if (ModelState.IsValid)
            {
                _context.Add(car);
                await _context.SaveChangesAsync();

                

                return RedirectToAction(nameof(Index));
            }
            ViewData["MakeId"] = new SelectList(_context.Makes, "Id", "Id", car.MakeId);
            ViewData["ModelId"] = new SelectList(_context.Models, "Id", "Id", car.ModelId);
            ViewData["VehicleId"] = new SelectList(_context.Vehicles, "Id", "Id", car.VehicleId);
            return View(car);
        }

        // GET: Cars/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Cars == null)
            {
                return NotFound();
            }

            var car = await _context.Cars.FindAsync(id);
            if (car == null)
            {
                return NotFound();
            }
            ViewData["MakeId"] = new SelectList(_context.Makes, "Id", "Id", car.MakeId);
            ViewData["ModelId"] = new SelectList(_context.Models, "Id", "Id", car.ModelId);
            ViewData["VehicleId"] = new SelectList(_context.Vehicles, "Id", "Id", car.VehicleId);
            return View(car);
        }

        // POST: Cars/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,VehicleId,MakeId,ModelId,NumberOfDoors,Color")] Car car)
        {
            if (id != car.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(car);
                    await _context.SaveChangesAsync();

                    
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CarExists(car.Id))
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
            ViewData["MakeId"] = new SelectList(_context.Makes, "Id", "Id", car.MakeId);
            ViewData["ModelId"] = new SelectList(_context.Models, "Id", "Id", car.ModelId);
            ViewData["VehicleId"] = new SelectList(_context.Vehicles, "Id", "Id", car.VehicleId);
            return View(car);
        }
        private bool CarExists(int id)
        {
          return (_context.Cars?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
