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

                    await CreateOrUpdateTrip(vehicle, status);

                    await _dbContext.SaveChangesAsync();

                    await Clients.All.SendAsync("VehicleStatusUpdated", vehicleId, status.ToString(), vehicle.DistanceDriven, vehicle.DistanceReversed); ;
                }
            }
            catch (Exception ex)
            {
                
                Console.WriteLine("An error occurred: " + ex.Message);
                throw;
            }
        }

        private async Task CreateOrUpdateTrip(Vehicle vehicle, eStatus status)
        {
            switch (status)
            {
                case eStatus.Driving:
                case eStatus.Reversing:
                    await CreateTrip(vehicle, status);
                    break;
                case eStatus.Stopped:
                    await UpdateActiveTrip(vehicle);
                    break;
                default:
                    break;
            }
        }

        private async Task CreateTrip(Vehicle vehicle, eStatus status)
        {
            var trip = new Trip
            {
                VehicleId = vehicle.Id,
                StartTime = DateTime.Now,
                EndTime = null,
                Distance = 0,
                TripType = status.ToString() 
            };
            _dbContext.Trips.Add(trip);
            await _dbContext.SaveChangesAsync();
        }

        private async Task UpdateActiveTrip(Vehicle vehicle)
        {
            var activeTrip = await _dbContext.Trips.FirstOrDefaultAsync(t => t.VehicleId == vehicle.Id && t.StartTime != null && t.Distance == 0);
            if (activeTrip != null)
            {
                activeTrip.EndTime = DateTime.Now;
                var timeElapsed = activeTrip.EndTime - activeTrip.StartTime;
                activeTrip.Distance = (decimal)CalculateDistanceTraveled(timeElapsed, vehicle.AverageSpeed, activeTrip.TripType);
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                Console.WriteLine("No active trip found");
            }
            vehicle.DistanceDriven = _dbContext.Trips.Where(t => t.VehicleId == vehicle.Id && t.TripType == eStatus.Driving.ToString()).Sum(t => t.Distance);
            vehicle.DistanceReversed = _dbContext.Trips.Where(t => t.VehicleId == vehicle.Id && t.TripType == eStatus.Reversing.ToString()).Sum(t => t.Distance);
            await _dbContext.SaveChangesAsync();
        }

        private decimal CalculateDistanceTraveled(TimeSpan? timeTraveled, decimal averageSpeed, string tripType)
        {
            if (timeTraveled.HasValue)
            {
               
                decimal totalHours = (decimal)timeTraveled.Value.TotalMinutes / 60;

                if (tripType.ToLower() == "reversing")
                {
                    return 0.1m * averageSpeed * totalHours;
                }
                else
                {                    
                    return averageSpeed * totalHours;
                }
            }
            else
            {
                return 0; 
            }
        }
    }
}

