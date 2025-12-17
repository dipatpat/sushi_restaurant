using System.Text.Json.Serialization;

namespace SushiRestaurant;

// enums instead of EmploymentType and EmploymnetRole classes, because Flattening
public enum EmployeeRole
{
    Waiter,
    Cleaner,
    Manager,
    Cook
}

public enum EmploymentType
{
    FullTime,
    PartTime
}

// employee no longer abstract
public class Employee : Person
{
    // persistence, now changed so it only stores instances of Employee
    private static readonly List<Employee> _extent = new();
    public static IReadOnlyList<Employee> Extent => _extent.AsReadOnly();
    public static void ClearExtent() => _extent.Clear();
    internal static void SetExtent(List<Employee>? items)
    {
        _extent.Clear();
        if (items is { Count: > 0 }) _extent.AddRange(items);
    }

    // basic fields
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

    // multi-aspect inheritance fields
    public EmployeeRole Role { get; private set; }
    public EmploymentType Type { get; private set; }

    // ASPECT EMPLOYEE ROLE

    // waiter fields
    private List<string>? _spokenLanguages;
    private decimal? _tips;

    public IReadOnlyCollection<string> SpokenLanguages
    {
        get
        {
            EnsureRole(EmployeeRole.Waiter);
            return _spokenLanguages!.AsReadOnly();
        }
    }

    public decimal Tips
    {
        get
        {
            EnsureRole(EmployeeRole.Waiter);
            return _tips!.Value;
        }
        set
        {
            EnsureRole(EmployeeRole.Waiter);
            if (value < 0) throw new ArgumentOutOfRangeException(nameof(Tips), "Tips cannot be negative.");
            _tips = value;
        }
    }

    public void AddLanguage(string lang)
    {
        EnsureRole(EmployeeRole.Waiter);
        if (string.IsNullOrWhiteSpace(lang)) throw new ArgumentException("Language cannot be empty.", nameof(lang));
        var norm = lang.Trim();
        if (!_spokenLanguages!.Contains(norm, StringComparer.OrdinalIgnoreCase))
            _spokenLanguages.Add(norm);
    }

    // cleaner fields
    private string? _cleaningShift;
    private string? _assignedArea;

    public string CleaningShift
    {
        get
        {
            EnsureRole(EmployeeRole.Cleaner);
            return _cleaningShift!;
        }
        set
        {
            EnsureRole(EmployeeRole.Cleaner);
            if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("Cleaning shift is required.");
            _cleaningShift = value.Trim();
        }
    }

    public string AssignedArea
    {
        get
        {
            EnsureRole(EmployeeRole.Cleaner);
            return _assignedArea!;
        }
        set
        {
            EnsureRole(EmployeeRole.Cleaner);
            if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("Assigned area is required.");
            _assignedArea = value.Trim();
        }
    }

    // ASPECT FULL-TIME PART-TIME

    // full-time fields
    private int? _vacationDays;
    private bool? _isOnSickLeave;

    public int VacationDays
    {
        get
        {
            EnsureType(EmploymentType.FullTime);
            return _vacationDays!.Value;
        }
        set
        {
            EnsureType(EmploymentType.FullTime);
            if (value < 0) throw new ArgumentOutOfRangeException(nameof(VacationDays), "Vacation days cannot be negative.");
            _vacationDays = value;
        }
    }

    public bool IsOnSickLeave
    {
        get
        {
            EnsureType(EmploymentType.FullTime);
            return _isOnSickLeave!.Value;
        }
        set
        {
            EnsureType(EmploymentType.FullTime);
            _isOnSickLeave = value;
        }
    }

    // part-time fields
    private double? _hoursInContract;

    public double HoursInContract
    {
        get
        {
            EnsureType(EmploymentType.PartTime);
            return _hoursInContract!.Value;
        }
        set
        {
            EnsureType(EmploymentType.PartTime);
            if (value <= 0) throw new ArgumentOutOfRangeException(nameof(HoursInContract), "Hours in contract must be positive.");
            _hoursInContract = value;
        }
    }

    // dynamic change of employee role
    public void ChangeRoleToWaiter(decimal initialTips = 0m, List<string>? spokenLanguages = null)
    {
        // clear other role fields
        _cleaningShift = null;
        _assignedArea = null;
        
        // initialize Waiter fields
        Role = EmployeeRole.Waiter;
        _spokenLanguages = spokenLanguages ?? new List<string>();
        _tips = initialTips;
    }

    public void ChangeRoleToCleaner(string shift, string area)
    {
        // clear other role fields
        _spokenLanguages = null;
        _tips = null;

        // initialize Cleaner fields
        Role = EmployeeRole.Cleaner;
        CleaningShift = shift;
        AssignedArea = area;
    }

    // dynamic change of employment type
    public void ChangeTypeToFullTime(int vacationDays, bool sickLeave = false)
    {
        // clear PartTime fields
        _hoursInContract = null;

        // init FullTime fields
        Type = EmploymentType.FullTime;
        _vacationDays = vacationDays;
        _isOnSickLeave = sickLeave;
    }

