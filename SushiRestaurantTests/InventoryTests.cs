using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using SushiRestaurant;

namespace sushi_restaurant_tests
{
    [TestFixture]
    public class InventoryTests
    {
        [SetUp]
        public void Setup()
        {
            Inventory.ClearAllInventory();
        }

        [Test]
        public void Constructor_EmptyProductName_ThrowsArgumentException()
        {
            var ex = Assert.Throws<ArgumentException>(() =>
                new Inventory("   ", 100, DateTime.Today));

            Assert.That(ex!.ParamName, Is.EqualTo("ProductName"));
            Assert.That(ex.Message, Does.Contain("Product name is required."));
        }

        [Test]
        public void Constructor_NegativeQuantityLeft_ThrowsArgumentOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new Inventory("Rice", -10, DateTime.Today));
        }
        
        [Test]
        public void Constructor_FuturePurchaseDate_ThrowsArgumentOutOfRangeException()
        {
            var tomorrow = DateTime.Today.AddDays(1);

            Assert.Throws<ArgumentOutOfRangeException>(() =>
                new Inventory("Fish", 10, tomorrow));
        }

        [Test]
        public void QuantityLeft_SetNegative_ThrowsArgumentOutOfRangeException()
        {
            var batch = new Inventory("Oil", 100, DateTime.Today);

            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => batch.QuantityLeft = -5);
            
            Assert.That(ex!.ParamName, Is.EqualTo("QuantityLeft"));
        }
        
        [Test]
        public void ExpirationDate_SetBeforePurchaseDate_ThrowsArgumentOutOfRangeException()
        {
            var purchaseDate = new DateTime(2025, 10, 15);
            var expirationDate = new DateTime(2025, 10, 14);
            
            var batch = new Inventory("Wasabi", 10, purchaseDate);

            var ex = Assert.Throws<ArgumentOutOfRangeException>(() =>
                batch.ExpirationDate = expirationDate);
            
            Assert.That(ex!.ParamName, Is.EqualTo("ExpirationDate"));
            Assert.That(ex.Message, Does.Contain("Expiration date cannot be before the purchase date."));
        }
        
        [Test]
        public void ExpirationDate_SetValidDate_Succeeds()
        {
            var purchaseDate = new DateTime(2025, 10, 15);
            var expirationDate = new DateTime(2025, 10, 16);
            
            var batch = new Inventory("Ginger", 10, purchaseDate);
            batch.ExpirationDate = expirationDate;

            Assert.That(batch.ExpirationDate, Is.EqualTo(expirationDate));
        }

        [Test]
        public void InventoryConstructor_SetsPropertiesCorrectly()
        {
            var date = new DateTime(2025, 1, 15);
            var expiry = new DateTime(2025, 7, 15);

            var batch = new Inventory("Cheese", 50, date, expiry);

            Assert.That(batch.ProductName, Is.EqualTo("Cheese"));
            Assert.That(batch.QuantityLeft, Is.EqualTo(50));
            Assert.That(batch.PurchaseDate, Is.EqualTo(date));
            Assert.That(batch.ExpirationDate, Is.EqualTo(expiry));
        }
        
        [Test]
        public void InventoryConstructor_HandlesNullExpirationDate()
        {
            var date = new DateTime(2025, 2, 1);
            var batch = new Inventory("Wine", 10, date);

            Assert.That(batch.ExpirationDate, Is.Null);
        }

        [Test]
        public void AddProduct_AddsToGlobalInventoryList()
        {
            var batch1 = new Inventory("Tomato", 100, new DateTime(2025, 3, 1));
            var batch2 = new Inventory("Onion", 50, new DateTime(2025, 3, 2));

            Inventory.AddProduct(batch1);
            Inventory.AddProduct(batch2);
            
            var allInventory = Inventory.ListAllInventory();

            Assert.That(allInventory.Count, Is.EqualTo(2));
            Assert.That(allInventory, Contains.Item(batch1));
            Assert.That(allInventory, Contains.Item(batch2));
        }
        
        [Test]
        public void ListAllInventory_ReturnsReadOnlyList()
        {
            var batch = new Inventory("Potato", 100, new DateTime(2025, 4, 1));
            Inventory.AddProduct(batch);

            var list = Inventory.ListAllInventory();

            Assert.That(list, Is.InstanceOf<IReadOnlyList<Inventory>>());
            Assert.That(list.Count, Is.EqualTo(1));
        }
    }
}

