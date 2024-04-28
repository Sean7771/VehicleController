using Microsoft.AspNetCore.SignalR;
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

                   
                   //Trip trip =  CreateOrUpdateTrip(vehicle, status);

                    
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
            Trip trip = null;

            switch (status)
            {
                case eStatus.Driving:
                case eStatus.Reversing:
                    // Create a new trip
                    trip = new Trip
                    {
                        VehicleId = vehicle.Id,
                        StartTime = DateTime.UtcNow,
                        EndTime  = null,
                        Distance = 0 // You need to calculate this based on your logic
                    };
                    _dbContext.Trips.Add(trip);
                    break;
                case eStatus.Stopped:
                    // Find the active trip and update end time
                    var activeTrip = _dbContext.Trips.FirstOrDefault(t => t.VehicleId == vehicle.Id && t.EndTime == null);
                    if (activeTrip != null)
                    {
                        activeTrip.EndTime = DateTime.UtcNow;
                        // Calculate distance traveled based on the time elapsed
                        var timeElapsed = activeTrip.EndTime - activeTrip.StartTime;
                        activeTrip.Distance = (decimal)CalculateDistanceTraveled(timeElapsed, vehicle.AverageSpeed); // You need to implement this method
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
                // Convert averageSpeed to double before calculation
                double averageSpeedDouble = Convert.ToDouble(averageSpeed);

                // Calculate distance traveled based on time traveled and average speed
                // For simplicity, let's assume distance traveled is equal to average speed times time traveled
                return averageSpeedDouble * timeTraveled.Value.TotalHours;
            }
            else
            {
                // If timeTraveled is null, return 0 distance
                return 0;
            }
        }
    }
}

