using NUnit.Framework;
using SushiRestaurant.Models;
using System;
using SushiRestaurant;

namespace SushiRestaurantTests
{
    [TestFixture]
    public class GuestLoyaltyCardTests
    {
        private Guest _guest;

        [SetUp]
        public void Setup()
        {
            Guest.ClearExtent();
            _guest = new Guest("John", "Doe");
        }

        [Test]
        public void Should_Use_Strict_Composition()
        {
            var card = _guest.CreateLoyaltyCard("alice@example.com", LoyaltyType.gold, 50);

            _guest.RemoveLoyaltyCard();

            Assert.That(_guest.LoyaltyCard, Is.Null);

            Assert.That(card.Owner, Is.EqualTo(_guest));
        }

        [Test]
        public void Guest_Cannot_Have_Two_Cards()
        {
            _guest.CreateLoyaltyCard("a@a.com", LoyaltyType.standard, 1);

            Assert.Throws<InvalidOperationException>(() =>
            {
                _guest.CreateLoyaltyCard("b@b.com", LoyaltyType.gold, 10);
            });
        }

        [Test]
        public void Removing_Card_Sets_LoyaltyCard_To_Null()
        {
            var card = _guest.CreateLoyaltyCard("x@y.com", LoyaltyType.standard, 5);

            _guest.RemoveLoyaltyCard();

            Assert.That(_guest.LoyaltyCard, Is.Null);
        }

        [Test]
        public void CreateLoyaltyCard_Should_Throw_When_Email_Invalid()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                _guest.CreateLoyaltyCard("not-email", LoyaltyType.gold, 5);
            });
        }

        [Test]
        public void Should_Upgrade_Loyalty_Tier()
        {
            var card = _guest.CreateLoyaltyCard("john@example.com", LoyaltyType.standard, 10);

            card.UpgradeTier();

            Assert.That(card.LoyaltyType, Is.EqualTo(LoyaltyType.gold));
        }

        [Test]
        public void Should_Throw_When_Upgrading_From_VIP()
        {
            var card = _guest.CreateLoyaltyCard("john@example.com", LoyaltyType.vip, 10);

            Assert.Throws<InvalidOperationException>(() =>
            {
                card.UpgradeTier();
            });
        }
    }
}
