namespace SushiRestaurant;

public class Table
{
    private static readonly List<Table> _extent = new();
    public static IReadOnlyList<Table> Extent => _extent.AsReadOnly();

    public static void ClearExtent() => _extent.Clear();

    public int TableNumber { get; private set; }
    public int Capacity { get; private set; }

    private readonly List<Reservation> _reservations = new();
    public IReadOnlyList<Reservation> Reservations => _reservations.AsReadOnly();

    public Table(int tableNumber, int capacity)
    {
        if (tableNumber <= 0)
            throw new ArgumentOutOfRangeException(nameof(tableNumber));

        if (capacity < 1)
            throw new ArgumentOutOfRangeException(nameof(capacity));

        TableNumber = tableNumber;
        Capacity = capacity;

        _extent.Add(this);
    }

    internal void AddReservation(Reservation r)
    {
        if (r == null)
            throw new ArgumentNullException(nameof(r));

        if (_reservations.Contains(r))
            throw new InvalidOperationException("Reservation already assigned to this table.");

        _reservations.Add(r);
    }

    internal void RemoveReservation(Reservation r)
    {
        _reservations.Remove(r);
    }
}