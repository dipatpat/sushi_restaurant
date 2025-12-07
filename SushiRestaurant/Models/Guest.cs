using System.Text.Json.Serialization;
using SushiRestaurant.Models;

namespace SushiRestaurant;

public class Guest : Person
{
    private static readonly List<Guest> _extent = new();
    public static IReadOnlyList<Guest> Extent => _extent.AsReadOnly();

    public static void ClearExtent() => _extent.Clear();

    internal static void SetExtent(List<Guest>? items)
    {
        _extent.Clear();
        if (items is { Count: > 0 })
            _extent.AddRange(items);
    }

    private string? _nickname;
    public string? Nickname
    {
        get => _nickname;
        set
        {
            if (value is not null && string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Nickname cannot be empty when provided.", nameof(Nickname));
            _nickname = value?.Trim();
        }
    }
    
    public LoyaltyCard? LoyaltyCard { get; set; }
    private readonly HashSet<Reservation> _reservations = new();
    
    [JsonIgnore] 
    public IReadOnlyCollection<Reservation> Reservations 
        => _reservations.ToList().AsReadOnly();

    public void AddReservation(Reservation reservation)
    {
        if  (reservation is null)
            throw new ArgumentNullException(nameof(reservation));

        if (!ReferenceEquals(reservation.Guest, null) &&
            !ReferenceEquals(reservation.Guest, this))
        {
            this.TakeOverReservation(reservation);
            return;
        }

        //add locally
        bool added = _reservations.Add(reservation);
        
        //reverse connection
        if (!ReferenceEquals(reservation.Guest, this))
        {
            reservation.InternalSetGuestFromGuest(this);
        }
    }

    public void TakeOverReservation(Reservation reservation)
    {
        if (reservation is null)
            throw new ArgumentNullException(nameof(reservation));
        
        reservation.ChangeGuest(this);
    }
    internal void InternalAddReservation(Reservation reservation)
    
    {
        if (reservation is null) 
            throw new ArgumentNullException(nameof(reservation));
        
        _reservations.Add(reservation);
        
    }
    
    internal void InternalRemoveReservation(Reservation reservation)
    {
        if (reservation is null) throw new ArgumentNullException(nameof(reservation));
        _reservations.Remove(reservation);
    }
    
    public SushiRestaurant.Models.LoyaltyCard CreateLoyaltyCard(
        string email, 
        SushiRestaurant.Models.LoyaltyType type, 
        int points)
    {
        if (LoyaltyCard != null)
            throw new InvalidOperationException("Guest already has a loyalty card.");

        return new SushiRestaurant.Models.LoyaltyCard(this, email, type, points);
    }

    public void RemoveLoyaltyCard()
    {
        LoyaltyCard = null;
    }

    public Guest(string firstName, string lastName, string? nickname = null)
        : base(firstName, lastName)
    {
        Nickname = nickname;
        _extent.Add(this);
    }


    public Guest() { }

    public static Guest? FindByName(string firstName, string lastName)
    {
        var f = firstName?.Trim() ?? string.Empty;
        var l = lastName?.Trim() ?? string.Empty;

        return _extent.FirstOrDefault(g =>
            string.Equals(g.FirstName, f, StringComparison.OrdinalIgnoreCase) &&
            string.Equals(g.LastName,  l, StringComparison.OrdinalIgnoreCase));
    }

    public override string ToString()
        => Nickname is { Length: > 0 }
            ? $"{FirstName} \"{Nickname}\" {LastName}"
            : $"{FirstName} {LastName}";
}