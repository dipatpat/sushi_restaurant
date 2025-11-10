namespace SushiRestaurant;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

public class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("--- Testing Employee Classes (Multi-Aspect Inheritance) ---");

        // 1. Create a Full-Time Manager (Inherits Manager, Implements IFullTimeAspect)
        var ftManager = new FullTimeManager
        {
            FirstName = "Alice",
            LastName = "Smith",
            BaseSalary = 75000m,
            SeniorityLevel = SeniorityLevel.Senior, // Enum
            VacationDays = 25,                      // FullTime Aspect
            IsOnSickLeave = false,                  // FullTime Aspect
            Address = new Address
            {
                StreetName = "Main St",
                StreetNumber = "101",
                CityName = "Metropolis"
            }
        };

        Console.WriteLine($"\n**Employee 1: {ftManager.FirstName} {ftManager.LastName} ({ftManager.SeniorityLevel} Manager)**");
        Console.WriteLine($"Base Salary: {ftManager.BaseSalary:C}");
        Console.WriteLine($"Derived Salary: {ftManager.Salary:C} (Base * 1.5 for Senior)");
        Console.WriteLine($"Vacation Days (FT): {ftManager.VacationDays}");
        Console.WriteLine($"Lives at: {ftManager.Address.StreetName} {ftManager.Address.StreetNumber}");

        Console.WriteLine("-----------------------------------------------------------------");

        // 2. Create a Part-Time Waiter (Inherits Waiter, Implements IPartTimeAspect)
        var ptWaiter = new PartTimeWaiter
        {
            FirstName = "Bob",
            LastName = "Johnson",
            BaseSalary = 15000m,
            Tips = 5000m,
            HoursInContract = 20.5, // PartTime Aspect
        };
        ptWaiter.SpokenLanguages.AddRange(new List<string> { "English", "Spanish" }); // Multi-Value Attribute

        Console.WriteLine($"\n**Employee 2: {ptWaiter.FirstName} {ptWaiter.LastName} (Part-Time Waiter)**");
        Console.WriteLine($"Base Salary: {ptWaiter.BaseSalary:C}");
        Console.WriteLine($"Tips: {ptWaiter.Tips:C}");
        Console.WriteLine($"Derived Salary: {ptWaiter.Salary:C} (Base + Tips)");
        Console.WriteLine($"Contract Hours (PT): {ptWaiter.HoursInContract}");
        Console.WriteLine($"Languages: {string.Join(", ", ptWaiter.SpokenLanguages)}");

        Console.WriteLine("-----------------------------------------------------------------");

        Console.WriteLine("\n--- Testing Guest and Reservation Classes ---");

        // 3. Create a Guest
        var guest = new Guest
        {
            FirstName = "Charlie",
            LastName = "Brown",
            Nickname = "Chuck" // Nullable property used
        };
        Console.WriteLine($"\n**Guest: {guest.FirstName} (Nickname: {guest.Nickname})**");
        
        // 4. Create a Reservation (Testing Derived Attributes and Constraints)
        var reservation = new Reservation
        {
            StartDateTime = DateTime.Now.AddDays(7).Date.AddHours(19),
            TotalCost = 120.50m,
            IsPaid = true,
            Comment = null, // Nullable property unused
        };
        
        try
        {
            // Set constrained attribute (valid value)
            reservation.NumberOfGuests = 4; // Must be 1 to 10
            
            // Set nullable constrained attribute (valid value)
            reservation.ReviewScore = 5; // Must be 0 to 5
            
            Console.WriteLine($"\n**Reservation Details**");
            Console.WriteLine($"Start Time: {reservation.StartDateTime:g}");
            Console.WriteLine($"Derived End Time (Static +{Reservation.DurationHours}h): {reservation.EndDateTime:g}");
            Console.WriteLine($"Guests: {reservation.NumberOfGuests}");
            Console.WriteLine($"Score: {reservation.ReviewScore}/5");
            Console.WriteLine($"Bonus Points (Default=0): {reservation.BonusPoints}");

        }
        catch (ArgumentOutOfRangeException ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
        
        // 5. Test constraint violation (too many guests)
        try
        {
            Console.WriteLine("\n--- Testing Constraint Violation (Guests > 10) ---");
            var invalidReservation = new Reservation();
            invalidReservation.NumberOfGuests = 12; // This should throw an exception
        }
        catch (ArgumentOutOfRangeException ex)
        {
            Console.WriteLine($"Caught expected exception: {ex.Message.Split('\n')[0]}");
        }
    }
}