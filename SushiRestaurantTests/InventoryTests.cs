using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

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
        public void InventoryConstructor_SetsPropertiesCorrectly()
        {
            var date = new DateTime(2026, 1, 15);
            var expiry = new DateTime(2026, 7, 15);

            var batch = new Inventory("Cheese", 50, date, expiry);

            Assert.That(batch.ProductName, Is.EqualTo("Cheese"));
            Assert.That(batch.QuantityLeft, Is.EqualTo(50));
            Assert.That(batch.PurchaseDate, Is.EqualTo(date));
            Assert.That(batch.ExpirationDate, Is.EqualTo(expiry));
        }
        
        [Test]
        public void InventoryConstructor_HandlesNullExpirationDate()
        {
            var date = new DateTime(2026, 2, 1);
            var batch = new Inventory("Wine", 10, date);

            Assert.That(batch.ExpirationDate, Is.Null);
        }

        [Test]
        public void AddProduct_AddsToGlobalInventoryList()
        {
            var batch1 = new Inventory("Tomato", 100, new DateTime(2026, 3, 1));
            var batch2 = new Inventory("Onion", 50, new DateTime(2026, 3, 2));

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
            var batch = new Inventory("Potato", 100, new DateTime(2026, 4, 1));
            Inventory.AddProduct(batch);

            var list = Inventory.ListAllInventory();

            Assert.That(list, Is.InstanceOf<IReadOnlyList<Inventory>>());
            Assert.That(list.Count, Is.EqualTo(1));
        }
    }
}

