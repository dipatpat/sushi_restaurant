using SushiRestaurant;

namespace sushi_restaurant_tests
{
    [TestFixture]
    public class DishInOrderTests
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
        public void Constructor_Should_Assign_Dish_Order_Quantity_And_Time()
        {
            var order = CreateSampleOrder();
            var dish = CreateSampleDish();

            var before = DateTime.Now;
            var dishInOrder = new DishInOrder(dish, order, quantity: 3);
            var after = DateTime.Now;

            Assert.Multiple(() =>
            {
                Assert.That(dishInOrder.Dish, Is.SameAs(dish));
                Assert.That(dishInOrder.Order, Is.SameAs(order));
                Assert.That(dishInOrder.Quantity, Is.EqualTo(3));

                Assert.That(dishInOrder.TimeOrdered, Is.InRange(before, after));

                Assert.That(DishInOrder.Extent, Has.Count.EqualTo(1));
                Assert.That(DishInOrder.Extent[0], Is.SameAs(dishInOrder));
            });
        }

        [Test]
        public void Constructor_Should_Throw_When_Dish_Is_Null()
        {
            var order = CreateSampleOrder();

            Assert.That(
                () => new DishInOrder(null!, order, 1),
                Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void Constructor_Should_Throw_When_Order_Is_Null()
        {
            var dish = CreateSampleDish();

            Assert.That(
                () => new DishInOrder(dish, null!, 1),
                Throws.TypeOf<ArgumentNullException>());
        }

        [TestCase(0)]
        [TestCase(-1)]
        public void Constructor_Should_Throw_When_Quantity_Not_Positive(int invalidQuantity)
        {
            var order = CreateSampleOrder();
            var dish = CreateSampleDish();

            Assert.That(
                () => new DishInOrder(dish, order, invalidQuantity),
                Throws.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void ChangeQuantity_Should_Update_Quantity()
        {
            var order = CreateSampleOrder();
            var dish = CreateSampleDish();
            var dishInOrder = new DishInOrder(dish, order, 2);

            dishInOrder.ChangeQuantity(5);

            Assert.That(dishInOrder.Quantity, Is.EqualTo(5));
        }

        [TestCase(0)]
        [TestCase(-3)]
        public void ChangeQuantity_Should_Throw_When_NewQuantity_Not_Positive(int invalidQuantity)
        {
            var order = CreateSampleOrder();
            var dish = CreateSampleDish();
            var dishInOrder = new DishInOrder(dish, order, 2);

            Assert.That(
                () => dishInOrder.ChangeQuantity(invalidQuantity),
                Throws.TypeOf<ArgumentOutOfRangeException>());
        }
    }
}
