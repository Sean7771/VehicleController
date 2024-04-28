using Microsoft.AspNetCore.SignalR;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using VehicleController.Data;
using VehicleController.Models;

namespace VehicleController.SignalR.Hubs
{
    public class VehicleHub : Hub
    {
        private readonly VehicleDbContext _dbContext;

        public VehicleHub(VehicleDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public enum eStatus
        {
            Driving = 1,
            Reversing = 2,
            Stopped = 3,
        }

        public async Task UpdateVehicleStatus(int vehicleId, eStatus status)
        {
            try
            {
                var vehicle = await _dbContext.Vehicles.FindAsync(vehicleId);
                if (vehicle != null)
                {
                    
                    vehicle.StatusId = (int)status;

                   
                   Trip trip =  CreateOrUpdateTrip(vehicle, status);

                    
                    await _dbContext.SaveChangesAsync();

                    
                    
                    await Clients.All.SendAsync("VehicleStatusUpdated", vehicleId, status.ToString());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
                throw;
            }
        }
        private Trip CreateOrUpdateTrip(Vehicle vehicle, eStatus status)
        {
            Trip trip = new Trip();
            switch (status)
            {
                case eStatus.Driving:
                case eStatus.Reversing:
                    trip.VehicleId = vehicle.Id;
                    trip.StartTime = DateTime.UtcNow;
                    trip.EndTime = DateTime.UtcNow.AddMilliseconds(1);
                    trip.Distance = 0;// Assuming initial distance is 0 when the trip starts

                    _dbContext.Trips.Add(trip);
                    break;
                case eStatus.Stopped:
                    var activeTrip = _dbContext.Trips.FirstOrDefault(t => t.VehicleId == vehicle.Id && t.StartTime != null && t.Distance == 0);

                    if (activeTrip != null)
                    {
                        activeTrip.EndTime = DateTime.UtcNow;
                        var timeElapsed = activeTrip.EndTime - activeTrip.StartTime;
                        activeTrip.Distance = (decimal)CalculateDistanceTraveled(timeElapsed, vehicle.AverageSpeed);
                        _dbContext.Trips.Update(activeTrip);
                    }
                    else
                    {
                        Console.WriteLine("No active trip found");
                    }
                    break;
                default:
                    break;
            }
            return trip;
        }
        private double CalculateDistanceTraveled(TimeSpan? timeTraveled, decimal averageSpeed)
        {
            if (timeTraveled.HasValue)
            {
                
                double averageSpeedDouble = Convert.ToDouble(averageSpeed);
                return averageSpeedDouble * timeTraveled.Value.TotalHours;
            }
            else
            {
                return 0;
            }
        }
    }
}

