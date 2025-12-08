using SushiRestaurant;

namespace sushi_restaurant_tests
{
    [TestFixture]
    public class DishInOrderReverseConnectionTests
    {
        [SetUp]
        public void SetUp()
        {
            Guest.ClearExtent();
            Reservation.ClearExtent();
            Order.ClearExtent();
            Dish.ClearExtent();
            DishInOrder.ClearExtent();
        }

        private static Order CreateSampleOrder()
        {
            var guest = new Guest("Anna", "Nowak");
            var table = new Table(1, 2);
            var reservation = new Reservation(
                DateTime.Now.AddHours(3),
                numberOfGuests: 2,
                guest: guest,
                table: table);

            return new Order(reservation);
        }

        private static Dish CreateSampleDish()
        {
            return new Dish("California Roll", 25m, DishType.Sushi);
        }

        [Test]
        public void AddItem_From_Order_Side_Should_Update_Both_Sides()
        {
            var order = CreateSampleOrder();
            var dish = CreateSampleDish();

            var item = order.AddItemToOrder(dish, quantity: 3);

            Assert.Multiple(() =>
            {
                Assert.That(item.Order, Is.SameAs(order));
                Assert.That(item.Dish, Is.SameAs(dish));
                Assert.That(item.Quantity, Is.EqualTo(3));

                Assert.That(order.Items, Has.Count.EqualTo(1));
                Assert.That(order.Items.Single(), Is.SameAs(item));

                Assert.That(dish.DishInOrders, Has.Count.EqualTo(1));
                Assert.That(dish.DishInOrders.Single(), Is.SameAs(item));
            });
        }

        [Test]
        public void AddItem_From_Dish_Side_Should_Update_Both_Sides()
        {
            var order = CreateSampleOrder();
            var dish = CreateSampleDish();

            var item = dish.AddToOrder(order, quantity: 2);

            Assert.Multiple(() =>
            {
                Assert.That(item.Order, Is.SameAs(order));
                Assert.That(item.Dish, Is.SameAs(dish));
                Assert.That(item.Quantity, Is.EqualTo(2));

                Assert.That(order.Items.Single(), Is.SameAs(item));
                Assert.That(dish.DishInOrders.Single(), Is.SameAs(item));
            });
        }

        [Test]
        public void Remove_From_Association_Object_Should_Clear_Both_Sides()
        {
            var order = CreateSampleOrder();
            var dish = CreateSampleDish();
            var item = order.AddItemToOrder(dish, 1);

            item.Remove();

            Assert.Multiple(() =>
            {
                Assert.That(order.Items, Is.Empty);
                Assert.That(dish.DishInOrders, Is.Empty);
                Assert.That(DishInOrder.Extent, Is.Empty);
            });
        }

        [Test]
        public void Remove_From_Order_Side_Should_Clear_Both_Sides()
        {
            var order = CreateSampleOrder();
            var dish = CreateSampleDish();
            var item = order.AddItemToOrder(dish, 1);

            order.RemoveItemFromOrder(item);

            Assert.Multiple(() =>
            {
                Assert.That(order.Items, Is.Empty);
                Assert.That(dish.DishInOrders, Is.Empty);
                Assert.That(DishInOrder.Extent, Is.Empty);
            });
        }

        [Test]
        public void Creating_DishInOrder_Directly_Should_Update_Order_And_Dish_Collections()
        {
            var order = CreateSampleOrder();
            var dish = CreateSampleDish();

            var item = new DishInOrder(dish, order, quantity: 2);

            Assert.Multiple(() =>
            {
                Assert.That(item.Order, Is.SameAs(order));
                Assert.That(item.Dish, Is.SameAs(dish));
                Assert.That(item.Quantity, Is.EqualTo(2));

                Assert.That(order.Items, Has.Count.EqualTo(1));
                Assert.That(order.Items.Single(), Is.SameAs(item));

                Assert.That(dish.DishInOrders, Has.Count.EqualTo(1));
                Assert.That(dish.DishInOrders.Single(), Is.SameAs(item));

                Assert.That(DishInOrder.Extent.Count, Is.EqualTo(1));
                Assert.That(DishInOrder.Extent.Single(), Is.SameAs(item));
            });
        }
    }
}

