using System.Text.Json.Serialization;

namespace SushiRestaurant;

public enum OrderStatus
{
    Pending,
    Accepted,
    Preparing,
    Cooked,
    Served,
    Completed,
    Canceled
}

public class Order
{
    private static readonly List<Order> _extent = new();
    public static IReadOnlyList<Order> Extent => _extent.AsReadOnly();
    public static void ClearExtent() => _extent.Clear();


    private Reservation _reservation = default!;
    public Reservation Reservation => _reservation;

    private void SetReservation(Reservation reservation)
    {
        _reservation = reservation ?? throw new ArgumentNullException(nameof(reservation));
        reservation.InternalAddOrder(this);   
    }
    
    public void ChangeReservation(Reservation newReservation)
    {
        if (newReservation == null)
            throw new ArgumentNullException(nameof(newReservation));

        if (DateTime.Now >= _reservation.StartDateTime)
            throw new InvalidOperationException("Cannot change reservation once the original reservation has started.");

        if (ReferenceEquals(newReservation, _reservation))
            return;

        var oldReservation = _reservation;
        _reservation = newReservation;

        oldReservation.InternalRemoveOrder(this);
        newReservation.InternalAddOrder(this);
    }
    
    public void Remove()
    {
        _reservation.InternalRemoveOrder(this);
        _extent.Remove(this);
    }

    private readonly HashSet<DishInOrder> _dishInOrderItems = new();
    
    [JsonIgnore]
    public IReadOnlyCollection<DishInOrder> ActiveDishInOrderItems => _dishInOrderItems.Where(i => i.IsActive).ToList().AsReadOnly();
    
    [JsonIgnore]
    public IReadOnlyCollection<DishInOrder> AllDishInOrderItems => _dishInOrderItems.ToList().AsReadOnly();
    internal void InternalAddDishInOrder(DishInOrder item)
    {
        if (item is null)
        {
            throw new ArgumentNullException(nameof(item));
        }

        _dishInOrderItems.Add(item);
        NotifyItemsChanged();
    }

    internal void InternalRemoveDishInOrder(DishInOrder item)
    {
        if (item is null)
        {
            throw new ArgumentNullException(nameof(item));
        }
        _dishInOrderItems.Remove(item);
        NotifyItemsChanged();
    }

    internal void NotifyItemsChanged()
    {
        _reservation.RegisterCostSnapshot();
    }

    public void RemoveItemFromOrder(DishInOrder item)
    {
        if (item is null)
        {
            throw new ArgumentNullException(nameof(item));
        }
        if (!ReferenceEquals(item.Order, this))
            throw new InvalidOperationException("This DishInOrder doesn't belong to this order.");
        item.Deactivate();
    }

    public DishInOrder AddItemToOrder(Dish dish, int quantity)
    {
        if (dish is null)
        {
            throw new ArgumentNullException(nameof(dish));
        }

        if (Status is OrderStatus.Canceled or OrderStatus.Completed)
        {
            throw new InvalidOperationException("Cannot add an item to canceled or completed orders.");
        }

        return new DishInOrder(dish, this, quantity);
    }

   

    [JsonIgnore]
    public decimal OrderSum => _dishInOrderItems
        .Where(i => i.IsActive)
        .Sum(i => i.Dish.Price * i.Quantity);


    public DateTime CreatedAt { get; private set; }
    public OrderStatus Status { get; private set; }


    public Order(Reservation reservation)
    {
        SetReservation(reservation);
        CreatedAt = DateTime.Now;
        Status = OrderStatus.Pending;

        _extent.Add(this);
    }
    
    public void PlaceOrder(bool successful)
    {
        if (DateTime.Now < Reservation.StartDateTime)
            throw new InvalidOperationException("Cannot place order before the reservation has started.");

        if (Status != OrderStatus.Pending)
            throw new InvalidOperationException("placeOrder can only be called from Pending.");

        Status = successful ? OrderStatus.Accepted : OrderStatus.Canceled;
    }

    public void CancelOrder()
    {
        if (Status is OrderStatus.Pending or OrderStatus.Accepted)
        {
            Status = OrderStatus.Canceled;
            return;
        }

        throw new InvalidOperationException("Cannot cancel at this stage.");
    }


    public void ChangeStatus(OrderStatus newStatus)
    {
        switch (Status)
        {
            case OrderStatus.Accepted when newStatus == OrderStatus.Preparing:
                Status = newStatus;
                break;

            case OrderStatus.Preparing when newStatus == OrderStatus.Cooked:
                Status = newStatus;
                break;

            case OrderStatus.Cooked when newStatus == OrderStatus.Served:
                Status = newStatus;
                break;

            case OrderStatus.Served when newStatus == OrderStatus.Completed:
                Status = newStatus;
                break;

            default:
                throw new InvalidOperationException(
                    $"Invalid transition from {Status} to {newStatus}");
        }
    }
}
