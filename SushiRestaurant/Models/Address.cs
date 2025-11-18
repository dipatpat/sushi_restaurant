namespace SushiRestaurant;

public class Address
{
    private string _streetName = default!;
    private string _streetNumber = default!;
    private string _postalCode = default!;
    private string _cityName = default!;
    private string? _apartmentNumber;
    private string? _doorNumber;

    private const int MaxStreetNameLength = 100;
    private const int MaxCityNameLength = 50;
    private const int MaxStreetNumberLength = 10;
    private const int MinPostalCodeLength = 5;
    private const int MaxPostalCodeLength = 15;
    private const int MaxApartmentDoorLength = 10;

    public string StreetName
    {
        get => _streetName;
        set
        {
            if (string.IsNullOrWhiteSpace(value)) 
                throw new ArgumentException("Street name is required.", nameof(StreetName));
            
            if (value.Length > MaxStreetNameLength)
                throw new ArgumentException($"Street name cannot exceed {MaxStreetNameLength} characters.", nameof(StreetName));

            _streetName = value.Trim();
        }
    }

    public string StreetNumber
    {
        get => _streetNumber;
        set
        {
            if (string.IsNullOrWhiteSpace(value)) 
                throw new ArgumentException("Street number is required.", nameof(StreetNumber));
            
            if (value.Trim().Any(c => !char.IsLetterOrDigit(c) && c != '/' && c != '-'))
                 throw new ArgumentException("Street number contains invalid characters. Only letters, digits, '/', and '-' are allowed.", nameof(StreetNumber));

            if (value.Length > MaxStreetNumberLength)
                throw new ArgumentException($"Street number cannot exceed {MaxStreetNumberLength} characters.", nameof(StreetNumber));

            _streetNumber = value.Trim();
        }
    }

    public string PostalCode
    {
        get => _postalCode;
        set
        {
            if (string.IsNullOrWhiteSpace(value)) 
                throw new ArgumentException("Postal code is required.", nameof(PostalCode));

            string trimmedValue = value.Trim();

            if (trimmedValue.Length < MinPostalCodeLength || trimmedValue.Length > MaxPostalCodeLength)
                throw new ArgumentException($"Postal code must be between {MinPostalCodeLength} and {MaxPostalCodeLength} characters.", nameof(PostalCode));
            
            if (trimmedValue.Any(c => !char.IsDigit(c) && c != ' ' && c != '-'))
                throw new ArgumentException("Postal code contains invalid characters. Only digits, spaces, and hyphens are allowed.", nameof(PostalCode));
            
            _postalCode = trimmedValue;
        }
    }

    public string CityName
    {
        get => _cityName;
        set
        {
            if (string.IsNullOrWhiteSpace(value)) 
                throw new ArgumentException("City name is required.", nameof(CityName));
            
            if (value.Length > MaxCityNameLength)
                throw new ArgumentException($"City name cannot exceed {MaxCityNameLength} characters.", nameof(CityName));

            if (value.Trim().Any(c => !char.IsLetter(c) && !char.IsWhiteSpace(c) && c != '-'))
                throw new ArgumentException("City name contains invalid characters.", nameof(CityName));

            _cityName = value.Trim();
        }
    }

    public string? ApartmentNumber
    {
        get => _apartmentNumber;
        set
        {
            if (value is not null)
            {
                string trimmedValue = value.Trim();
                if (string.IsNullOrEmpty(trimmedValue))
                    throw new ArgumentException("Apartment number cannot be empty if provided.", nameof(ApartmentNumber));
                
                if (trimmedValue.Length > MaxApartmentDoorLength)
                    throw new ArgumentException($"Apartment number cannot exceed {MaxApartmentDoorLength} characters.", nameof(ApartmentNumber));
                
                _apartmentNumber = trimmedValue;
            }
            else
            {
                _apartmentNumber = null;
            }
        }
    }

    public string? DoorNumber
    {
        get => _doorNumber;
        set
        {
            if (value is not null)
            {
                string trimmedValue = value.Trim();
                if (string.IsNullOrEmpty(trimmedValue))
                    throw new ArgumentException("Door number cannot be empty if provided.", nameof(DoorNumber));
                
                if (trimmedValue.Length > MaxApartmentDoorLength)
                    throw new ArgumentException($"Door number cannot exceed {MaxApartmentDoorLength} characters.", nameof(DoorNumber));
                
                _doorNumber = trimmedValue;
            }
            else
            {
                _doorNumber = null;
            }
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