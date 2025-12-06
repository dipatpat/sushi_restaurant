namespace SushiRestaurant;

public class Table
{
    private static readonly List<Table> _extent = new();
    public static IReadOnlyList<Table> Extent => _extent.AsReadOnly();

    public static void ClearExtent() => _extent.Clear();

    public int TableNumber { get; }
    public int Capacity { get; }

    private readonly List<Reservation> _reservations = new();
    public IReadOnlyList<Reservation> Reservations => _reservations.AsReadOnly();

    public Table(int tableNumber, int capacity)
    {
        if (tableNumber <= 0)
            throw new ArgumentOutOfRangeException(nameof(tableNumber));

        if (capacity <= 0)
            throw new ArgumentOutOfRangeException(nameof(capacity));

        TableNumber = tableNumber;
        Capacity = capacity;

        _extent.Add(this);
    }

    internal void AddReservation(Reservation reservation)
    {
        if (reservation == null)
            throw new ArgumentNullException(nameof(reservation));

        if (_reservations.Contains(reservation))
            throw new InvalidOperationException("This reservation is already assigned to this table.");
        
        if (_reservations.Any(r => r.StartDateTime == reservation.StartDateTime))
            throw new InvalidOperationException($"Table {TableNumber} is already reserved at this time.");

        _reservations.Add(reservation);
    }

    internal void RemoveReservation(Reservation reservation)
    {
        if (reservation == null)
            throw new ArgumentNullException(nameof(reservation));

        _reservations.Remove(reservation);
    }
}