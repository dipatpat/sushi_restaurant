using SushiRestaurant;

namespace sushi_restaurant_tests
{
    [TestFixture]
    public class OrderTests
    {
        [SetUp]
        public void SetUp()
        {
            Order.ClearExtent();
            Reservation.ClearExtent();
            Dish.ClearExtent();
        }


        private static Reservation CreateReservation(string label = "A")
        {
            return new Reservation(
                DateTime.Now.AddHours(2),   // must be in the future
                numberOfGuests: 2,
                totalCost: 0m
            )
            {
                Comment = $"Res-{label}"
            };
        }

        private static Dish CreateDish(string name, decimal price, DishType type)
        {
            return new Dish(name, price, type);
        }

        [Test]
        public void Creating_Order_Associates_With_Reservation_And_Updates_Reverse()
        {
            var res = CreateReservation("A");

            var order = new Order(res);

          
            Assert.That(order.Reservation, Is.SameAs(res));

            Assert.That(res.Orders, Has.Count.EqualTo(1));
            Assert.That(res.Orders.First(), Is.SameAs(order));

            Assert.That(Order.Extent, Has.Count.EqualTo(1));
            Assert.That(Order.Extent[0], Is.SameAs(order));
        }

        [Test]
        public void Creating_Order_With_Null_Reservation_Should_Throw()
        {
            Assert.Throws<ArgumentNullException>(() => new Order(null!));
        }

        [Test]
        public void ChangeReservation_Moves_Order_Between_Reservations()
        {
            var res1 = CreateReservation("A");
            var res2 = CreateReservation("B");
            var order = new Order(res1);

            order.ChangeReservation(res2);

            Assert.That(order.Reservation, Is.SameAs(res2));

            Assert.That(res1.Orders, Is.Empty);

            Assert.That(res2.Orders, Has.Count.EqualTo(1));
            Assert.That(res2.Orders.First(), Is.SameAs(order));
        }

        [Test]
        public void ChangeReservation_With_Null_Should_Throw()
        {
            var res = CreateReservation("A");
            var order = new Order(res);

            Assert.Throws<ArgumentNullException>(() => order.ChangeReservation(null!));
        }

        [Test]
        public void ChangeReservation_To_Same_Reservation_Does_Nothing()
        {
            var res = CreateReservation("A");
            var order = new Order(res);

            order.ChangeReservation(res);   

            Assert.That(order.Reservation, Is.SameAs(res));
            Assert.That(res.Orders, Has.Count.EqualTo(1));
            Assert.That(res.Orders.First(), Is.SameAs(order));
        }

        [Test]
        public void Remove_Removes_Order_From_Reservation_And_Extent()
        {
            var res = CreateReservation("A");
            var order = new Order(res);

            order.Remove();

            Assert.That(res.Orders, Is.Empty);

            Assert.That(Order.Extent, Is.Empty);
        }
        
        [Test]
        public void AddItemToOrder_Adds_Dish_And_Updates_OrderSum()
        {
            var res = CreateReservation("A");
            var order = new Order(res);

            var d1 = CreateDish("Miso Soup", 10m, DishType.Starter);
            var d2 = CreateDish("Green Tea", 5m, DishType.Drink);

            order.AddItemToOrder(d1);
            order.AddItemToOrder(d2);

            Assert.That(order.Dishes, Has.Count.EqualTo(2));
            Assert.That(order.Dishes, Does.Contain(d1));
            Assert.That(order.Dishes, Does.Contain(d2));
            Assert.That(order.OrderSum, Is.EqualTo(15m));
        }

        [Test]
        public void AddItemToOrder_Null_Dish_Should_Throw()
        {
            var res = CreateReservation("A");
            var order = new Order(res);

            Assert.Throws<ArgumentNullException>(() => order.AddItemToOrder(null!));
        }

        [Test]
        public void RemoveItemFromOrder_Removes_Dish_And_Updates_OrderSum()
        {
            var res = CreateReservation("A");
            var order = new Order(res);

            var d1 = CreateDish("Miso Soup", 10m, DishType.Starter);
            var d2 = CreateDish("Green Tea", 5m, DishType.Drink);

            order.AddItemToOrder(d1);
            order.AddItemToOrder(d2);

            var removed = order.RemoveItemFromOrder(d1);

            Assert.That(removed, Is.True);
            Assert.That(order.Dishes, Has.Count.EqualTo(1));
            Assert.That(order.Dishes.First(), Is.SameAs(d2));
            Assert.That(order.OrderSum, Is.EqualTo(5m));
        }

        [Test]
        public void RemoveItemFromOrder_Dish_Not_Present_Returns_False()
        {
            var res = CreateReservation("A");
            var order = new Order(res);

            var d1 = CreateDish("Miso Soup", 10m, DishType.Starter);
            var d2 = CreateDish("Cola", 8m, DishType.Drink);

            order.AddItemToOrder(d1);

            var result = order.RemoveItemFromOrder(d2);

            Assert.That(result, Is.False);
            Assert.That(order.Dishes, Has.Count.EqualTo(1));
            Assert.That(order.OrderSum, Is.EqualTo(10m));
        }

        [Test]
        public void RemoveItemFromOrder_Null_Dish_Should_Throw()
        {
            var res = CreateReservation("A");
            var order = new Order(res);

            Assert.Throws<ArgumentNullException>(() => order.RemoveItemFromOrder(null!));
        }
    }
}
