namespace SushiRestaurant;

public class Person
{
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

    protected Person(string firstName, string lastName)
    {
        FirstName = firstName; 
        LastName  = lastName;
    }

    protected Person() { }
}

