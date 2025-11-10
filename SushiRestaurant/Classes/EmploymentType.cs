namespace SushiRestaurant;

public interface IFullTimeAspect
{
    int VacationDays { get; set; }
    bool IsOnSickLeave { get; set; }
}

public interface IPartTimeAspect
{
    double HoursInContract { get; set; }
}