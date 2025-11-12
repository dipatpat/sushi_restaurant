namespace SushiRestaurant;

using System;
using System.Linq;

public class Program
{
    public static void Main(string[] args)
    {
        var loaded = Persistence.LoadAll();
        Console.WriteLine(loaded
            ? "Loaded existing data from sushi.json."
            : "No file found or load failed. Starting with empty extents.");

        PrintCounts("After initial load");

        if (!loaded)
        {
           
            var g1 = new Guest("Charlie", "Brown", "Chuck");
            var g2 = new Guest("Lucy", "Van Pelt");

            var res = new Reservation(DateTime.Now.AddDays(7).Date.AddHours(19), 4, 120.50m)
            {
                IsPaid = true,
                ReviewScore = 5
            };

            var addr = new Address("Main St", "101", "00-001", "Metropolis");
            var ftMgr = new FullTimeManager("Alice", "Smith", addr, "PL00112233", "555-111-222", 75000m,
                                            SeniorityLevel.Senior, vacationDays: 25, isOnSickLeave: false);

            var ptWaiter = new PartTimeWaiter("Bob", "Johnson", addr, "PL99887766", "555-333-444", 15000m,
                                              hoursInContract: 20.5, tips: 5000m);
            ptWaiter.AddLanguage("English");
            ptWaiter.AddLanguage("Spanish");

            var ftCook = new FullTimeCook("Mina", "Tanaka", addr, "PL11223344", "555-777-888", 42000m,
                                          bonus: 3000m, specialization: "Sushi", vacationDays: 12);

            var ptCleaner = new PartTimeCleaner("John", "Doe", addr, "PL55667788", "555-000-111", 22000m,
                                                cleaningShift: "Evening", assignedArea: "Dining Hall",
                                                hoursInContract: 15);

            PrintCounts("After creating sample data");

            Persistence.SaveAll();
            Console.WriteLine("Saved to sushi.json.");
        }

        ClearAllExtents();
        PrintCounts("After manual clear (simulate fresh app)");

        var reloaded = Persistence.LoadAll();
        Console.WriteLine(reloaded ? "Reloaded from sushi.json." : "Reload failed.");
        PrintCounts("After reload");

        var guest = Guest.FindByName("Charlie", "Brown");
        Console.WriteLine(guest != null
            ? $"Found guest after reload: {guest}"
            : "Guest 'Charlie Brown' not found after reload.");

        var anyRes = Reservation.Extent.FirstOrDefault();
        if (anyRes != null)
        {
            Console.WriteLine($"Reservation: starts {anyRes.StartDateTime:g}, ends {anyRes.EndDateTime:g}, guests {anyRes.NumberOfGuests}, paid={anyRes.IsPaid}");
        }
    }

    private static void PrintCounts(string label)
    {
        Console.WriteLine($"\n-- {label} --");
        Console.WriteLine($"Guests: {Guest.Extent.Count}");
        Console.WriteLine($"Reservations: {Reservation.Extent.Count}");
        Console.WriteLine($"FT Waiters: {FullTimeWaiter.Extent.Count} | PT Waiters: {PartTimeWaiter.Extent.Count}");
        Console.WriteLine($"FT Managers: {FullTimeManager.Extent.Count} | PT Managers: {PartTimeManager.Extent.Count}");
        Console.WriteLine($"FT Cooks: {FullTimeCook.Extent.Count} | PT Cooks: {PartTimeCook.Extent.Count}");
        Console.WriteLine($"FT Cleaners: {FullTimeCleaner.Extent.Count} | PT Cleaners: {PartTimeCleaner.Extent.Count}");
    }

    private static void ClearAllExtents()
    {
        Guest.ClearExtent();
        Reservation.ClearExtent();

        FullTimeWaiter.ClearExtent();
        PartTimeWaiter.ClearExtent();

        FullTimeManager.ClearExtent();
        PartTimeManager.ClearExtent();

        FullTimeCook.ClearExtent();
        PartTimeCook.ClearExtent();

        FullTimeCleaner.ClearExtent();
        PartTimeCleaner.ClearExtent();
    }
}
