namespace SushiRestaurant;

public class FullTimeWaiter : Waiter, IFullTimeAspect
{
    public int VacationDays { get; set; }
    public bool IsOnSickLeave { get; set; }
}

public class FullTimeManager : Manager, IFullTimeAspect
{
    public int VacationDays { get; set; }
    public bool IsOnSickLeave { get; set; }
}

public class FullTimeCook : Cook, IFullTimeAspect
{
    public int VacationDays { get; set; }
    public bool IsOnSickLeave { get; set; }
}

public class FullTimeCleaner : Cleaner, IFullTimeAspect
{
    public int VacationDays { get; set; }
    public bool IsOnSickLeave { get; set; }
}

public class PartTimeWaiter : Waiter, IPartTimeAspect
{
    public double HoursInContract { get; set; }
}

public class PartTimeManager : Manager, IPartTimeAspect
{
    public double HoursInContract { get; set; }
}

public class PartTimeCook : Cook, IPartTimeAspect
{
    public double HoursInContract { get; set; }
}

public class PartTimeCleaner : Cleaner, IPartTimeAspect
{
    public double HoursInContract { get; set; }
}