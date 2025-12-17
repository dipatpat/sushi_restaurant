using System.Text.Json.Serialization;
using SushiRestaurant.Models;

namespace SushiRestaurant;

// Employee no longer inherits from Person Flattening
public class Guest
{
    // Person field
    private string _firstName = default!;
    private string _lastName  = default!;

    public string FirstName
    {
        get => _firstName;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("First name is required.", nameof(FirstName));
            _firstName = value.Trim();
        }
    }

    public string LastName
    {
        get => _lastName;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Last name is required.", nameof(LastName));
            _lastName = value.Trim();
        }
    }
    
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
    
    public LoyaltyCard? LoyaltyCard { get; private set; }
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

        bool added = _reservations.Add(reservation);
        
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

    public void RemoveReservation(Reservation reservation)
    {
        if (reservation is null)
            throw new ArgumentNullException(nameof(reservation));
        
        bool removed = _reservations.Remove(reservation);
        if (!removed)
        {
            return;
        }

        if (ReferenceEquals(reservation.Guest, this))
        {
            reservation.InternalRemoveGuestFromGuest(this);
        }
    }

    internal void InternalSetLoyaltyCard(LoyaltyCard card)
    {
        if (card is null) throw new ArgumentNullException(nameof(card));
        
        if (LoyaltyCard != null && !ReferenceEquals(LoyaltyCard, card))
            throw new InvalidOperationException("Guest already has a loyalty card.");
            
        LoyaltyCard = card;
    }

    internal void InternalRemoveLoyaltyCard(LoyaltyCard card)
    {
        if (ReferenceEquals(LoyaltyCard, card))
        {
            LoyaltyCard = null;
        }
    }
    
    public LoyaltyCard CreateLoyaltyCard(
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
        if (LoyaltyCard != null)
        {
            LoyaltyCard.RemoveCompletely();
        }
    }

    public Guest(string firstName, string lastName, string? nickname = null)
    {
        LastName  = lastName;
        Nickname  = nickname;
        Nickname = nickname;
        _extent.Add(this);
    }


    public Guest()
    {
        _extent.Add(this);
    }

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