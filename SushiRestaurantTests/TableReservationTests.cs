using NUnit.Framework;
using SushiRestaurant;

namespace sushi_restaurant_tests
{
    [TestFixture]
    public class TableReservationTests
    {
        private Guest CreateGuest()
            => new Guest("Anna", "Nowak");

        private Table CreateTable(int num = 1, int cap = 4)
            => new Table(num, cap);

        [SetUp]
        public void Setup()
        {
            Guest.ClearExtent();
            Reservation.ClearExtent();
        }

        [Test]
        public void Reservation_Should_Store_Table()
        {
            var guest = CreateGuest();
            var table = CreateTable();

            var res = new Reservation(
                DateTime.Now.AddHours(2),
                3,
                guest,
                table);

            Assert.That(res.Table, Is.EqualTo(table));
            Assert.That(table.Reservations, Contains.Item(res));
        }

        [Test]
        public void Table_Cannot_Add_Same_Reservation_Twice()
        {
            var g = CreateGuest();
            var t = CreateTable();
            var r = new Reservation(DateTime.Now.AddHours(2), 2, g, t);

            Assert.Throws<InvalidOperationException>(() =>
                t.AddReservation(r));
        }

        [Test]
        public void ChangeTable_Should_Move_Reservation_Between_Tables()
        {
            var g = CreateGuest();
            var t1 = CreateTable(1, 4);
            var t2 = CreateTable(2, 4);

            var r = new Reservation(DateTime.Now.AddHours(2), 2, g, t1);

            r.ChangeTable(t2);

            Assert.That(r.Table, Is.EqualTo(t2));
            Assert.That(t1.Reservations, Does.Not.Contain(r));
            Assert.That(t2.Reservations, Contains.Item(r));
        }

        [Test]
        public void ChangeTable_To_Same_Table_Does_Nothing()
        {
            var g = CreateGuest();
            var t1 = CreateTable();
            var r = new Reservation(DateTime.Now.AddHours(2), 2, g, t1);

            r.ChangeTable(t1);

            Assert.That(t1.Reservations.Count, Is.EqualTo(1));
        }

        [Test]
        public void Reservation_Constructor_Throws_If_Table_Null()
        {
            var g = CreateGuest();

            Assert.Throws<ArgumentNullException>(() =>
                new Reservation(
                    DateTime.Now.AddHours(2),
                    2,
                    g,
                    null!));
        }
    }
}
