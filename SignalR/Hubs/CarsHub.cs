using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using VehicleController.Models; // Make sure to import the namespace containing the Car class

namespace VehicleController.SignalR.Hubs
{
    public class CarsHub : Hub
    {
        // Method to broadcast that a new car has been created
        public async Task CreateCar(Car car)
        {
            // Broadcast the car object to all clients
            await Clients.All.SendAsync("CarCreated", car);
        }

        // Method to broadcast that a car has been updated
        public async Task UpdateCar(Car car)
        {
            // Broadcast the car object to all clients
            await Clients.All.SendAsync("CarUpdated", car);
        }
    }
}