namespace SushiRestaurant;

public class FullTimeWaiter : Waiter, IFullTimeAspect
{
    private static readonly List<FullTimeWaiter> _extent = new();
    public static IReadOnlyList<FullTimeWaiter> Extent => _extent.AsReadOnly();
    public static void ClearExtent() => _extent.Clear();
    internal static void SetExtent(List<FullTimeWaiter>? items)
    {
        _extent.Clear();
        if (items is { Count: > 0 }) _extent.AddRange(items);
    }

    public int VacationDays { get; set; }
    public bool IsOnSickLeave { get; set; }

    public FullTimeWaiter(string firstName, string lastName, Address address,
        string bankAccount, string phoneNumber, decimal baseSalary,
        int vacationDays, bool isOnSickLeave = false, decimal tips = 0m)
        : base(firstName, lastName, address, bankAccount, phoneNumber, baseSalary, true)
    {
        if (vacationDays < 0) throw new ArgumentOutOfRangeException(nameof(vacationDays));
        if (tips < 0) throw new ArgumentOutOfRangeException(nameof(tips));

        VacationDays = vacationDays;
        IsOnSickLeave = isOnSickLeave;
        Tips = tips;

        _extent.Add(this);
    }

    public FullTimeWaiter() { }
}


public class FullTimeManager : Manager, IFullTimeAspect
{
    private static readonly List<FullTimeManager> _extent = new();
    public static IReadOnlyList<FullTimeManager> Extent => _extent.AsReadOnly();
    public static void ClearExtent() => _extent.Clear();
    internal static void SetExtent(List<FullTimeManager>? items)
    {
        _extent.Clear();
        if (items is { Count: > 0 }) _extent.AddRange(items);
    }

    public int VacationDays { get; set; }
    public bool IsOnSickLeave { get; set; }

    public FullTimeManager(string firstName, string lastName, Address address,
        string bankAccount, string phoneNumber, decimal baseSalary,
        SeniorityLevel seniority, int vacationDays, bool isOnSickLeave = false)
        : base(firstName, lastName, address, bankAccount, phoneNumber, baseSalary, true, seniority)
    {
        if (vacationDays < 0) throw new ArgumentOutOfRangeException(nameof(vacationDays));
        VacationDays = vacationDays;
        IsOnSickLeave = isOnSickLeave;

        _extent.Add(this);
    }

    public FullTimeManager() { }
}
public class FullTimeCook : Cook, IFullTimeAspect
{
    private static readonly List<FullTimeCook> _extent = new();
    public static IReadOnlyList<FullTimeCook> Extent => _extent.AsReadOnly();
    public static void ClearExtent() => _extent.Clear();
    internal static void SetExtent(List<FullTimeCook>? items)
    {
        _extent.Clear();
        if (items is { Count: > 0 }) _extent.AddRange(items);
    }

    public int VacationDays { get; set; }
    public bool IsOnSickLeave { get; set; }

    public FullTimeCook(string firstName, string lastName, Address address,
        string bankAccount, string phoneNumber, decimal baseSalary,
        decimal bonus = 0m, string? specialization = null,
        int vacationDays = 0, bool isOnSickLeave = false)
        : base(firstName, lastName, address, bankAccount, phoneNumber, baseSalary, true, bonus, specialization)
    {
        if (vacationDays < 0) throw new ArgumentOutOfRangeException(nameof(vacationDays));
        VacationDays = vacationDays;
        IsOnSickLeave = isOnSickLeave;

        _extent.Add(this);
    }

    public FullTimeCook() { }
}


public class FullTimeCleaner : Cleaner, IFullTimeAspect
{
    private static readonly List<FullTimeCleaner> _extent = new();
    public static IReadOnlyList<FullTimeCleaner> Extent => _extent.AsReadOnly();
    public static void ClearExtent() => _extent.Clear();
    internal static void SetExtent(List<FullTimeCleaner>? items)
    {
        _extent.Clear();
        if (items is { Count: > 0 }) _extent.AddRange(items);
    }

    public int VacationDays { get; set; }
    public bool IsOnSickLeave { get; set; }

    public FullTimeCleaner(string firstName, string lastName, Address address,
        string bankAccount, string phoneNumber, decimal baseSalary,
        string cleaningShift, string assignedArea,
        int vacationDays = 0, bool isOnSickLeave = false)
        : base(firstName, lastName, address, bankAccount, phoneNumber, baseSalary, true, cleaningShift, assignedArea)
    {
        if (vacationDays < 0) throw new ArgumentOutOfRangeException(nameof(vacationDays));
        VacationDays = vacationDays;
        IsOnSickLeave = isOnSickLeave;

        _extent.Add(this);
    }

