
using SushiRestaurant;


namespace sushi_restaurant_tests
{
    [TestFixture]
    public class ExtentTests
    {
        private Address _defaultAddress = new("Street", "1", "123456", "City");

        [SetUp]
        public void SetUp()
        {
            Employee.ClearExtent();
        }

        [Test]
        public void CreatingObjects_AddsThemToEmployeeExtent()
        {
            // Create 1 Waiter
            new Employee(
                "Alice", "W", _defaultAddress, "1", "1", 3000m,
                EmployeeRole.Waiter, EmploymentType.PartTime, hoursInContract: 20, tips: 50m
            );

            // Create 1 Cleaner
            new Employee(
                "Bob", "C", _defaultAddress, "2", "2", 2500m,
                EmployeeRole.Cleaner, EmploymentType.FullTime, cleaningShift: "Day"
            );

            // Assert Extent contains both
            Assert.That(Employee.Extent, Has.Count.EqualTo(2));

            // Assert filtering works via Discriminator
            var waiters = Employee.Extent.Where(e => e.Role == EmployeeRole.Waiter).ToList();
            Assert.That(waiters, Has.Count.EqualTo(1));
            Assert.That(waiters[0].FirstName, Is.EqualTo("Alice"));

            var cleaners = Employee.Extent.Where(e => e.Role == EmployeeRole.Cleaner).ToList();
            Assert.That(cleaners, Has.Count.EqualTo(1));
            Assert.That(cleaners[0].FirstName, Is.EqualTo("Bob"));
        }
    }
    // [TestFixture]
    // public class ExtentTests
    // {
    //     private static Address CreateTestAddress() =>
    //         new Address("Main St", "10A", "00-001", "Metropolis");

    //     private static void ClearAllExtents()
    //     {
    //         Guest.ClearExtent();
    //         Reservation.ClearExtent();
    //         Employee.ClearExtent();
    //     }

    //     [SetUp]
    //     public void SetUp()
    //     {
    //         ClearAllExtents();
    //     }

    //     [Test]
    //     public void CreatingObjects_AddsThemToCorrectExtents()
    //     {
    //         var addr = CreateTestAddress();

    //         var guest = new Guest("Charlie", "Brown");
    //         var table = new Table(1, 4);
    //         var reservation = new Reservation(DateTime.Today.AddHours(19), 4, guest,table);

    //         var ftManager = new FullTimeManager("Alice", "Smith", addr, "PL001", "555-111-222",
    //                                             7500m, SeniorityLevel.Senior, vacationDays: 25);

    //         var ptWaiter = new PartTimeWaiter("Bob", "Johnson", addr, "PL002", "555-333-444",
    //                                           3000m, hoursInContract: 20.5, tips: 500m);

    //         var ftCook = new FullTimeCook("Mina", "Tanaka", addr, "PL003", "555-777-888",
    //                                       4200m, bonus: 300m, specialization: "Sushi");

    //         var ptCleaner = new PartTimeCleaner("John", "Doe", addr, "PL004", "555-000-111",
    //                                             2200m, "Evening", "Dining Hall", hoursInContract: 15);

    //         Assert.That(Guest.Extent, Has.Count.EqualTo(1));
    //         Assert.That(Guest.Extent[0], Is.SameAs(guest));

    //         Assert.That(Reservation.Extent, Has.Count.EqualTo(1));
    //         Assert.That(Reservation.Extent[0], Is.SameAs(reservation));

    //         Assert.That(FullTimeManager.Extent, Has.Count.EqualTo(1));
    //         Assert.That(FullTimeManager.Extent[0], Is.SameAs(ftManager));

    //         Assert.That(PartTimeWaiter.Extent, Has.Count.EqualTo(1));
    //         Assert.That(PartTimeWaiter.Extent[0], Is.SameAs(ptWaiter));

    //         Assert.That(FullTimeCook.Extent, Has.Count.EqualTo(1));
    //         Assert.That(FullTimeCook.Extent[0], Is.SameAs(ftCook));

    //         Assert.That(PartTimeCleaner.Extent, Has.Count.EqualTo(1));
    //         Assert.That(PartTimeCleaner.Extent[0], Is.SameAs(ptCleaner));

    //         Assert.That(FullTimeWaiter.Extent, Is.Empty);
    //         Assert.That(PartTimeManager.Extent, Is.Empty);
    //         Assert.That(PartTimeCook.Extent, Is.Empty);
    //         Assert.That(FullTimeCleaner.Extent, Is.Empty);
    //     }
        