    public void ChangeTypeToPartTime(double hours)
    {
        // clear FullTime fields
        _vacationDays = null;
        _isOnSickLeave = null;

        // init PartTime fields
        Type = EmploymentType.PartTime;
        HoursInContract = hours;
    }

       private Employee? _mentor;
       public Employee Mentor
    {
        get => _mentor;
    }

       private readonly List<Employee> _mentees = new();

       [JsonIgnore]
       public IReadOnlyCollection<Employee> Mentees => _mentees.AsReadOnly();

       public void AssignMentor(Employee mentor)
    {
        if (mentor is null)
            throw new ArgumentNullException(nameof(mentor));

        if (ReferenceEquals(mentor, this))
            throw new InvalidOperationException("An employee cannot mentor themselves.");

        // check Role instead of Type, because Type is always Employee now
        if (mentor.Role != this.Role)
            throw new InvalidOperationException(
                "Mentor must have the same role (e.g. Waiter cannot mentor Cleaner).");

        if (_mentor != null)
        {
            _mentor._mentees.Remove(this);
        }

        _mentor = mentor;

        if (!mentor._mentees.Contains(this))
            mentor._mentees.Add(this);
    }

       public void RemoveMentor()
       {
           if (_mentor == null)
               return;

           _mentor._mentees.Remove(this);
           _mentor = null;
       }

       private readonly List<Contract> _contracts = new();
       public IReadOnlyCollection<Contract> Contracts => _contracts.AsReadOnly();

       public void AddContract(Contract contract)
       {
           if (contract == null)
               throw new ArgumentNullException(nameof(contract));

           if (_contracts.Contains(contract))
               return;

           _contracts.Add(contract);
           
           if (contract.Employee != this)
               contract.SetEmployee(this);
       }

       public void RemoveContract(Contract contract)
       {
           if (!_contracts.Contains(contract))
               throw new InvalidOperationException("This employee does not have this contract.");

           if (_contracts.Count == 1)
               throw new InvalidOperationException("Employee must have at least one contract.");

           _contracts.Remove(contract);
       }

    [JsonIgnore]
    public decimal Salary
    {
        get
        {
            if (Role == EmployeeRole.Waiter) return BaseSalary + (_tips ?? 0m);
            // add logic for manager and cook here
            return BaseSalary;
        }
    }

    public Employee() { _extent.Add(this); }

    public Employee(
        // 1. Base Employee Fields
        string firstName, 
        string lastName, 
        Address address,
        string bankAccount, 
        string phoneNumber, 
        decimal baseSalary,
        
        // 2. Discriminators (Mandatory to determine identity)
        EmployeeRole role,
        EmploymentType type,

        // 3. Waiter Aspect Fields (Optional)
        decimal tips = 0m,
        List<string>? spokenLanguages = null,

        // 4. Cleaner Aspect Fields (Optional)
        string? cleaningShift = null,
        string? assignedArea = null,

        // 5. FullTime Aspect Fields (Optional)
        int vacationDays = 0,
        bool isOnSickLeave = false,

        // 6. PartTime Aspect Fields (Optional)
        double hoursInContract = 0)
    {
        // A. Initialize Base Fields
        FirstName = firstName;
        LastName = lastName;
        Address = address;
        BankAccount = bankAccount;
        PhoneNumber = phoneNumber;
        BaseSalary = baseSalary;

        // Add to Extent immediately upon creation
        _extent.Add(this);

        // B. Initialize Role Aspect (Waiter vs Cleaner)
        switch (role)
        {
            case EmployeeRole.Waiter:
                // Initialize Waiter state
                ChangeRoleToWaiter(tips);
                // Handle the list separately as ChangeRoleToWaiter initializes a fresh list
                if (spokenLanguages != null)
                {
                    foreach (var lang in spokenLanguages) 
                        AddLanguage(lang);
                }
                break;

            case EmployeeRole.Cleaner:
                // Validate required Cleaner args are present before calling
                var shift = cleaningShift ?? "Default Shift"; 
                var area = assignedArea ?? "General Area";
                ChangeRoleToCleaner(shift, area);
                break;
            
            // Cases for Manager/Cook would go here in the future
            case EmployeeRole.Manager:
            case EmployeeRole.Cook:
            default:
                // If role is None or not yet implemented, we define basic state
                Role = role; 
                break;
        }

        // C. Initialize Employment Type Aspect (FullTime vs PartTime)
        switch (type)
        {
            case EmploymentType.FullTime:
                ChangeTypeToFullTime(vacationDays, isOnSickLeave);
                break;

            case EmploymentType.PartTime:
                ChangeTypeToPartTime(hoursInContract);
                break;
            
            default:
                Type = type;
                break;
        }
    }
    
    private void EnsureRole(EmployeeRole required)
    {
        if (Role != required)
            throw new InvalidOperationException($"Operation only valid for {required}, current role is {Role}");
    }

    private void EnsureType(EmploymentType required)
    {
        if (Type != required)
            throw new InvalidOperationException($"Operation only valid for {required}, current type is {Type}");
    }
}