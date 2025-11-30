using System.Collections;
using SushiRestaurant;

namespace sushi_restaurant_tests
{
    [TestFixture]
    public class DishTests
    {
        [SetUp]
        public void SetUp()
        {
            Dish.ClearExtent();
        }

        [Test]
        public void Constructor_ValidData_CreatesDishAndAddsToExtent()
        {
            var name = "Miso Soup";
            var price = 10m;
            var type = DishType.Starter;

            var dish = new Dish(name, price, type);

            Assert.That(dish.DishName, Is.EqualTo(name));
            Assert.That(dish.Price, Is.EqualTo(price));
            Assert.That(dish.DishType, Is.EqualTo(type));

            Assert.That(Dish.Extent, Has.Count.EqualTo(1));
            Assert.That(Dish.Extent[0], Is.SameAs(dish));
        }

        [Test]
        public void Constructor_EmptyName_ThrowsArgumentException()
        {
            var ex = Assert.Throws<ArgumentException>(() =>
                new Dish("   ", 10m, DishType.Sushi));

            Assert.That(ex!.ParamName, Is.Null.Or.EqualTo("value").Or.EqualTo("DishName"));
        }

        [Test]
        public void Constructor_NameTooLong_ThrowsArgumentException()
        {
            var longName = new string('A', 21);

            var ex = Assert.Throws<ArgumentException>(() =>
                new Dish(longName, 10m, DishType.Sushi));

            Assert.That(ex!.Message, Does.Contain("cannot exceed 20").IgnoreCase);
        }

        [Test]
        public void Setting_DishName_TrimsWhitespace()
        {
            var dish = new Dish(" Nigiri  ", 12m, DishType.Sushi);

            Assert.That(dish.DishName, Is.EqualTo("Nigiri"));
        }

        [Test]
        public void Constructor_NonPositivePrice_ThrowsArgumentOutOfRange()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                new Dish("Test", 0m, DishType.Drink));

            Assert.Throws<ArgumentOutOfRangeException>(() =>
                new Dish("Test", -5m, DishType.Drink));
        }

        [Test]
        public void AddNewDish_CreatesDishAndAddsToExtent()
        {
            var dish = Dish.AddNewDish("California Roll", 25m, DishType.Sushi);

            Assert.That(dish.DishName, Is.EqualTo("California Roll"));
            Assert.That(dish.Price, Is.EqualTo(25m));
            Assert.That(dish.DishType, Is.EqualTo(DishType.Sushi));

            Assert.That(Dish.Extent, Has.Count.EqualTo(1));
            Assert.That(Dish.Extent[0], Is.SameAs(dish));
        }

        [Test]
        public void DisplayDetailedInformation_WritesExpectedOutput()
        {
            var dish = new Dish("Salmon Nigiri", 15m, DishType.Sushi);

            var sw = new StringWriter();
            var originalOut = Console.Out;
            Console.SetOut(sw);

            try
            {
                dish.DisplayDetailedInformation();
            }
            finally
            {
                Console.SetOut(originalOut);
            }

            var output = sw.ToString();

            Assert.That(output, Does.Contain("Salmon Nigiri"));
            Assert.That(output, Does.Contain("Sushi"));
            Assert.That(output, Does.Contain("15"));
        }

        [Test]
        public void DisplayMenu_WritesAllDishes()
        {
            var d1 = new Dish("Miso Soup", 10m, DishType.Starter);
            var d2 = new Dish("Green Tea", 5m, DishType.Drink);

            var sw = new StringWriter();
            var originalOut = Console.Out;
            Console.SetOut(sw);

            try
            {
                Dish.DisplayMenu();
            }
            finally
            {
                Console.SetOut(originalOut);
            }

            var output = sw.ToString();

            Assert.That(output, Does.Contain("Miso Soup"));
            Assert.That(output, Does.Contain("Green Tea"));
        }

        [Test]
        public void Extent_IsReadOnly_ExternalCodeCannotModifyExtent()
        {
            var d1 = new Dish("Tempura", 18m, DishType.Starter);

            var extent = Dish.Extent;

            Assert.Throws<NotSupportedException>(() =>
            {
                var list = (IList)extent;
                list.Add(d1);
            });

            Assert.That(Dish.Extent.Count, Is.EqualTo(1));
            Assert.That(Dish.Extent[0], Is.SameAs(d1));
        }
    }
}
