using SushiRestaurant;

namespace sushi_restaurant_tests
{
    [TestFixture]
    public class GuestTests
    {
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
    }
}