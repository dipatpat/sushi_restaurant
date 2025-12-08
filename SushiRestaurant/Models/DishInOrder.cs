namespace SushiRestaurant;

public class DishInOrder
{
    private static readonly List<DishInOrder> _extent = new();
    public static IReadOnlyList<DishInOrder> Extent => _extent.AsReadOnly();
    public static void ClearExtent() => _extent.Clear();

    private Dish? _dish;
    public Dish Dish => _dish ?? throw new InvalidOperationException("Association no longer valid (Dish removed)");

    private Order? _order;
    public Order Order => _order ?? throw new InvalidOperationException("Association no longer valid (Order removed)");

    public int Quantity { get; private set; }
    public DateTime TimeOrdered { get; }
    
    public bool IsActive { get; private set; }
    public DateTime? TimeRemoved { get; private set; }

    public DishInOrder(Dish dish, Order order, int quantity)
    {
        if (dish is null)
            throw new ArgumentNullException(nameof(dish));
        if (order is null)
            throw new ArgumentNullException(nameof(order));
        if (quantity <= 0)
            throw new ArgumentOutOfRangeException(nameof(quantity),
                "Quantity must be positive.");

        _dish = dish;
        _order = order;
        Quantity = quantity;
        TimeOrdered = DateTime.Now;
        IsActive = true;
        TimeRemoved = null;

        order.InternalAddDishInOrder(this);
        dish.InternalAddDishInOrder(this);

        _extent.Add(this);
    }

    public void ChangeQuantity(int newQuantity)
    {
        if (newQuantity <= 0)
            throw new ArgumentOutOfRangeException(nameof(newQuantity),
                "Quantity must be positive.");
        
        if (!IsActive)
            throw new InvalidOperationException("Cannot change quantity while order is not active");
        Quantity = newQuantity;

        Order.NotifyItemsChanged();
    }

    public void Deactivate()
    {
        if (!IsActive)
            return;
        IsActive = false;
        TimeRemoved = DateTime.Now;
        
        Order.NotifyItemsChanged();
    }

    public void RemoveCompletely()
    {
        if (_dish is null && _order is null)
        {
            return;
        }
        _order?.InternalRemoveDishInOrder(this);
        _dish?.InternalRemoveDishInOrder(this);
        
        _extent.Remove(this);
        
        _order = null;
        _dish = null;
        
        IsActive = false;
        TimeRemoved ??= DateTime.Now;
    }
}