namespace SushiRestaurant;

public class DishInOrder
{
    private static readonly List<DishInOrder> _extent = new();
    public static IReadOnlyList<DishInOrder> Extent => _extent.AsReadOnly();
    public static void ClearExtent() => _extent.Clear();

    private Dish _dish = default!;
    public Dish Dish => _dish;

    private Order _order = default!;
    public Order Order => _order;

    public int Quantity { get; private set; }
    public DateTime TimeOrdered { get; }

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

        order.InternalAddDishInOrder(this);
        dish.InternalAddDishInOrder(this);

        _extent.Add(this);
    }

    public void ChangeQuantity(int newQuantity)
    {
        if (newQuantity <= 0)
            throw new ArgumentOutOfRangeException(nameof(newQuantity),
                "Quantity must be positive.");

        Quantity = newQuantity;

        Order.NotifyItemsChanged();
    }

    public void Remove()
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
    }
}