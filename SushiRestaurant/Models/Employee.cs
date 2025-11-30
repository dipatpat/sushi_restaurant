using System.Text.Json.Serialization;

namespace SushiRestaurant;

public abstract class Employee : Person
{
    public static decimal MinimumWage { get; set; } = 23.50m;

    private Address _address = new();
    public Address Address
    {
        get => _address;
        set => _address = value ?? throw new ArgumentNullException(nameof(Address));
    }

    private string _bankAccount = default!;
    public string BankAccount
    {
        get => _bankAccount;
        set
        {
            if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("Bank account is required.", nameof(BankAccount));
            _bankAccount = value.Trim();
        }
    }

    private string _phoneNumber = default!;
    public string PhoneNumber
    {
        get => _phoneNumber;
        set
        {
            if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("Phone number is required.", nameof(PhoneNumber));
            _phoneNumber = value.Trim();
        }
    }

    private decimal _baseSalary;
    public decimal BaseSalary
    {
        get => _baseSalary;
        set
        {
            if (value < MinimumWage) throw new ArgumentOutOfRangeException(nameof(BaseSalary), $"Base salary must be ≥ {MinimumWage}.");
            _baseSalary = value;
        }
    }

    [JsonIgnore] 
    public abstract decimal Salary { get; }

    public bool IsFullTime { get; protected set; }

    protected Employee(string firstName, string lastName, Address address,
        string bankAccount, string phoneNumber, decimal baseSalary, bool isFullTime)
        : base(firstName, lastName)
    {
        Address = address ?? throw new ArgumentNullException(nameof(address));
        BankAccount = bankAccount;
        PhoneNumber = phoneNumber;
        BaseSalary = baseSalary;
        IsFullTime = isFullTime;
    }

    protected Employee() { } 
}