namespace SushiRestaurant;


public enum DishType
{
    Starter,
    Sushi,
    Drink,
    Dessert
}

public class Dish
{
    private static readonly List<Dish> _extent = new();
    public static IReadOnlyList<Dish> Extent => _extent.AsReadOnly();

    public static void ClearExtent() => _extent.Clear();

    internal static void SetExtent(List<Dish>? items)
    {
        _extent.Clear();
        if (items is { Count: > 0 })
            _extent.AddRange(items);
    }

    private string _dishName = default!;
    public string DishName
    {
        get => _dishName;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Dish name is required.");
            
            var trimmed = value.Trim();

            if (trimmed.Length > 20)
                throw new ArgumentException("Dish name cannot exceed 20 characters.", nameof(DishName));

            _dishName = trimmed;
        }
    }

    private decimal _price;
    public decimal Price
    {
        get => _price;
        set
        {
            if (value <= 0)
                throw new ArgumentOutOfRangeException(nameof(Price), "Price must be positive.");
            _price = value;
        }
    }

    public DishType DishType { get; set; }

    public Dish(string name, decimal price, DishType type)
    {
        DishName = name;
        Price = price;
        DishType = type;

        _extent.Add(this);
    }

    public Dish() { }

    public void DisplayDetailedInformation()
    {
        Console.WriteLine($"{DishName} ({DishType}) — {Price:C}");
    }

    public static Dish AddNewDish(string name, decimal price, DishType type)
    {
        return new Dish(name, price, type);
    }

    public static void DisplayMenu()
    {
        foreach (var dish in _extent)
        {
            Console.WriteLine($"{dish.DishName} — {dish.Price:C} ({dish.DishType})");
        }
    }
}
