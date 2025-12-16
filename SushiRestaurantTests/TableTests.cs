using NUnit.Framework;
using SushiRestaurant;
using System;
using System.Linq;

namespace sushi_restaurant_tests
{
    [TestFixture]
    public class TableTests
    {
        private Guest CreateGuest(string f = "Anna", string l = "Nowak")
        {
            return new Guest(f, l);
        }

        private Table CreateTable(int num = 1, int cap = 4)
        {
            return new Table(num, cap);
        }

        private Reservation CreateReservation(Table table)
        {
            return new Reservation(
                DateTime.Now.AddHours(2),
                numberOfGuests: 2,
                guest: CreateGuest(),
                table: table);
        }
        
        [Test]
        public void Constructor_Sets_TableNumber_And_Capacity()
        {
            var t = new Table(5, 10);

            Assert.That(t.TableNumber, Is.EqualTo(5));
            Assert.That(t.Capacity, Is.EqualTo(10));
        }
        
        [Test]
        public void Add_Reservation_Through_Reservation_Constructor()
        {
            var t = CreateTable();
            var r = CreateReservation(t);

            Assert.That(t.Reservations.Contains(r), Is.True);
        }
        
        [Test]
        public void Cannot_Add_Two_Reservations_At_Same_Time()
        {
            var t = CreateTable();
            var r1 = new Reservation(
                new DateTime(2026, 1, 16, 20, 30, 0),
                2,
                CreateGuest(),
                t);
            
            Assert.Throws<InvalidOperationException>(() =>
            {
                var r2 = new Reservation(
                    new DateTime(2026, 1, 16, 20, 30, 0),
                    3,
                    CreateGuest(),
                    t);
            });
        }

        
        [Test]
        public void Table_Allows_Different_Reservations_At_Different_Times()
        {
            var t = CreateTable();

            var r1 = new Reservation(
                new DateTime(2026, 1, 16, 20, 30, 0),
                2,
                CreateGuest(),
                t
            );

            var r2 = new Reservation(
                new DateTime(2026, 1, 16, 14, 30, 0),
                3,
                CreateGuest(),
                t
            );

            Assert.That(t.Reservations.Count, Is.EqualTo(2));
        }
        
        [Test]
        public void Reservation_Can_Change_Table()
        {
            var t1 = CreateTable(1, 4);
            var t2 = CreateTable(2, 4);

            var r = CreateReservation(t1);

            r.ChangeTable(t2);

            Assert.That(r.Table, Is.EqualTo(t2));
            Assert.That(t1.Reservations.Contains(r), Is.False);
            Assert.That(t2.Reservations.Contains(r), Is.True);
        }
        
        [Test]
        public void Removing_Reservation_Is_Done_Through_ChangeTable()
        {
            var t1 = CreateTable();
            var t2 = CreateTable(2, 4);

            var r = CreateReservation(t1);

            r.ChangeTable(t2);

            Assert.That(t1.Reservations.Contains(r), Is.False);
            Assert.That(t2.Reservations.Contains(r), Is.True);
        }
    }
}
