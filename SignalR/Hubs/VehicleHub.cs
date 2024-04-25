using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using VehicleController.Data;

namespace VehicleController.SignalR.Hubs
{
    public class VehicleHub : Hub
    {
        private readonly VehicleDbContext _dbContext;

        public VehicleHub(VehicleDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task DriveVehicle(int vehicleId)
        {
            var vehicle = await _dbContext.Vehicles.FindAsync(vehicleId);
            if (vehicle != null)
            {
                var trip = await _dbContext.Trips
                    .Where(t => t.VehicleId == vehicleId)
                    .OrderByDescending(t => t.StartTime)
                    .FirstOrDefaultAsync();

                if (trip != null)
                {
                    // Use the start time from the associated trip record
                    var startTime = trip.StartTime;

                    // Update vehicle status to "Driving"
                    vehicle.StatusId = 1;

                    // Save changes to the database
                    await _dbContext.SaveChangesAsync();

                    // Notify clients about the drive start
                    await Clients.All.SendAsync("VehicleDriven", vehicleId);

                    // Optionally, you can also send the start time back to the client
                    await Clients.All.SendAsync("DriveStarted", vehicleId, startTime);
                }
            }
        }

        public async Task StopVehicle(int vehicleId)
        {
            var vehicle = await _dbContext.Vehicles.FindAsync(vehicleId);
            if (vehicle != null)
            {
                // Update vehicle status to "Stopped"
                vehicle.StatusId = 2;
                await _dbContext.SaveChangesAsync();

                // Notify clients about the vehicle being stopped
                await Clients.All.SendAsync("VehicleStopped", vehicleId);
            }
        }

        public async Task ReverseVehicle(int vehicleId)
        {
            var vehicle = await _dbContext.Vehicles.FindAsync(vehicleId);
            if (vehicle != null)
            {
                // Update vehicle status to "Reversed"
                vehicle.StatusId = 3;
                await _dbContext.SaveChangesAsync();

                // Notify clients about the vehicle being reversed
                await Clients.All.SendAsync("VehicleReversed", vehicleId);
            }
        }
    }
}