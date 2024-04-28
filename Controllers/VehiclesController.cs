using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using VehicleController.Data;
using VehicleController.Models;
using VehicleController.SignalR.Hubs;

namespace VehicleController.Controllers
{
    public class VehiclesController : Controller
    {
        private readonly VehicleDbContext _dbContext;
        private readonly IHubContext<VehicleHub> _hubContext;

        public VehiclesController(VehicleDbContext dbContext, IHubContext<VehicleHub> hubContext)
        {
            _dbContext = dbContext;
            _hubContext = hubContext;
        }

        public async Task<IActionResult> Index()
        {
            var vehicles = await _dbContext.Vehicles.ToListAsync();
            return View(vehicles);
        } 
    }
}

