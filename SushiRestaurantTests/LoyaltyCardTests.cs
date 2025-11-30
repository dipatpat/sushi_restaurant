

using NUnit.Framework;
using SushiRestaurant;
using SushiRestaurant.Models;

namespace SushiRestaurantTests
{
    [TestFixture]
    public class LoyaltyCardTests
    {
        private Guest _guest;

        [SetUp]
        public void Setup()
        {
            Guest.ClearExtent();
            _guest = new Guest("John", "Doe");
        }

        [Test]
        public void Should_Create_LoyaltyCard_With_Valid_Data()
        {
            var card = new LoyaltyCard(_guest, "john@example.com", LoyaltyType.standard, 10);

            Assert.That(card.EmailAddress, Is.EqualTo("john@example.com"));
            Assert.That(card.LoyaltyType, Is.EqualTo(LoyaltyType.standard));
            Assert.That(card.NumberOfPoints, Is.EqualTo(10));
            Assert.That(card.Owner, Is.EqualTo(_guest));
            Assert.That(_guest.LoyaltyCard, Is.EqualTo(card));
        }

        [Test]
        public void Should_Throw_When_Email_Invalid()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                new LoyaltyCard(_guest, "not-an-email", LoyaltyType.standard, 10);
            });
        }

        [Test]
        public void Should_Throw_When_Points_Negative()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                new LoyaltyCard(_guest, "a@b.com", LoyaltyType.gold, -5);
            });
        }

        [Test]
        public void Should_Allow_Changing_NumberOfPoints()
        {
            var card = new LoyaltyCard(_guest, "john@example.com", LoyaltyType.standard, 10);

            card.ChangeNumberOfPoints(5);

            Assert.That(card.NumberOfPoints, Is.EqualTo(15));
        }

        [Test]
        public void Should_Throw_When_Changing_Points_To_Negative()
        {
            var card = new LoyaltyCard(_guest, "a@b.com", LoyaltyType.standard, 2);

            Assert.Throws<InvalidOperationException>(() =>
            {
                card.ChangeNumberOfPoints(-5);
            });
        }

        [Test]
        public void Should_Upgrade_Loyalty_Tier()
        {
            var card = new LoyaltyCard(_guest, "john@example.com", LoyaltyType.standard, 10);

            card.UpgradeTier();

            Assert.That(card.LoyaltyType, Is.EqualTo(LoyaltyType.gold));
        }

        [Test]
        public void Should_Not_Upgrade_If_Already_VIP()
        {
            var card = new LoyaltyCard(_guest, "john@example.com", LoyaltyType.vip, 10);

            Assert.Throws<InvalidOperationException>(() =>
            {
                card.UpgradeTier();
            });
        }

    }
}