    //     [Test]
    //     public void SaveAll_ThenLoadAll_RestoresExtentsCorrectly()
    //     {
    //         var addr = CreateTestAddress();
    //         var tempFile = Path.GetTempFileName();

    //         try
    //         {
    //             var reservationStart = DateTime.Today.AddDays(7).AddHours(19);

    //             var guest = new Guest("Charlie", "Brown", "Chuck");
    //             var table = new Table(1, 4);
    //             var reservation = new Reservation(reservationStart, 4, guest,table)
    //             {
    //                 IsPaid = true,
    //                 ReviewScore = 5
    //             };
    //             var ftManager = new FullTimeManager("Alice", "Smith", addr, "PL001", "555-111-222",
    //                                                 75000m, SeniorityLevel.Senior, vacationDays: 25);
    //             var ptWaiter = new PartTimeWaiter("Bob", "Johnson", addr, "PL002", "555-333-444",
    //                                               15000m, hoursInContract: 20.5, tips: 5000m);

    //             Persistence.SaveAll(tempFile);
    //             ClearAllExtents();

    //             var loaded = Persistence.LoadAll(tempFile);

    //             Assert.That(loaded, Is.True);

    //             Assert.That(Guest.Extent, Has.Count.EqualTo(1));
    //             Assert.That(Guest.Extent[0].FirstName, Is.EqualTo("Charlie"));
    //             Assert.That(Guest.Extent[0].LastName, Is.EqualTo("Brown"));
    //             Assert.That(Guest.Extent[0].Nickname, Is.EqualTo("Chuck"));

    //             Assert.That(Reservation.Extent, Has.Count.EqualTo(1));
    //             var loadedRes = Reservation.Extent[0];
    //             Assert.That(loadedRes.NumberOfGuests, Is.EqualTo(reservation.NumberOfGuests));
    //             Assert.That(loadedRes.TotalCost, Is.EqualTo(reservation.TotalCost));
    //             Assert.That(loadedRes.IsPaid, Is.EqualTo(reservation.IsPaid));
    //             Assert.That(loadedRes.ReviewScore, Is.EqualTo(reservation.ReviewScore));

    //             Assert.That(FullTimeManager.Extent, Has.Count.EqualTo(1));
    //             Assert.That(FullTimeManager.Extent[0].FirstName, Is.EqualTo("Alice"));

    //             Assert.That(PartTimeWaiter.Extent, Has.Count.EqualTo(1));
    //             Assert.That(PartTimeWaiter.Extent[0].FirstName, Is.EqualTo("Bob"));

    //             Assert.That(FullTimeWaiter.Extent, Is.Empty);
    //             Assert.That(PartTimeManager.Extent, Is.Empty);
    //             Assert.That(FullTimeCook.Extent, Is.Empty);
    //             Assert.That(PartTimeCook.Extent, Is.Empty);
    //             Assert.That(FullTimeCleaner.Extent, Is.Empty);
    //             Assert.That(PartTimeCleaner.Extent, Is.Empty);
    //         }
    //         finally
    //         {
    //             if (File.Exists(tempFile))
    //                 File.Delete(tempFile);
    //         }
    //     }

    //     [Test]
    //     public void ModifyingObject_NotInExtent_DoesNotChangeExtent()
    //     {
    //         var g1 = new Guest("Anna", "Nowak");

    //         var g2 = new Guest();
    //         g2.FirstName = "Ghost";
    //         g2.LastName = "User";

    //         g2.FirstName = "Another";
    //         g2.LastName = "Name";

    //         Assert.That(Guest.Extent, Has.Count.EqualTo(1));
    //         Assert.That(Guest.Extent[0], Is.SameAs(g1));
    //         Assert.That(Guest.Extent, Does.Not.Contain(g2));
    //     }

    //     [Test]
    //     public void Extent_IsReadOnly_ExternalCodeCannotModifyExtent()
    //     {
    //         var g1 = new Guest("Anna", "Nowak");

    //         var extent = Guest.Extent;

    //         Assert.Throws<NotSupportedException>(() =>
    //         {
    //             var list = (System.Collections.IList)extent;
    //             list.Add(g1);   
    //         });

    //         Assert.That(Guest.Extent.Count, Is.EqualTo(1));
    //         Assert.That(Guest.Extent[0], Is.SameAs(g1));
    //     }
    // }
}
