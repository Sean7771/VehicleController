using Microsoft.AspNetCore.SignalR;

namespace VehicleController.SignalR.Hubs
{
    public class MakesHub : Hub
    {
        private readonly ILogger<MakesHub> _logger;

        public MakesHub(ILogger<MakesHub> logger)
        {
            _logger = logger;
        }

        public async Task MakeAdded(string makeName, int makeId)
        {
            try
            {
                await Clients.All.SendAsync("SendMakeAddedNotification", makeName, makeId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }

        public async Task MakeEdited(string makeName, int makeId)
        {
            try
            {
                await Clients.All.SendAsync("SendMakeEditedNotification", makeName, makeId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }

        public async Task MakeDeleted(int makeId)
        {
            try
            {
                await Clients.All.SendAsync("SendMakeDeletedNotification", makeId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }
    }
}
