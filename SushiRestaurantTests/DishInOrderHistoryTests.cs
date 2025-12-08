using NUnit.Framework;
using SushiRestaurant;

namespace sushi_restaurant_tests;

[TestFixture]
public class DishInOrderHistoryTests
{
    [SetUp]
    public void Setup()
    {
        Guest.ClearExtent();
        Reservation.ClearExtent();
        Order.ClearExtent();
        Dish.ClearExtent();
        DishInOrder.ClearExtent();
    }

    private static Order CreateSampleOrder()
    {
        var guest = new Guest("Anna", "Test");
        var table = new Table(1, 2);
        var res = new Reservation(DateTime.Now.AddHours(1), 2, guest, table);
        return new Order(res);
    }

    private static Dish CreateSampleDish(string name = "Sushi", decimal price = 10)
        => new Dish(name, price, DishType.Sushi);



    [Test]
    public void Deactivate_Should_Mark_Association_Inactive_And_Keep_In_Extent()
    {
        var order = CreateSampleOrder();
        var dish = CreateSampleDish();
        var item = order.AddItemToOrder(dish, 2);

        item.Deactivate();

        Assert.Multiple(() =>
        {
            Assert.That(item.IsActive, Is.False);
            Assert.That(item.TimeRemoved, Is.Not.Null);
            Assert.That(DishInOrder.Extent, Contains.Item(item), "Extent should still contain inactive items");
            Assert.That(order.AllDishInOrderItems, Contains.Item(item));
            Assert.That(dish.AllDishInOrders, Contains.Item(item));
        });
    }

    // -------------------------------------------------------
    // 2) Deactivate removes business effect (no more cost)
    // -------------------------------------------------------

    [Test]
    public void Deactivate_Should_Remove_Item_From_OrderSum()
    {
        var order = CreateSampleOrder();
        var dish1 = CreateSampleDish("A", 10);
        var dish2 = CreateSampleDish("B", 5);

        var i1 = order.AddItemToOrder(dish1, 1);
        var i2 = order.AddItemToOrder(dish2, 1);

        // Total = 15
        Assert.That(order.OrderSum, Is.EqualTo(15m));

        i1.Deactivate();

        // Now only dish2 counts → 5
        Assert.That(order.OrderSum, Is.EqualTo(5m));
    }

    // -------------------------------------------------------
    // 3) RemoveCompletely deletes all links but preserves TimeRemoved if already set
    // -------------------------------------------------------

    [Test]
    public void RemoveCompletely_Should_Delete_All_References()
    {
        var order = CreateSampleOrder();
        var dish = CreateSampleDish();
        var item = order.AddItemToOrder(dish, 1);

        item.RemoveCompletely();

        Assert.Multiple(() =>
        {
            Assert.That(order.AllDishInOrderItems, Has.Count.Zero);
            Assert.That(dish.DishInOrdersItems, Has.Count.Zero);
            Assert.That(DishInOrder.Extent, Has.Count.Zero);
        });
    }

    [Test]
    public void RemoveCompletely_Should_Not_Overwrite_TimeRemoved_Set_By_Deactivate()
    {
        var order = CreateSampleOrder();
        var dish = CreateSampleDish();
        var item = order.AddItemToOrder(dish, 1);

        item.Deactivate();
        var deactivatedAt = item.TimeRemoved;

        item.RemoveCompletely();

        Assert.Multiple(() =>
        {
            Assert.That(item.TimeRemoved, Is.EqualTo(deactivatedAt), "RemoveCompletely must not overwrite existing history timestamp");
        });
    }

    // -------------------------------------------------------
    // 4) Active vs All collections must behave correctly
    // -------------------------------------------------------

    [Test]
    public void Active_Collections_Should_Contain_Only_Active_Items()
    {
        var order = CreateSampleOrder();
        var dish = CreateSampleDish();

        var i1 = order.AddItemToOrder(dish, 1);
        var i2 = order.AddItemToOrder(dish, 2);

        i1.Deactivate();

        Assert.Multiple(() =>
        {
            Assert.That(order.AllDishInOrderItems.Count, Is.EqualTo(2));
            Assert.That(order.ActiveDishInOrderItems.Count, Is.EqualTo(1));
            Assert.That(order.ActiveDishInOrderItems.Single(), Is.SameAs(i2));

            Assert.That(dish.AllDishInOrders.Count, Is.EqualTo(2));
            Assert.That(dish.ActiveDishInOrderItems.Count, Is.EqualTo(1));
        });
    }

    // -------------------------------------------------------
    // 5) Deactivate can be called repeatedly but does nothing after first time
    // -------------------------------------------------------

    [Test]
    public void Deactivate_Called_Twice_Should_Not_Change_TimeRemoved()
    {
        var order = CreateSampleOrder();
        var dish = CreateSampleDish();
        var item = order.AddItemToOrder(dish, 1);

        item.Deactivate();
        var firstRemoval = item.TimeRemoved;

        Thread.Sleep(10);  
        item.Deactivate();

        Assert.That(item.TimeRemoved, Is.EqualTo(firstRemoval), "Deactivate must be idempotent");
    }
}
