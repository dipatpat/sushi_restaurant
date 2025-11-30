using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using SushiRestaurant;

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
        public void Constructor_EmptyName_ThrowsArgumentException()
        {
            var ex = Assert.Throws<ArgumentException>(() => new Ingredient("   ", 1));

            Assert.That(ex!.ParamName, Is.EqualTo("Name"));
            Assert.That(ex.Message, Does.Contain("Ingredient name is required."));
        }

        [Test]
        public void Constructor_NonPositiveQuantity_ThrowsArgumentOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new Ingredient("Water", 0));
            Assert.Throws<ArgumentOutOfRangeException>(() => new Ingredient("Water", -10));
        }

        [Test]
        public void AddInventoryBatch_CreatesQualifiedAssociation()
        {
            var ingredient = new Ingredient("Sugar", 100);
            var date1 = new DateTime(2025, 1, 1);
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
            var date = new DateTime(2025, 2, 1);
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
            var date = new DateTime(2025, 3, 1);
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
            var date = new DateTime(2025, 4, 1);
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
            var date = new DateTime(2025, 5, 1);
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
            
            var date1 = new DateTime(2025, 7, 1);
            var batch1 = new Inventory("Yeast", 20, date1);
            
            var date2 = new DateTime(2025, 7, 15);
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
            
            var date1 = new DateTime(2025, 8, 1);
            var batch1 = new Inventory("Dough", 10, date1);
            
            var date2 = new DateTime(2025, 8, 15);
            var batch2 = new Inventory("Dough", 30, date2);
            
            var date3 = new DateTime(2025, 8, 30);
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
            
            var date1 = new DateTime(2025, 9, 1);
            var batch1 = new Inventory("Tuna", 20, date1);
            
            Inventory.AddProduct(batch1);
            ingredient.AddInventoryBatch(batch1);
            
            ingredient.UseQuantityFromBatch(date1);

            Assert.That(batch1.QuantityLeft, Is.EqualTo(0));
            Assert.That(ingredient.GetInventoryBatches().Count, Is.EqualTo(0)); 
        }

        [Test]
        public void GetPartByDishes_ReturnsCopy_PreventsExternalModification()
        {
            var ingredient = new Ingredient("Wasabi Paste", 5);
            var dish = new Dish("Wasabi Bomb", 1m, DishType.Sushi);
            dish.AddIngredient(ingredient);

            var externalCopy = ingredient.GetPartByDishes();

            Assert.That(externalCopy.Count, Is.EqualTo(1));
            Assert.That(ingredient.GetPartByDishes().Count, Is.EqualTo(1)); 
        }

        [Test]
        public void AddDish_InternalMethod_CreatesLinkFromDishCall()
        {
            var ingredient = new Ingredient("Avocado", 50);
            var dish1 = new Dish("Avocado Roll", 10m, DishType.Sushi);
            var dish2 = new Dish("Veggie Bowl", 15m, DishType.Starter);

            dish1.AddIngredient(ingredient);
            dish2.AddIngredient(ingredient);

            var linkedDishes = ingredient.GetPartByDishes();
            Assert.That(linkedDishes, Contains.Item(dish1));
            Assert.That(linkedDishes, Contains.Item(dish2));
            Assert.That(linkedDishes.Count, Is.EqualTo(2));
        }

        [Test]
        public void RemoveDish_InternalMethod_RemovesLinkFromDishCall()
        {
            var ingredient = new Ingredient("Soy Sauce", 10);
            var dish1 = new Dish("Sashimi Platter", 30m, DishType.Sushi);
            var dish2 = new Dish("Edamame", 5m, DishType.Starter);
            
            dish1.AddIngredient(ingredient);
            dish2.AddIngredient(ingredient);

            dish1.RemoveIngredient(ingredient);

            var linkedDishes = ingredient.GetPartByDishes();
            Assert.That(linkedDishes, Does.Not.Contain(dish1));
            Assert.That(linkedDishes, Contains.Item(dish2));
            Assert.That(linkedDishes.Count, Is.EqualTo(1));
        }
    }
}

