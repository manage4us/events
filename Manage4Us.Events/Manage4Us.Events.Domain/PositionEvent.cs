namespace Manage4Us.Events.Domain
{
    public class PositionEvent
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int EmployeeId { get; set; }
        public int LoginId { get; set; }
    }
}