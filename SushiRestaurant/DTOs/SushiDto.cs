namespace SushiRestaurant;

public class SushiDto
{
    public List<Guest> Guests { get; set; } = new();
    public List<Reservation> Reservations { get; set; } = new();
    public List<Order> Orders { get; set; } = new();                 // NEW
    public List<DishInOrder> DishInOrders { get; set; } = new();     // NEW

    public List<Employee> Employees { get; set; } = new();

    public List<Dish> Dishes { get; set; } = new();
    public List<Ingredient> Ingredients { get; set; } = new();
    public List<Inventory> Inventory { get; set; } = new();
}