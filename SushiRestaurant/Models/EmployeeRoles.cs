using System.Text.Json.Serialization;

namespace SushiRestaurant;

public class Waiter : Employee
{
    private readonly List<string> _spokenLanguages = new();
    public IReadOnlyCollection<string> SpokenLanguages => _spokenLanguages.AsReadOnly();

    public void AddLanguage(string lang)
    {
        if (string.IsNullOrWhiteSpace(lang)) throw new ArgumentException("Language cannot be empty.", nameof(lang));
        var norm = lang.Trim();
        if (!_spokenLanguages.Contains(norm, StringComparer.OrdinalIgnoreCase))
            _spokenLanguages.Add(norm);
    }

    public bool RemoveLanguage(string lang) =>
        _spokenLanguages.RemoveAll(l => string.Equals(l, lang, StringComparison.OrdinalIgnoreCase)) > 0;

    private decimal _tips;
    public decimal Tips
    {
        get => _tips;
        set
        {
            if (value < 0) throw new ArgumentOutOfRangeException(nameof(Tips), "Tips cannot be negative.");
            _tips = value;
        }
    }

    [JsonIgnore] 
    public override decimal Salary => BaseSalary + Tips;

    public Waiter(string firstName, string lastName, Address address,
        string bankAccount, string phoneNumber, decimal baseSalary, bool isFullTime)
        : base(firstName, lastName, address, bankAccount, phoneNumber, baseSalary, isFullTime)
    { }

    public Waiter() { } 
}

public enum SeniorityLevel
{
    Junior,
    Senior
}


public class Manager : Employee
{
    public SeniorityLevel SeniorityLevel { get; set; }

    [JsonIgnore] 
    public override decimal Salary => BaseSalary * (SeniorityLevel == SeniorityLevel.Senior ? 1.5m : 1.2m);

    public Manager(string firstName, string lastName, Address address,
        string bankAccount, string phoneNumber, decimal baseSalary, bool isFullTime,
        SeniorityLevel seniority = SeniorityLevel.Junior)
        : base(firstName, lastName, address, bankAccount, phoneNumber, baseSalary, isFullTime)
    {
        SeniorityLevel = seniority;
    }

    public Manager() { } 
}

public class Cook : Employee
{
    private decimal _bonus;
    public decimal Bonus
    {
        get => _bonus;
        set
        {
            if (value < 0) throw new ArgumentOutOfRangeException(nameof(Bonus), "Bonus cannot be negative.");
            _bonus = value;
        }
    }

    private string? _specialization;
    public string? Specialization
    {
        get => _specialization;
        set
        {
            if (value is not null && string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Specialization cannot be empty when provided.", nameof(Specialization));
            _specialization = value?.Trim();
        }
    }

    [JsonIgnore] 
    public override decimal Salary => BaseSalary + Bonus;

    public Cook(string firstName, string lastName, Address address,
        string bankAccount, string phoneNumber, decimal baseSalary, bool isFullTime,
        decimal bonus = 0m, string? specialization = null)
        : base(firstName, lastName, address, bankAccount, phoneNumber, baseSalary, isFullTime)
    {
        Bonus = bonus;
        Specialization = specialization;
    }

    public Cook() { } 
}


public class Cleaner : Employee
{
    private string _cleaningShift = default!;
    public string CleaningShift
    {
        get => _cleaningShift;
        set
        {
            if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("Cleaning shift is required.");
            _cleaningShift = value.Trim();
        }
    }

    private string _assignedArea = default!;
    public string AssignedArea
    {
        get => _assignedArea;
        set
        {
            if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("Assigned area is required.");
            _assignedArea = value.Trim();
        }
    }

    [JsonIgnore] 
    public override decimal Salary => BaseSalary;

    public Cleaner(string firstName, string lastName, Address address,
        string bankAccount, string phoneNumber, decimal baseSalary, bool isFullTime,
        string cleaningShift, string assignedArea)
        : base(firstName, lastName, address, bankAccount, phoneNumber, baseSalary, isFullTime)
    {
        CleaningShift = cleaningShift;
        AssignedArea = assignedArea;
    }

    public Cleaner() { } 
}