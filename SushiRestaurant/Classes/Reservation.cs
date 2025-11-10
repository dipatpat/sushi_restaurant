namespace SushiRestaurant;

public class Reservation
{
    public const int DurationHours = 3;

    public DateTime StartDateTime { get; set; }
    public DateTime EndDateTime => StartDateTime.AddHours(DurationHours);
    public decimal TotalCost { get; set; }
    private int? _reviewScore;
    public int? ReviewScore
    {
        get => _reviewScore;
        set
        {
            if (value.HasValue && (value < 0 || value > 5))
            {
                throw new ArgumentOutOfRangeException(nameof(ReviewScore), "Review score must be between 0 and 5.");
            }
            _reviewScore = value;
        }
    }

    public string? Comment { get; set; }
    private int _numberOfGuests;
    public int NumberOfGuests
    {
        get => _numberOfGuests;
        set
        {
            if (value < 1 || value > 10)
            {
                throw new ArgumentOutOfRangeException(nameof(NumberOfGuests), "Number of guests must be between 1 and 10.");
            }
            _numberOfGuests = value;
        }
    }
    public bool IsPaid { get; set; }
    public int BonusPoints { get; set; } = 0;
}