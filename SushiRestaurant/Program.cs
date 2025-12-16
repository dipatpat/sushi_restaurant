using SushiRestaurant;
using System;
using System.Linq;

public class Program
{
    public static void Main(string[] args)
    {
//         var loaded = Persistence.LoadAll();
//         Console.WriteLine(loaded
//             ? "Loaded existing data from sushi.json."
//             : "No file found or load failed. Starting with empty extents.");

//         PrintCounts("After initial load");

//         if (!loaded)
//         {
//             // --- SAMPLE DATA (guests, staff, etc.) ---
//             var g1 = new Guest("Charlie", "Brown", "Chuck");
//             var g2 = new Guest("Lucy", "Van Pelt");
//             var t1 = new Table(1, 4);
//             var t2 = new Table(2, 6);
//             var t3 = new Table(3, 2);

//             var res = new Reservation(
//                 DateTime.Now.AddDays(7).Date.AddHours(19),
//                 numberOfGuests: 4,
//                 guest: g1,
//                 table: t1)
//             {
//                 IsPaid = true,
//                 ReviewScore = 5
//             };

//             var order = new Order(res);

//             // ===== HISTORY ASSOCIATION DEMO (DishInOrder) =====
//             Console.WriteLine("\n=== DishInOrder (bag/history association) DEMO ===");

//             var dish1 = new Dish("Sushi Set", 120.50m, DishType.Sushi);
//             var dish2 = new Dish("Green Tea", 15m, DishType.Drink);

//             var item1 = order.AddItemToOrder(dish1, quantity: 1); // active
//             var item2 = order.AddItemToOrder(dish2, quantity: 1); // active

//             Console.WriteLine($"Initial OrderSum (both items active): {order.OrderSum}");
//             Console.WriteLine($"Reservation TotalCost: {res.TotalCost}");

//             Console.WriteLine($"Active items in order: {order.ActiveDishInOrderItems.Count}");
//             Console.WriteLine($"All items in order (history): {order.AllDishInOrderItems.Count}");
//             Console.WriteLine($"DishInOrder.Extent count: {DishInOrder.Extent.Count}");

//             Console.WriteLine("\nDeactivating first DishInOrder (history-style remove)...");
//             item1.Deactivate();

//             Console.WriteLine($"OrderSum after deactivation: {order.OrderSum}");
//             Console.WriteLine($"Reservation TotalCost after deactivation: {res.TotalCost}");

//             Console.WriteLine($"Active items in order: {order.ActiveDishInOrderItems.Count}");
//             Console.WriteLine($"All items in order (history): {order.AllDishInOrderItems.Count}");
//             Console.WriteLine($"DishInOrder.Extent count (history kept): {DishInOrder.Extent.Count}");

//             Console.WriteLine($"item1.IsActive = {item1.IsActive}, TimeRemoved = {item1.TimeRemoved}");
//             Console.WriteLine($"item2.IsActive = {item2.IsActive}, TimeRemoved = {item2.TimeRemoved}");

//             var addr = new Address("Main St", "101", "00-001", "Metropolis");

//             var ftMgr = new FullTimeManager("Alice", "Smith", addr, "PL00112233", "555-111-222",
//                 75000m, SeniorityLevel.Senior, vacationDays: 25, isOnSickLeave: false);

//             var ptWaiter = new PartTimeWaiter("Bob", "Johnson", addr, "PL99887766", "555-333-444",
//                 15000m, hoursInContract: 20.5, tips: 5000m);
//             ptWaiter.AddLanguage("English");
//             ptWaiter.AddLanguage("Spanish");

//             var ftCook = new FullTimeCook("Mina", "Tanaka", addr, "PL11223344", "555-777-888",
//                 42000m, bonus: 3000m, specialization: "Sushi", vacationDays: 12);

//             var ptCleaner = new PartTimeCleaner("John", "Doe", addr, "PL55667788", "555-000-111",
//                 22000m, cleaningShift: "Evening", assignedArea: "Dining Hall",
//                 hoursInContract: 15);

//             PrintCounts("After creating sample data (with DishInOrder history demo)");

//             Persistence.SaveAll();
//             Console.WriteLine("Saved to sushi.json.");
//         }

//         ClearAllExtents();
//         PrintCounts("After manual clear");

//         var reloaded = Persistence.LoadAll();
//         Console.WriteLine(reloaded ? "Reloaded from sushi.json." : "Reload failed.");
//         PrintCounts("After reload");

//         var guest = Guest.FindByName("Charlie", "Brown");
//         Console.WriteLine(guest != null
//             ? $"Found guest after reload: {guest}"
//             : "Guest 'Charlie Brown' not found after reload.");

//         var anyRes = Reservation.Extent.FirstOrDefault();
//         if (anyRes != null)
//         {
//             Console.WriteLine(
//                 $"Reservation: starts {anyRes.StartDateTime:g}, ends {anyRes.EndDateTime:g}, " +
//                 $"guests {anyRes.NumberOfGuests}, paid={anyRes.IsPaid}, totalCost={anyRes.TotalCost}");
//         }

//         if (DishInOrder.Extent.Any())
//         {
//             Console.WriteLine();
//             Console.WriteLine("=== Reloaded DishInOrder history from file (bag/history) ===");

//             var allItems = DishInOrder.Extent.ToList();
//             var activeItems = allItems.Where(i => i.IsActive).ToList();
//             var removedItems = allItems.Where(i => !i.IsActive).ToList();

//             decimal activeTotal = activeItems.Sum(i => i.Dish.Price * i.Quantity);
//             decimal removedTotal = removedItems.Sum(i => i.Dish.Price * i.Quantity);
//             decimal everTotal = allItems.Sum(i => i.Dish.Price * i.Quantity);

//             Console.WriteLine($"Total DishInOrder entries (history): {allItems.Count}");
//             Console.WriteLine($"  Active entries:   {activeItems.Count}");
//             Console.WriteLine($"  Removed entries:  {removedItems.Count}");
//             Console.WriteLine($"  Sum of ALL items ever ordered: {everTotal}");
//             Console.WriteLine($"  Sum of ACTIVE items:           {activeTotal}");
//             Console.WriteLine($"  Sum of REMOVED items:          {removedTotal}");

//             Console.WriteLine();
//             Console.WriteLine("All DishInOrder history entries:");
//             foreach (var item in allItems)
//             {
//                 var statusLabel = item.IsActive ? "[ACTIVE ]" : "[REMOVED]";
//                 var removedInfo = item.TimeRemoved?.ToString("g") ?? "-";

//                 Console.WriteLine(
//                     $"{statusLabel} Dish: '{item.Dish.DishName}', " +
//                     $"qty={item.Quantity}, " +
//                     $"price={item.Dish.Price}, " +
//                     $"lineValue={item.Dish.Price * item.Quantity}, " +
//                     $"ordered={item.TimeOrdered:g}, " +
//                     $"removed={removedInfo}");
//             }


//         }
//     }

//     private static void PrintCounts(string label)
//     {
//         Console.WriteLine($"\n-- {label} --");
//         Console.WriteLine($"Guests: {Guest.Extent.Count}");
//         Console.WriteLine($"Reservations: {Reservation.Extent.Count}");
//         Console.WriteLine($"Orders: {Order.Extent.Count}");
//         Console.WriteLine($"Dishes: {Dish.Extent.Count}");
//         Console.WriteLine($"DishInOrder (history entries): {DishInOrder.Extent.Count}");
//         Console.WriteLine($"FT Waiters: {FullTimeWaiter.Extent.Count} | PT Waiters: {PartTimeWaiter.Extent.Count}");
//         Console.WriteLine($"FT Managers: {FullTimeManager.Extent.Count} | PT Managers: {PartTimeManager.Extent.Count}");
//         Console.WriteLine($"FT Cooks: {FullTimeCook.Extent.Count} | PT Cooks: {PartTimeCook.Extent.Count}");
//         Console.WriteLine($"FT Cleaners: {FullTimeCleaner.Extent.Count} | PT Cleaners: {PartTimeCleaner.Extent.Count}");
//     }

//     private static void ClearAllExtents()
//     {
//         Guest.ClearExtent();
//         Reservation.ClearExtent();
//         Order.ClearExtent();
//         Dish.ClearExtent();
//         DishInOrder.ClearExtent();

//         FullTimeWaiter.ClearExtent();
//         PartTimeWaiter.ClearExtent();

//         FullTimeManager.ClearExtent();
//         PartTimeManager.ClearExtent();

//         FullTimeCook.ClearExtent();
//         PartTimeCook.ClearExtent();

//         FullTimeCleaner.ClearExtent();
//         PartTimeCleaner.ClearExtent();
        
//         Table.ClearExtent();
    }
}
