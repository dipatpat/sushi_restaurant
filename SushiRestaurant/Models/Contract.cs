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
        Employee = employee;
        
        employee.AddContract(this);
    }
    
    public void SetEmployee(Employee newEmployee)
    {
        if (newEmployee == null)
            throw new ArgumentNullException(nameof(newEmployee));

        if (_employee == newEmployee)
            return;

        var oldEmployee = _employee;
        _employee = newEmployee;
        
        oldEmployee.RemoveContract(this);
        newEmployee.AddContract(this);
    }
}