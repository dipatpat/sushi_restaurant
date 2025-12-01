
using SushiRestaurant;

namespace sushi_restaurant_tests
{
    [TestFixture]
    public class ReservationHistoryTests
    {
        [SetUp]
        public void SetUp()
        {
            Guest.ClearExtent();
            Reservation.ClearExtent();
            Order.ClearExtent();
            Dish.ClearExtent();
        }

        private static Guest CreateGuest(string name = "G")
            => new Guest(name, "Test");

        [Test]
        public void New_Reservation_Has_Initial_TotalCostHistory_Entry_Zero()
        {
            var guest = CreateGuest();
            var res = new Reservation(DateTime.Now.AddHours(3), 2, guest);

            Assert.That(res.TotalCostHistory.Count, Is.EqualTo(1));
            Assert.That(res.TotalCostHistory.First(), Is.EqualTo(0m));
        }

        [Test]
        public void Adding_And_Removing_Dishes_Produces_TotalCost_History_Bag()
        {
            var guest = CreateGuest();
            var res   = new Reservation(DateTime.Now.AddHours(3), 2, guest);
            var order = new Order(res);

            var d1 = new Dish("Miso Soup", 10m, DishType.Starter);
            var d2 = new Dish("Green Tea", 5m, DishType.Drink);

            var initialCount = res.TotalCostHistory.Count;

            order.AddItemToOrder(d1);

            order.AddItemToOrder(d2); 

            order.RemoveItemFromOrder(d1);

            Assert.That(res.TotalCostHistory.Count, Is.EqualTo(initialCount + 3));

            Assert.That(res.TotalCostHistory.Last(), Is.EqualTo(res.TotalCost));
            Assert.That(res.TotalCost, Is.EqualTo(5m));
        }
    }
}
