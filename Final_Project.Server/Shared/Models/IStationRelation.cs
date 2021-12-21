namespace Final_Project.Server.Shared.Models
{
    public interface IStationRelation
    {
        public int StationToId { get; set; }
        public FlightDirection Direction { get; set; }
    }
}
