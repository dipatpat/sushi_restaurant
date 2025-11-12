namespace SushiRestaurant;

public class Address
{
    private string _streetName = default!;
    private string _streetNumber = default!;
    private string _postalCode = default!;
    private string _cityName = default!;
    private string? _apartmentNumber;
    private string? _doorNumber;

    public string StreetName
    {
        get => _streetName;
        set
        {
            if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("Street name is required.", nameof(StreetName));
            _streetName = value.Trim();
        }
    }

    public string StreetNumber
    {
        get => _streetNumber;
        set
        {
            if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("Street number is required.", nameof(StreetNumber));
            _streetNumber = value.Trim();
        }
    }

    public string PostalCode
    {
        get => _postalCode;
        set
        {
            if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("Postal code is required.", nameof(PostalCode));
            _postalCode = value.Trim();
        }
    }

    public string CityName
    {
        get => _cityName;
        set
        {
            if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("City name is required.", nameof(CityName));
            _cityName = value.Trim();
        }
    }

    public string? ApartmentNumber
    {
        get => _apartmentNumber;
        set
        {
            if (value is not null && string.IsNullOrWhiteSpace(value)) throw new ArgumentException("Apartment number cannot be empty.");
            _apartmentNumber = value?.Trim();
        }
    }

    public string? DoorNumber
    {
        get => _doorNumber;
        set
        {
            if (value is not null && string.IsNullOrWhiteSpace(value)) throw new ArgumentException("Door number cannot be empty.");
            _doorNumber = value?.Trim();
        }
    }

    public Address() { }

    public Address(string streetName, string streetNumber, string postalCode, string cityName,
                   string? apartmentNumber = null, string? doorNumber = null)
    {
        StreetName = streetName;
        StreetNumber = streetNumber;
        PostalCode = postalCode;
        CityName = cityName;
        ApartmentNumber = apartmentNumber;
        DoorNumber = doorNumber;
    }
}

