namespace SushiRestaurant;

public class Contract
{
    private Employee? _employee;

    public Employee Employee
    {
        get => _employee!;
        private set => _employee = value;
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

    public Contract(DateTime startDate)
    {
        StartDate = startDate;
    }

    public void SetEmployee(Employee employee)
    {
        if (employee == null)
            throw new ArgumentNullException(nameof(employee));

        if (Employee != null)
            throw new InvalidOperationException("This contract already has an employee.");

        Employee = employee;
        
        employee.AddContract(this);
    }

    public void RemoveEmployee(Employee employee)
    {
        if (Employee != employee)
            throw new InvalidOperationException("This employee is not assigned to this contract.");

        Employee = null;

        employee.RemoveContract(this);
    }
}