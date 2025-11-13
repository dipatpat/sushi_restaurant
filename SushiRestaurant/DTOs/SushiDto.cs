namespace SushiRestaurant;

public class SushiDto
{
    public List<Guest> Guests { get; set; } = new();
    public List<Reservation> Reservations { get; set; } = new();

    public List<FullTimeWaiter> FullTimeWaiters { get; set; } = new();
    public List<PartTimeWaiter> PartTimeWaiters { get; set; } = new();

    public List<FullTimeManager> FullTimeManagers { get; set; } = new();
    public List<PartTimeManager> PartTimeManagers { get; set; } = new();

    public List<FullTimeCook> FullTimeCooks { get; set; } = new();
    public List<PartTimeCook> PartTimeCooks { get; set; } = new();

    public List<FullTimeCleaner> FullTimeCleaners { get; set; } = new();
    public List<PartTimeCleaner> PartTimeCleaners { get; set; } = new();
}