using System.Text.Json.Serialization;
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