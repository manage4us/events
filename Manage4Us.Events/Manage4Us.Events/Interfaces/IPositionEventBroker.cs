namespace Manage4Us.Events.Interfaces
{
    public interface IPositionEventBroker
    {
        Task PushPositionAsync(double latitude, double longitude, int employeeId, int loginId);
    }
}
