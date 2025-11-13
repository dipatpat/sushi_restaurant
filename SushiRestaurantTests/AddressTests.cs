using SushiRestaurant;
using NUnit.Framework;


namespace sushi_restaurant_tests
{
    [TestFixture]
    public class AddressTests
    {
        [Test]
        public void Should_Assign_AllAddressFields_Correctly()
        {
            var address = new Address
            {
                StreetName = "Sakura Street",
                StreetNumber = "12A",
                PostalCode = "100-0001",
                CityName = "Tokyo",
                ApartmentNumber = "3B",
                DoorNumber = "2"
            };

            Assert.That("Sakura Street", Is.EqualTo(address.StreetName));
            Assert.That("12A", Is.EqualTo(address.StreetNumber));
            Assert.That("100-0001", Is.EqualTo(address.PostalCode));
            Assert.That("Tokyo", Is.EqualTo(address.CityName));
            Assert.That("3B", Is.EqualTo(address.ApartmentNumber));
            Assert.That("2", Is.EqualTo(address.DoorNumber));
        }

        [Test]
        public void Should_Allow_Empty_Optional_Fields()
        {
            var address = new Address
            {
                StreetName = "Shinjuku Avenue",
                CityName = "Tokyo"
            };

            Assert.That("Shinjuku Avenue", Is.EqualTo(address.StreetName));
            Assert.That("Tokyo", Is.EqualTo(address.CityName));
            Assert.That(address.ApartmentNumber, Is.Null);
            Assert.That(address.DoorNumber, Is.Null);
        }
    }
}