    public FullTimeCleaner() { }
}

public class PartTimeWaiter : Waiter, IPartTimeAspect
{
    private static readonly List<PartTimeWaiter> _extent = new();
    public static IReadOnlyList<PartTimeWaiter> Extent => _extent.AsReadOnly();
    public static void ClearExtent() => _extent.Clear();
    internal static void SetExtent(List<PartTimeWaiter>? items)
    {
        _extent.Clear();
        if (items is { Count: > 0 }) _extent.AddRange(items);
    }

    private double _hoursInContract;
    public double HoursInContract
    {
        get => _hoursInContract;
        set
        {
            if (value <= 0) throw new ArgumentOutOfRangeException(nameof(HoursInContract));
            _hoursInContract = value;
        }
    }

    public PartTimeWaiter(string firstName, string lastName, Address address,
        string bankAccount, string phoneNumber, decimal baseSalary,
        double hoursInContract, decimal tips = 0m)
        : base(firstName, lastName, address, bankAccount, phoneNumber, baseSalary, false)
    {
        HoursInContract = hoursInContract;
        if (tips < 0) throw new ArgumentOutOfRangeException(nameof(tips));
        Tips = tips;

        _extent.Add(this);
    }

    public PartTimeWaiter() { }
}

public class PartTimeManager : Manager, IPartTimeAspect
{
    private static readonly List<PartTimeManager> _extent = new();
    public static IReadOnlyList<PartTimeManager> Extent => _extent.AsReadOnly();
    public static void ClearExtent() => _extent.Clear();
    internal static void SetExtent(List<PartTimeManager>? items)
    {
        _extent.Clear();
        if (items is { Count: > 0 }) _extent.AddRange(items);
    }

    private double _hoursInContract;
    public double HoursInContract
    {
        get => _hoursInContract;
        set
        {
            if (value <= 0) throw new ArgumentOutOfRangeException(nameof(HoursInContract));
            _hoursInContract = value;
        }
    }

    public PartTimeManager(string firstName, string lastName, Address address,
        string bankAccount, string phoneNumber, decimal baseSalary,
        SeniorityLevel seniority, double hoursInContract)
        : base(firstName, lastName, address, bankAccount, phoneNumber, baseSalary, false, seniority)
    {
        HoursInContract = hoursInContract;
        _extent.Add(this);
    }

    public PartTimeManager() { }
}


public class PartTimeCook : Cook, IPartTimeAspect
{
    private static readonly List<PartTimeCook> _extent = new();
    public static IReadOnlyList<PartTimeCook> Extent => _extent.AsReadOnly();
    public static void ClearExtent() => _extent.Clear();
    internal static void SetExtent(List<PartTimeCook>? items)
    {
        _extent.Clear();
        if (items is { Count: > 0 }) _extent.AddRange(items);
    }

    private double _hoursInContract;
    public double HoursInContract
    {
        get => _hoursInContract;
        set
        {
            if (value <= 0) throw new ArgumentOutOfRangeException(nameof(HoursInContract));
            _hoursInContract = value;
        }
    }

    public PartTimeCook(string firstName, string lastName, Address address,
        string bankAccount, string phoneNumber, decimal baseSalary,
        double hoursInContract, decimal bonus = 0m, string? specialization = null)
        : base(firstName, lastName, address, bankAccount, phoneNumber, baseSalary, false, bonus, specialization)
    {
        HoursInContract = hoursInContract;
        _extent.Add(this);
    }

    public PartTimeCook() { }
}

public class PartTimeCleaner : Cleaner, IPartTimeAspect
{
    private static readonly List<PartTimeCleaner> _extent = new();
    public static IReadOnlyList<PartTimeCleaner> Extent => _extent.AsReadOnly();
    public static void ClearExtent() => _extent.Clear();
    internal static void SetExtent(List<PartTimeCleaner>? items)
    {
        _extent.Clear();
        if (items is { Count: > 0 }) _extent.AddRange(items);
    }

    private double _hoursInContract;
    public double HoursInContract
    {
        get => _hoursInContract;
        set
        {
            if (value <= 0) throw new ArgumentOutOfRangeException(nameof(HoursInContract));
            _hoursInContract = value;
        }
    }

    public PartTimeCleaner(string firstName, string lastName, Address address,
        string bankAccount, string phoneNumber, decimal baseSalary,
        string cleaningShift, string assignedArea,
        double hoursInContract)
        : base(firstName, lastName, address, bankAccount, phoneNumber, baseSalary, false, cleaningShift, assignedArea)
    {
        HoursInContract = hoursInContract;
        _extent.Add(this);
    }

    public PartTimeCleaner() { }
}