namespace SushiRestaurant;

public class Contract
{
    private Employee _employee;

    public Employee Employee
    {
        get => _employee;
        private set => _employee = value ?? throw new ArgumentNullException(nameof(Employee));
    }

    private DateTime _startDate;
    public DateTime StartDate
    {
        get => _startDate;
        set
        {
            if (value == default)
                throw new ArgumentException("Start date is required.");
            _startDate = value;
        }
    }

    private DateTime? _endDate;
    public DateTime? EndDate
    {
        get => _endDate;
        set
        {
            if (value != null && value <= StartDate)
                throw new ArgumentException("End date must be after start date.");
            _endDate = value;
        }
    }
    
    public Contract(Employee employee, DateTime startDate)
    {
        if (employee == null)
            throw new ArgumentNullException(nameof(employee));

        StartDate = startDate;
        _employee = employee;
        
        employee.AddContract(this);
    }
    
    public void SetEmployee(Employee employee)
    {
        if (employee == null)
            throw new ArgumentNullException(nameof(employee));

        if (_employee == employee)
            return;

        var oldEmployee = _employee;
        _employee = employee;
        
        oldEmployee.RemoveContract(this);
        employee.AddContract(this);
    }
}