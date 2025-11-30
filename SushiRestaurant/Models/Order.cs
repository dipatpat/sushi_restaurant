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


    private readonly List<Dish> _dishes = new();
    public IReadOnlyCollection<Dish> Dishes => _dishes.AsReadOnly();

    public void AddItemToOrder(Dish dish)
    {
        if (dish == null)
            throw new ArgumentNullException(nameof(dish));
        if (Status == OrderStatus.Canceled || Status == OrderStatus.Completed)
            throw new InvalidOperationException("Cannot add items to canceled/completed orders.");

        _dishes.Add(dish);
    }


    [JsonIgnore]
    public decimal OrderSum => _dishes.Sum(d => d.Price);


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
                Status = newStatus; break;

            case OrderStatus.Preparing when newStatus == OrderStatus.Cooked:
                Status = newStatus; break;

            case OrderStatus.Cooked when newStatus == OrderStatus.Served:
                Status = newStatus; break;

            case OrderStatus.Served when newStatus == OrderStatus.Completed:
                Status = newStatus; break;

            default:
                throw new InvalidOperationException(
                    $"Invalid transition from {Status} to {newStatus}");
        }
    }
}
