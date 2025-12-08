using NUnit.Framework;
using SushiRestaurant;
using System;
using System.Linq;

namespace sushi_restaurant_tests
{
    [TestFixture]
    public class OrderTests
    {
        [SetUp]
        public void SetUp()
        {
            Order.ClearExtent();
            Reservation.ClearExtent();
            Dish.ClearExtent();
            DishInOrder.ClearExtent();
            Guest.ClearExtent();
        }

        private static Reservation CreateReservation(string label = "A")
        {
            var guest = new Guest
            {
                FirstName = "Aiko",
                LastName = "Sato",
                Nickname = "SushiQueen"
            };
            var table = new Table(3, 2);

            return new Reservation(
                DateTime.Now.AddHours(2),
                numberOfGuests: 2,
                guest: guest,
                table: table
            )
            {
                Comment = $"Res-{label}"
            };
        }

        private static Dish CreateDish(string name, decimal price, DishType type)
        {
            return new Dish(name, price, type);
        }

        // ------------------------------------------------------------
        // ORDER CREATION TESTS
        // ------------------------------------------------------------

        [Test]
        public void Creating_Order_Associates_With_Reservation_And_Updates_Reverse()
        {
            var res = CreateReservation("A");
            var order = new Order(res);

            Assert.Multiple(() =>
            {
                Assert.That(order.Reservation, Is.SameAs(res));
                Assert.That(res.Orders, Has.Count.EqualTo(1));
                Assert.That(res.Orders.First(), Is.SameAs(order));

                Assert.That(Order.Extent, Has.Count.EqualTo(1));
                Assert.That(Order.Extent[0], Is.SameAs(order));
            });
        }

        [Test]
        public void Creating_Order_With_Null_Reservation_Should_Throw()
        {
            Assert.Throws<ArgumentNullException>(() => new Order(null!));
        }

        // ------------------------------------------------------------
        // CHANGE RESERVATION
        // ------------------------------------------------------------

        [Test]
        public void ChangeReservation_Moves_Order_Between_Reservations()
        {
            var res1 = CreateReservation("A");
            var res2 = CreateReservation("B");
            var order = new Order(res1);

            order.ChangeReservation(res2);

            Assert.Multiple(() =>
            {
                Assert.That(order.Reservation, Is.SameAs(res2));

                Assert.That(res1.Orders, Is.Empty);

                Assert.That(res2.Orders, Has.Count.EqualTo(1));
                Assert.That(res2.Orders.First(), Is.SameAs(order));
            });
        }

        [Test]
        public void ChangeReservation_With_Null_Should_Throw()
        {
            var res = CreateReservation("A");
            var order = new Order(res);

            Assert.Throws<ArgumentNullException>(() => order.ChangeReservation(null!));
        }

        [Test]
        public void ChangeReservation_To_Same_Reservation_Does_Nothing()
        {
            var res = CreateReservation("A");
            var order = new Order(res);

            order.ChangeReservation(res);

            Assert.Multiple(() =>
            {
                Assert.That(order.Reservation, Is.SameAs(res));
                Assert.That(res.Orders.Count, Is.EqualTo(1));
            });
        }

        // ------------------------------------------------------------
        // REMOVE ORDER
        // ------------------------------------------------------------

        [Test]
        public void Remove_Removes_Order_From_Reservation_And_Extent()
        {
            var res = CreateReservation("A");
            var order = new Order(res);

            order.Remove();

            Assert.Multiple(() =>
            {
                Assert.That(res.Orders, Is.Empty);
                Assert.That(Order.Extent, Is.Empty);
            });
        }

        // ------------------------------------------------------------
        // ADDING ITEMS (DishInOrder)
        // ------------------------------------------------------------

        [Test]
        public void AddItemToOrder_Adds_DishInOrder_And_Updates_OrderSum()
        {
            var res = CreateReservation("A");
            var order = new Order(res);

            var d1 = CreateDish("Miso Soup", 10m, DishType.Starter);
            var d2 = CreateDish("Green Tea", 5m, DishType.Drink);

            order.AddItemToOrder(d1, quantity: 1);
            order.AddItemToOrder(d2, quantity: 1);

            Assert.Multiple(() =>
            {
                Assert.That(order.DishInOrderItems.Count, Is.EqualTo(2));
                Assert.That(order.DishInOrderItems.Select(i => i.Dish), Does.Contain(d1));
                Assert.That(order.DishInOrderItems.Select(i => i.Dish), Does.Contain(d2));

                Assert.That(order.OrderSum, Is.EqualTo(15m));
            });
        }

        [Test]
        public void AddItemToOrder_Null_Dish_Should_Throw()
        {
            var res = CreateReservation("A");
            var order = new Order(res);

            Assert.Throws<ArgumentNullException>(() =>
                order.AddItemToOrder(null!, quantity: 1));
        }

        // ------------------------------------------------------------
        // REMOVING ITEMS (DishInOrder)
        // ------------------------------------------------------------

        [Test]
        public void RemoveItemFromOrder_Removes_DishInOrder_And_Updates_OrderSum()
        {
            var res = CreateReservation("A");
            var order = new Order(res);

            var d1 = CreateDish("Miso Soup", 10m, DishType.Starter);
            var d2 = CreateDish("Green Tea", 5m, DishType.Drink);

            var i1 = order.AddItemToOrder(d1, 1);
            var i2 = order.AddItemToOrder(d2, 1);

            order.RemoveItemFromOrder(i1);

            Assert.Multiple(() =>
            {
                Assert.That(order.DishInOrderItems.Count, Is.EqualTo(1));
                Assert.That(order.DishInOrderItems.First(), Is.SameAs(i2));

                Assert.That(order.OrderSum, Is.EqualTo(5m));
            });
        }

        [Test]
        public void RemoveItemFromOrder_Null_Should_Throw()
        {
            var res = CreateReservation("A");
            var order = new Order(res);

            Assert.Throws<ArgumentNullException>(() =>
                order.RemoveItemFromOrder(null!));
        }
    }
}
