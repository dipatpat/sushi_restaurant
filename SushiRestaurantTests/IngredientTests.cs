using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace sushi_restaurant_tests
{
    [TestFixture]
    public class IngredientTests
    {
        [SetUp]
        public void Setup()
        {
        Inventory.ClearAllInventory();
        }

        [Test]
        public void AddInventoryBatch_CreatesQualifiedAssociation()
        {
            var ingredient = new Ingredient("Sugar", 100);
            var date1 = new DateTime(2026, 1, 1);
            var batch1 = new Inventory("Sugar", 500, date1);
        
            ingredient.AddInventoryBatch(batch1);
            var retrievedBatch = ingredient.GetInventoryBatch(date1);

            Assert.That(ingredient.GetInventoryBatches().Count, Is.EqualTo(1));
            Assert.That(retrievedBatch, Is.EqualTo(batch1));
        }
    
        [Test]
        public void AddInventoryBatch_PreventsDuplicateQualifier()
        {
            var ingredient = new Ingredient("Salt", 50);
            var date = new DateTime(2026, 2, 1);
            var batchA = new Inventory("Salt", 100, date);
            var batchB = new Inventory("Salt", 200, date);
        
            ingredient.AddInventoryBatch(batchA);
            ingredient.AddInventoryBatch(batchB);

            Assert.That(ingredient.GetInventoryBatches().Count, Is.EqualTo(1)); 
            Assert.That(ingredient.GetInventoryBatch(date), Is.EqualTo(batchA)); 
        }

        [Test]
        public void GetInventoryBatches_ReturnsCopy_PreventsExternalModification()
        {
            var ingredient = new Ingredient("Milk", 200);
            var date = new DateTime(2026, 3, 1);
            var batch = new Inventory("Milk", 1000, date);
            ingredient.AddInventoryBatch(batch);
        
            var externalCopy = ingredient.GetInventoryBatches();
        
            Assert.That(externalCopy.Count, Is.EqualTo(1));
            Assert.That(ingredient.GetInventoryBatches().Count, Is.EqualTo(1)); 
        }

        [Test]
        public void RemoveInventoryBatch_DeletesAssociation()
        {
            var ingredient = new Ingredient("Butter", 50);
            var date = new DateTime(2026, 4, 1);
            var batch = new Inventory("Butter", 500, date);
            ingredient.AddInventoryBatch(batch);

            ingredient.RemoveInventoryBatch(date);
        
            Assert.That(ingredient.GetInventoryBatches().Count, Is.EqualTo(0));
            Assert.That(ingredient.GetInventoryBatch(date), Is.Null);
        }

        [Test]
        public void UseQuantityFromBatch_SufficientQuantity_UpdatesInventory()
        {
            var required = 50;
            var ingredient = new Ingredient("Cream", required);
            var date = new DateTime(2026, 5, 1);
            var initialQuantity = 100;
            var batch = new Inventory("Cream", initialQuantity, date);
            ingredient.AddInventoryBatch(batch);

            ingredient.UseQuantityFromBatch(date);
            
            Assert.That(batch.QuantityLeft, Is.EqualTo(initialQuantity - required)); // 100 - 50 = 50
        }

        [Test]
        public void UseQuantityFromBatch_Insufficient_SwitchesToNextAvailableBatch()
        {
            var required = 50;
            var ingredient = new Ingredient("Yeast", required);
            
            var date1 = new DateTime(2026, 7, 1);
            var batch1 = new Inventory("Yeast", 20, date1);
            
            var date2 = new DateTime(2026, 7, 15);
            var batch2 = new Inventory("Yeast", 100, date2);
            
            Inventory.AddProduct(batch1); 
            Inventory.AddProduct(batch2);
            
            ingredient.AddInventoryBatch(batch1);

            ingredient.UseQuantityFromBatch(date1);

            Assert.That(batch1.QuantityLeft, Is.EqualTo(0));
            
            Assert.That(ingredient.GetInventoryBatch(date1), Is.Null);
            Assert.That(ingredient.GetInventoryBatch(date2), Is.EqualTo(batch2));
            
            Assert.That(batch2.QuantityLeft, Is.EqualTo(70)); 
        }

        [Test]
        public void UseQuantityFromBatch_Insufficient_RequiresMultipleSwitchesInOrder()
        {
            var required = 100;
            var ingredient = new Ingredient("Dough", required);
            
            var date1 = new DateTime(2026, 8, 1);
            var batch1 = new Inventory("Dough", 10, date1);
            
            var date2 = new DateTime(2026, 8, 15);
            var batch2 = new Inventory("Dough", 30, date2);
            
            var date3 = new DateTime(2026, 8, 30);
            var batch3 = new Inventory("Dough", 100, date3);
            
            Inventory.AddProduct(batch1);
            Inventory.AddProduct(batch2);
            Inventory.AddProduct(batch3);
            
            ingredient.AddInventoryBatch(batch1);
            
            ingredient.UseQuantityFromBatch(date1);
            
            Assert.That(batch1.QuantityLeft, Is.EqualTo(0));
            Assert.That(batch2.QuantityLeft, Is.EqualTo(0));
            
            Assert.That(ingredient.GetInventoryBatch(date1), Is.Null);
            Assert.That(ingredient.GetInventoryBatch(date2), Is.EqualTo(batch2));
            Assert.That(ingredient.GetInventoryBatch(date3), Is.EqualTo(batch3));
            Assert.That(batch3.QuantityLeft, Is.EqualTo(40)); 
        }

        [Test]
        public void UseQuantityFromBatch_Insufficient_FailsIfNoOtherBatchesFound()
        {
            var required = 50;
            var ingredient = new Ingredient("Tuna", required);
            
            var date1 = new DateTime(2026, 9, 1);
            var batch1 = new Inventory("Tuna", 20, date1);
            
            Inventory.AddProduct(batch1);
            ingredient.AddInventoryBatch(batch1);
            
            ingredient.UseQuantityFromBatch(date1);

            Assert.That(batch1.QuantityLeft, Is.EqualTo(0));
            Assert.That(ingredient.GetInventoryBatches().Count, Is.EqualTo(0)); 
        }
    }
}

