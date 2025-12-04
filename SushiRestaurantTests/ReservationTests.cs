using SushiRestaurant;

namespace sushi_restaurant_tests
{
    [TestFixture]
    public class ReservationTests
    {
        private static Guest CreateGuest(string first = "Anna", string last = "Nowak") =>
            new Guest(first, last);

        [SetUp]
        public void SetUp()
        {
            Guest.ClearExtent();
            Reservation.ClearExtent();
            Order.ClearExtent();
            Dish.ClearExtent();
        }

        [Test]
        public void Should_Calculate_EndDateTime_As_Start_Plus_3_Hours()
        {
            var start = new DateTime(2025, 12, 12, 18, 0, 0);
            var reservation = new Reservation { StartDateTime = start };

            var expectedEnd = start.AddHours(Reservation.DurationHours);

            Assert.That(reservation.EndDateTime, Is.EqualTo(expectedEnd));
        }

        [Test]
        public void Should_Allow_Valid_ReviewScore()
        {
            var reservation = new Reservation();
            reservation.ReviewScore = 4;

            Assert.That(reservation.ReviewScore, Is.EqualTo(4));
        }

        [Test]
        public void Should_Reject_Invalid_ReviewScore_Less_Than_0()
        {
            var reservation = new Reservation();

            Assert.Throws<ArgumentOutOfRangeException>(
                () => reservation.ReviewScore = -2,
                "Setting a review score < 0 should throw an error"
            );
        }

        [Test]
        public void Should_Reject_Invalid_ReviewScore_Greater_Than_5()
        {
            var reservation = new Reservation();

            Assert.Throws<ArgumentOutOfRangeException>(
                () => reservation.ReviewScore = 10,
                "Setting a review score > 5 should throw an error"
            );
        }

        [Test]
        public void Should_Allow_Valid_NumberOfGuests()
        {
            var reservation = new Reservation();
            reservation.NumberOfGuests = 6;

            Assert.That(reservation.NumberOfGuests, Is.EqualTo(6));
        }

        [Test]
        public void Should_Reject_NumberOfGuests_Less_Than_1()
        {
            var reservation = new Reservation();

            Assert.Throws<ArgumentOutOfRangeException>(
                () => reservation.NumberOfGuests = 0,
                "Guests < 1 should trigger exception"
            );
        }

        [Test]
        public void Should_Reject_NumberOfGuests_Greater_Than_10()
        {
            var reservation = new Reservation();

            Assert.Throws<ArgumentOutOfRangeException>(
                () => reservation.NumberOfGuests = 15,
                "Guests > 10 should trigger exception"
            );
        }

        [Test]
        public void Should_Default_BonusPoints_To_Zero()
        {
            var reservation = new Reservation();

            Assert.That(reservation.BonusPoints, Is.EqualTo(0));
        }

        [Test]
        public void Should_Store_Comment_And_Payment_Status()
        {
            var reservation = new Reservation
            {
                Comment = "Perfect dinner experience!",
                IsPaid = true
            };

            Assert.That(reservation.Comment, Is.EqualTo("Perfect dinner experience!"));
            Assert.That(reservation.IsPaid, Is.True);
        }
        
        [Test]
        public void Reservation_Created_With_Guest_Is_Associated_Both_Ways()
        {
            var guest = CreateGuest();
            var reservation = new Reservation(
                DateTime.Now.AddHours(2),
                numberOfGuests: 2,
                guest: guest);

            Assert.That(reservation.Guest, Is.SameAs(guest));

            Assert.That(guest.Reservations, Has.Count.EqualTo(1));
            Assert.That(guest.Reservations.First(), Is.SameAs(reservation));
        }

        [Test]
        public void ChangeGuest_Moves_Reservation_Between_Guests_And_Updates_Reverse()
        {
            var guest1 = CreateGuest("Anna", "Nowak");
            var guest2 = CreateGuest("John", "Smith");

            var reservation = new Reservation(
                DateTime.Now.AddHours(2),
                numberOfGuests: 2,
                guest: guest1);

            reservation.ChangeGuest(guest2);

            Assert.That(reservation.Guest, Is.SameAs(guest2));

            Assert.That(guest1.Reservations, Is.Empty);

            Assert.That(guest2.Reservations, Has.Count.EqualTo(1));
            Assert.That(guest2.Reservations.First(), Is.SameAs(reservation));
        }

        [Test]
        public void ChangeGuest_With_Null_Should_Throw()
        {
            var guest1 = CreateGuest();
            var reservation = new Reservation(
                DateTime.Now.AddHours(2),
                numberOfGuests: 2,
                guest: guest1);

            Assert.Throws<ArgumentNullException>(() => reservation.ChangeGuest(null!));
        }

        [Test]
        public void ChangeGuest_To_Same_Guest_Does_Nothing()
        {
            var guest1 = CreateGuest();
            var reservation = new Reservation(
                DateTime.Now.AddHours(2),
                numberOfGuests: 2,
                guest: guest1);

            reservation.ChangeGuest(guest1); 

            Assert.That(reservation.Guest, Is.SameAs(guest1));
            Assert.That(guest1.Reservations, Has.Count.EqualTo(1));
            Assert.That(guest1.Reservations.First(), Is.SameAs(reservation));
        }


        [Test]
        public void New_Reservation_Should_Have_Empty_Orders_Collection()
        {
            var reservation = new Reservation();

            Assert.That(reservation.Orders, Is.Empty);
        }

        [Test]
        public void Creating_Order_For_Reservation_Adds_It_To_Orders_Collection()
        {
            var guest = CreateGuest();
            var reservation = new Reservation(
                DateTime.Now.AddHours(2),
                numberOfGuests: 2,
                guest: guest);

            var order = new Order(reservation);

            Assert.That(reservation.Orders, Has.Count.EqualTo(1));
            Assert.That(reservation.Orders.First(), Is.SameAs(order));
        }

        [Test]
        public void GetTotalCost_Should_Sum_OrderSums_For_All_Orders()
        {
            var guest = CreateGuest();
            var reservation = new Reservation(
                DateTime.Now.AddHours(2),
                numberOfGuests: 2,
                guest: guest);

            var order1 = new Order(reservation);
            var order2 = new Order(reservation);

            order1.AddItemToOrder(new Dish("Miso Soup", 10m, DishType.Starter));
            order1.AddItemToOrder(new Dish("Green Tea", 5m, DishType.Drink));

            order2.AddItemToOrder(new Dish("Salmon Nigiri", 20m, DishType.Sushi));

            var expectedTotal = 10m + 5m + 20m;

            Assert.That(reservation.GetTotalCost(), Is.EqualTo(expectedTotal));
            Assert.That(reservation.TotalCost, Is.EqualTo(expectedTotal));
        }
    }
}
