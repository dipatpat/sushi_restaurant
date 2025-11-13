using SushiRestaurant;


namespace sushi_restaurant_tests
{
    [TestFixture]
    public class ReservationTests
    {
        [Test]
        public void Should_Calculate_EndDateTime_As_Start_Plus_3_Hours()
        {
            var start = new DateTime(2025, 11, 12, 18, 0, 0);
            var reservation = new Reservation { StartDateTime = start };

            var expectedEnd = start.AddHours(Reservation.DurationHours);

            Assert.That(reservation.EndDateTime, Is.EqualTo(expectedEnd));
        }

        [Test]
        public void Should_Allow_Valid_ReviewScore()
        {
            var reservation = new Reservation();
            reservation.ReviewScore = 4;
            Assert.That(4, Is.EqualTo(reservation.ReviewScore));
        }

        [Test]
        public void Should_Reject_Invalid_ReviewScore_Less_Than_0()
        {
            var reservation = new Reservation();

            Assert.Throws<ArgumentOutOfRangeException>(
                () => reservation.ReviewScore = -2,
                "Setting a review score < 0 should throw an error"
            );
        }

        [Test]
        public void Should_Reject_Invalid_ReviewScore_Greater_Than_5()
        {
            var reservation = new Reservation();

            Assert.Throws<ArgumentOutOfRangeException>(
                () => reservation.ReviewScore = 10,
                "Setting a review score > 5 should throw an error"
            );
        }

        [Test]
        public void Should_Allow_Valid_NumberOfGuests()
        {
            var reservation = new Reservation();
            reservation.NumberOfGuests = 6;

            Assert.That(6, Is.EqualTo(reservation.NumberOfGuests));
        }

        [Test]
        public void Should_Reject_NumberOfGuests_Less_Than_1()
        {
            var reservation = new Reservation();

            Assert.Throws<ArgumentOutOfRangeException>(
                () => reservation.NumberOfGuests = 0,
                "Guests < 1 should trigger exception"
            );
        }

        [Test]
        public void Should_Reject_NumberOfGuests_Greater_Than_10()
        {
            var reservation = new Reservation();

            Assert.Throws<ArgumentOutOfRangeException>(
                () => reservation.NumberOfGuests = 15,
                "Guests > 10 should trigger exception"
            );
        }

        [Test]
        public void Should_Default_BonusPoints_To_Zero()
        {
            var reservation = new Reservation();

            Assert.That(0, Is.EqualTo(reservation.BonusPoints));
        }

        [Test]
        public void Should_Store_Comment_And_Payment_Status()
        {
            var reservation = new Reservation
            {
                Comment = "Perfect dinner experience!",
                IsPaid = true
            };
            Assert.That("Perfect dinner experience!", Is.EqualTo(reservation.Comment));
            Assert.That(reservation.IsPaid, Is.True);
        }

        [Test]
        public void Should_Set_And_Get_TotalCost()
        {
            var reservation = new Reservation();
            reservation.TotalCost = 150.75m;

            Assert.That(150.75m, Is.EqualTo(reservation.TotalCost));
        }
    }
}

    

    
