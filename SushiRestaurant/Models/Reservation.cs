using System.Text.Json.Serialization;
using System.Linq;

namespace SushiRestaurant;

public class Reservation
{
    private static readonly List<Reservation> _extent = new();
    public static IReadOnlyList<Reservation> Extent => _extent.AsReadOnly();

    public static void ClearExtent() => _extent.Clear();

    internal static void SetExtent(List<Reservation>? items)
    {
        _extent.Clear();
        if (items is { Count: > 0 })
            _extent.AddRange(items);
    }

    public static int DurationHours = 3;

    private Guest _guest = default!;
    public Guest Guest => _guest;
    // USED BY CONSTRUCTOR (Reservation side → calls Guest.InternalAddReservation)
    private void SetGuest(Guest guest)
    {
        if (guest is null)
            throw new ArgumentNullException(nameof(guest));
        
        _guest = guest;
        guest.InternalAddReservation(this);   
    }

    // USED BY GUEST.AddReservation (Guest side → NO reverse call)
    internal void InternalSetGuestFromGuest(Guest guest)
    {
        if (guest is null)
            throw new ArgumentNullException(nameof(guest));

        if (ReferenceEquals(guest, _guest))
            return;
        _guest = guest;
        //no guest.InternalAddReservation(this) here,
        // because Guest.AddReservation already updated its collection.
    }
    
    public void ChangeGuest(Guest newGuest)
    {
        if (newGuest is null)
            throw new ArgumentNullException(nameof(newGuest));

        if (ReferenceEquals(newGuest, _guest))
            return;

        var oldGuest = _guest;

        if (oldGuest is not null)
        {
            oldGuest.InternalRemoveReservation(this);
        }
        _guest = newGuest;

        newGuest.InternalAddReservation(this);
    }

    public void RemoveGuest()
    {
        var oldGuest = _guest;
        if (oldGuest is null)
            return;
        
        oldGuest.InternalRemoveReservation(this);
        _guest = null;
    }

    internal void InternalRemoveGuestFromGuest(Guest guest)
    {
        if (!ReferenceEquals(guest, _guest))
            return;
        _guest = null;
    }
    
    private Table _table = default!;
    public Table Table => _table;

    private void SetTable(Table table)
    {
        if (table is null)
            throw new ArgumentNullException(nameof(table));
        
        if (_table != null && _table != table)
            throw new InvalidOperationException("This reservation is already assigned to another table.");

        _table = table;
        
        table.AddReservation(this);
    }

    public void ChangeTable(Table newTable)
    {
        if (newTable is null)
            throw new ArgumentNullException(nameof(newTable));

        if (ReferenceEquals(newTable, _table))
            return; 

        var oldTable = _table;
        _table = newTable;
        
        oldTable.RemoveReservation(this);
        newTable.AddReservation(this);
    }


    private DateTime _startDateTime;
    public DateTime StartDateTime
    {
        get => _startDateTime;
        set
        {
            if (value == default)
                throw new ArgumentException("StartDateTime is required.", nameof(StartDateTime));

            if (value < DateTime.Now)
                throw new ArgumentOutOfRangeException(nameof(StartDateTime),
                    "Start date and time must be in the future.");

            _startDateTime = value;
        }
    }

    [JsonIgnore]
    public DateTime EndDateTime => StartDateTime.AddHours(DurationHours);

    private int? _reviewScore;
    public int? ReviewScore
    {
        get => _reviewScore;
        set
        {
            if (value is < 0 or > 5)
                throw new ArgumentOutOfRangeException(nameof(ReviewScore),
                    "Review score must be between 0 and 5.");
            _reviewScore = value;
        }
    }

    private string? _comment;
    public string? Comment
    {
        get => _comment;
        set
        {
            if (value is null)
            {
                _comment = null;
            }
            else
            {
                var trimmed = value.Trim();
                if (trimmed.Length > 500)
                    throw new ArgumentException("Comment too long (max 500 chars).", nameof(Comment));
                _comment = trimmed;
            }
        }
    }

    private int _numberOfGuests;
    public int NumberOfGuests
    {
        get => _numberOfGuests;
        set
        {
            if (value < 1 || value > 10)
                throw new ArgumentOutOfRangeException(nameof(NumberOfGuests),
                    "Number of guests must be between 1 and 10.");
            _numberOfGuests = value;
        }
    }

    public bool IsPaid { get; set; }

    private int _bonusPoints;
    public int BonusPoints
    {
        get => _bonusPoints;
        set
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException(nameof(BonusPoints),
                    "Bonus points cannot be negative.");
            _bonusPoints = value;
        }
    }
    
    private readonly List<Order> _orders = new();
    [JsonIgnore]
    public IReadOnlyCollection<Order> Orders => _orders.AsReadOnly();

    internal void InternalAddOrder(Order order)
    {
        if (order is null) throw new ArgumentNullException(nameof(order));
        if (!_orders.Contains(order))
            _orders.Add(order);
        RegisterCostSnapshot();
    }

    internal void InternalRemoveOrder(Order order)
    {
        if (order is null) throw new ArgumentNullException(nameof(order));
        _orders.Remove(order);
        
        RegisterCostSnapshot();
    }
    
    [JsonIgnore]
    public decimal TotalCost => GetTotalCost();

    public decimal GetTotalCost() => _orders.Sum(o => o.OrderSum);
    
    private readonly List<decimal> _totalCostHistory = new();
    
    [JsonIgnore]
    public IReadOnlyCollection<decimal> TotalCostHistory => _totalCostHistory.AsReadOnly();

    internal void RegisterCostSnapshot()
    {
        _totalCostHistory.Add(GetTotalCost());
    }


    public Reservation(DateTime startDateTime, int numberOfGuests, Guest guest, Table table)
    {
        StartDateTime = startDateTime;
        NumberOfGuests = numberOfGuests;
        SetGuest(guest); //uses reverse connection to Guest
        SetTable(table);

        _extent.Add(this);
        RegisterCostSnapshot();
    }
    
    public Reservation() { }
}
