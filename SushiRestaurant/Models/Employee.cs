namespace SushiRestaurant;

public abstract class Employee : Person
{
    public Address Address { get; set; } = new Address();

    public string BankAccount { get; set; }
    public string PhoneNumber { get; set; }
    public decimal BaseSalary { get; set; }

    public abstract decimal Salary { get; }

    public bool IsFullTime { get; set; }
}