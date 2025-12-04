using SushiRestaurant;

namespace sushi_restaurant_tests
{
    [TestFixture]
    public class GuestTests
    {
        [SetUp]
        public void SetUp()
        {
            Guest.ClearExtent();
            Reservation.ClearExtent();
        }

        [Test]
        public void Should_Assign_FirstName_LastName_And_Nickname()
        {
            var guest = new Guest
            {
                FirstName = "Aiko",
                LastName = "Sato",
                Nickname = "SushiQueen"
            };

            Assert.That(guest.FirstName, Is.EqualTo("Aiko"));
            Assert.That(guest.LastName, Is.EqualTo("Sato"));
            Assert.That(guest.Nickname, Is.EqualTo("SushiQueen"));
        }

        [Test]
        public void Should_Allow_Null_Nickname()
        {
            var guest = new Guest
            {
                FirstName = "Nick",
                LastName = "Moon",
                Nickname = null
            };

            Assert.That(guest.Nickname, Is.Null);
            Assert.That(guest.FirstName, Is.EqualTo("Nick"));
            Assert.That(guest.LastName, Is.EqualTo("Moon"));
        }

        [Test]
        public void New_Guest_Should_Have_Empty_Reservations_Collection()
        {
            var guest = new Guest("Anna", "Nowak");

            Assert.That(guest.Reservations, Is.Empty);
        }

        [Test]
        public void Creating_Reservation_Associates_It_With_Guest_And_Updates_Reverse()
        {
            var guest = new Guest("Anna", "Nowak");

            var reservation = new Reservation(
                DateTime.Now.AddHours(3),
                numberOfGuests: 2,
                guest: guest);

            Assert.That(reservation.Guest, Is.SameAs(guest));

            Assert.That(guest.Reservations, Has.Count.EqualTo(1));
            Assert.That(guest.Reservations.First(), Is.SameAs(reservation));
        }
    }
}
