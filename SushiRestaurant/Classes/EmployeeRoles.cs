namespace SushiRestaurant;

public class Waiter : Employee
{
    public decimal Tips { get; set; }
    public List<string> SpokenLanguages { get; set; } = new List<string>();
    
    public override decimal Salary => BaseSalary + Tips;
}

public enum SeniorityLevel
{
    Junior,
    Senior
}


public class Manager : Employee
{
    public SeniorityLevel SeniorityLevel { get; set; }

    public override decimal Salary => BaseSalary * (SeniorityLevel == SeniorityLevel.Senior ? 1.5m : 1.2m);
}

public class Cook : Employee
{
    public decimal Bonus { get; set; }
    public string? Specialization { get; set; }

    public override decimal Salary => BaseSalary + Bonus;
}

public class Cleaner : Employee
{
    public string CleaningShift { get; set; }
    public string AssignedArea { get; set; }

    public override decimal Salary => BaseSalary;
}