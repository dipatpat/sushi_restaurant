using System.Text.Json.Serialization;
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

    public const int DurationHours = 3;

    private DateTime _startDateTime;
    public DateTime StartDateTime
    {
        get => _startDateTime;
        set
        {
            if (value == default)
                throw new ArgumentException("StartDateTime is required.", nameof(StartDateTime));
            
            _startDateTime = value;
        }
    }

    [JsonIgnore] 
    public DateTime EndDateTime => StartDateTime.AddHours(DurationHours);

    private decimal _totalCost;
    public decimal TotalCost
    {
        get => _totalCost;
        set
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException(nameof(TotalCost), "Total cost cannot be negative.");
            _totalCost = value;
        }
    }

    private int? _reviewScore;
    public int? ReviewScore
    {
        get => _reviewScore;
        set
        {
            if (value is < 0 or > 5)
                throw new ArgumentOutOfRangeException(nameof(ReviewScore), "Review score must be between 0 and 5.");
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
                throw new ArgumentOutOfRangeException(nameof(NumberOfGuests), "Number of guests must be between 1 and 10.");
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
                throw new ArgumentOutOfRangeException(nameof(BonusPoints), "Bonus points cannot be negative.");
            _bonusPoints = value;
        }
    }

    public Reservation(DateTime startDateTime, int numberOfGuests, decimal totalCost)
    {
        StartDateTime = startDateTime; 
        NumberOfGuests = numberOfGuests;
        TotalCost = totalCost;         
        _extent.Add(this);
    }

    public Reservation() { }
}