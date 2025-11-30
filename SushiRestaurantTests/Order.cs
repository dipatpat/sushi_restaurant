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

        private static Reservation CreateReservation()
        {
            return new Reservation(
                DateTime.Now.AddHours(2),
                numberOfGuests: 2,
                totalCost: 0m
            );
        }

        private static Dish CreateDish(string name, decimal price, DishType type)
        {
            return new Dish(name, price, type);
        }


        [Test]
        public void Creating_Order_Adds_It_To_Extent_And_Associates_With_Reservation()
        {
            var reservation = CreateReservation();

            var order = new Order(reservation);

            Assert.That(Order.Extent, Has.Count.EqualTo(1));
            Assert.That(Order.Extent[0], Is.SameAs(order));

            Assert.That(order.Reservation, Is.SameAs(reservation));
            Assert.That(reservation.Orders, Has.Count.EqualTo(1));
            Assert.That(reservation.Orders, Does.Contain(order));
            Assert.That(order.Status, Is.EqualTo(OrderStatus.Pending));
        }


        [Test]
        public void AddItemToOrder_Should_Add_Dish_And_Update_OrderSum()
        {
            var reservation = CreateReservation();
            var order = new Order(reservation);

            var dish1 = CreateDish("Miso Soup", 10m, DishType.Starter);
            var dish2 = CreateDish("Green Tea", 5m, DishType.Drink);

            order.AddItemToOrder(dish1);
            order.AddItemToOrder(dish2);

            Assert.That(order.Dishes, Has.Count.EqualTo(2));
            Assert.That(order.Dishes, Does.Contain(dish1));
            Assert.That(order.Dishes, Does.Contain(dish2));

            Assert.That(order.OrderSum, Is.EqualTo(15m));
        }

        [Test]
        public void AddItemToOrder_Should_Throw_When_Dish_Is_Null()
        {
            var reservation = CreateReservation();
            var order = new Order(reservation);

            Assert.Throws<ArgumentNullException>(() => order.AddItemToOrder(null!));
        }

        [Test]
        public void AddItemToOrder_Should_Not_Allow_Adding_To_Canceled_Or_Completed_Order()
        {
            var reservation = CreateReservation();
            var order = new Order(reservation);

            order.PlaceOrder(true); 
            order.ChangeStatus(OrderStatus.Preparing);
            order.ChangeStatus(OrderStatus.Cooked);
            order.ChangeStatus(OrderStatus.Served);
            order.ChangeStatus(OrderStatus.Completed);

            var dish = CreateDish("Nigiri", 20m, DishType.Sushi);

            Assert.Throws<InvalidOperationException>(() => order.AddItemToOrder(dish));

            var canceledOrder = new Order(reservation);
            canceledOrder.PlaceOrder(false); 

            Assert.Throws<InvalidOperationException>(() => canceledOrder.AddItemToOrder(dish));
        }


        [Test]
        public void PlaceOrder_Success_Should_Move_From_Pending_To_Accepted()
        {
            var order = new Order(CreateReservation());

            order.PlaceOrder(true);

            Assert.That(order.Status, Is.EqualTo(OrderStatus.Accepted));
        }

        [Test]
        public void PlaceOrder_Failure_Should_Move_From_Pending_To_Canceled()
        {
            var order = new Order(CreateReservation());

            order.PlaceOrder(false);

            Assert.That(order.Status, Is.EqualTo(OrderStatus.Canceled));
        }

        [Test]
        public void PlaceOrder_Called_From_Non_Pending_State_Should_Throw()
        {
            var order = new Order(CreateReservation());
            order.PlaceOrder(true); 

            Assert.Throws<InvalidOperationException>(() => order.PlaceOrder(true));
        }

        [Test]
        public void CancelOrder_From_Pending_Or_Accepted_Should_Set_Status_To_Canceled()
        {
            var orderPending = new Order(CreateReservation());
            orderPending.CancelOrder();
            Assert.That(orderPending.Status, Is.EqualTo(OrderStatus.Canceled));

            var orderAccepted = new Order(CreateReservation());
            orderAccepted.PlaceOrder(true); 
            orderAccepted.CancelOrder();
            Assert.That(orderAccepted.Status, Is.EqualTo(OrderStatus.Canceled));
        }

        [Test]
        public void CancelOrder_From_Invalid_State_Should_Throw()
        {
            var order = new Order(CreateReservation());
            order.PlaceOrder(false);   

            Assert.Throws<InvalidOperationException>(() => order.CancelOrder());
        }


        [Test]
        public void ChangeStatus_Should_Follow_Valid_Transition_Sequence()
        {
            var order = new Order(CreateReservation());
            order.PlaceOrder(true); 

            order.ChangeStatus(OrderStatus.Preparing);
            Assert.That(order.Status, Is.EqualTo(OrderStatus.Preparing));

            order.ChangeStatus(OrderStatus.Cooked);
            Assert.That(order.Status, Is.EqualTo(OrderStatus.Cooked));

            order.ChangeStatus(OrderStatus.Served);
            Assert.That(order.Status, Is.EqualTo(OrderStatus.Served));

            order.ChangeStatus(OrderStatus.Completed);
            Assert.That(order.Status, Is.EqualTo(OrderStatus.Completed));
        }

        [Test]
        public void ChangeStatus_Invalid_Transition_Should_Throw()
        {
            var order = new Order(CreateReservation());
            order.PlaceOrder(true); 

            Assert.Throws<InvalidOperationException>(() =>
                order.ChangeStatus(OrderStatus.Cooked));

            order.ChangeStatus(OrderStatus.Preparing);

            Assert.Throws<InvalidOperationException>(() =>
                order.ChangeStatus(OrderStatus.Served));
        }
    }
}
