namespace SushiRestaurant;
using System.Text.Json.Serialization;

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

    private readonly HashSet<Ingredient> _ingredients = new HashSet<Ingredient>();

    public IReadOnlySet<Ingredient> GetIngredients()
    {
        return new HashSet<Ingredient>(_ingredients);
    }

    public void AddIngredient(Ingredient ingredient)
    {
        if (ingredient == null)
            throw new ArgumentNullException(nameof(ingredient));

        bool added = _ingredients.Add(ingredient);
        if (!added) return;

        if (!ingredient.GetPartByDishes().Contains(this))
        {
            ingredient.InternalAddDish(this);
            Console.WriteLine($"Ingredient '{ingredient.Name}' added to Dish '{DishName}'.");
        }
    }

    internal void InternalAddIngredient(Ingredient ingredient)
    {
        if (ingredient == null) throw new ArgumentNullException(nameof(ingredient));
        _ingredients.Add(ingredient);
    }

    public void RemoveIngredient(Ingredient ingredient)
    {
        if (ingredient == null)
            throw new ArgumentNullException(nameof(ingredient));

        bool removed = _ingredients.Remove(ingredient);
        if (!removed)
        {
            Console.WriteLine($"Warning: Ingredient is null or not found in Dish '{DishName}'.");
            return;
        }
        ingredient.InternalRemoveDish(this);
        
        Console.WriteLine($"Ingredient '{ingredient.Name}' removed from Dish '{DishName}'.");

        if (!_ingredients.Any())
        {
            Console.WriteLine(
                $"Warning: Dish '{DishName}' now contains 0 ingredients, violating the 1..* minimum constraint.");
        }
    }

    internal void InternalRemoveIngredient(Ingredient ingredient)
    {
        if (ingredient == null) throw new ArgumentNullException(nameof(ingredient));
        _ingredients.Remove(ingredient);
    }

    private readonly HashSet<DishInOrder> _dishInOrdersItems = new();
    
    [JsonIgnore]
    public IReadOnlyCollection<DishInOrder> ActiveDishInOrderItems =>
        _dishInOrdersItems.Where(i => i.IsActive).ToList().AsReadOnly();

    [JsonIgnore] public IReadOnlyCollection<DishInOrder> AllDishInOrders => _dishInOrdersItems.ToList().AsReadOnly();
    
    public Dish(string name, decimal price, DishType type)
    {
        DishName = name;
        Price = price;
        DishType = type;

        _extent.Add(this);
    }

    public Dish()
    {
    }

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

    [JsonIgnore] public IReadOnlyCollection<DishInOrder> DishInOrdersItems => _dishInOrdersItems.ToList().AsReadOnly();

    internal void InternalAddDishInOrder(DishInOrder item)
    {
        if (item is null)
        {
            throw new ArgumentNullException(nameof(item));
        }

        _dishInOrdersItems.Add(item);
    }

    internal void InternalRemoveDishInOrder(DishInOrder item)
    {
        if (item is null)
        {
            throw new ArgumentNullException(nameof(item));
        }

        _dishInOrdersItems.Remove(item);
    }

    public DishInOrder AddToOrder(Order order, int quantity)
    {
        if (order is null)
        {
            throw new ArgumentNullException(nameof(order));
        }

        return new DishInOrder(this, order, quantity);
    }

    public void RemoveCompletelyFromOrder(DishInOrder item)
    {
        if (item is null)
            throw new ArgumentNullException(nameof(item));
        if (!ReferenceEquals(item.Dish, this))
        {
            throw new InvalidOperationException("This DishInOrder does not belong to this dish.");
        }

        item.RemoveCompletely();
    }
    
    public void DeactivateFromOrder(DishInOrder item)
    {
        if (item is null)
            throw new ArgumentNullException(nameof(item));
        if (!ReferenceEquals(item.Dish, this))
        {
            throw new InvalidOperationException("This DishInOrder does not belong to this dish.");
        }

        item.Deactivate();
    }
}