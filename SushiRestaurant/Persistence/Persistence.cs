using System;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace SushiRestaurant;

public static class Persistence
{
    private static readonly JsonSerializerOptions Options = new()
    {
        WriteIndented = true
    };

    public static void SaveAll(string path = "sushi.json")
    {
        var dto = new SushiDto
        {
            // Core
            Guests       = Guest.Extent.ToList(),
            Reservations = Reservation.Extent.ToList(),

            // Employees – concrete variants
            FullTimeWaiters  = FullTimeWaiter.Extent.ToList(),
            PartTimeWaiters  = PartTimeWaiter.Extent.ToList(),
            FullTimeManagers = FullTimeManager.Extent.ToList(),
            PartTimeManagers = PartTimeManager.Extent.ToList(),
            FullTimeCooks    = FullTimeCook.Extent.ToList(),
            PartTimeCooks    = PartTimeCook.Extent.ToList(),
            FullTimeCleaners = FullTimeCleaner.Extent.ToList(),
            PartTimeCleaners = PartTimeCleaner.Extent.ToList()
        };

        var json = JsonSerializer.Serialize(dto, Options);
        File.WriteAllText(path, json);
    }

    public static bool LoadAll(string path = "sushi.json")
    {
        try
        {
            if (!File.Exists(path))
            {
                ClearAllExtents();
                return false;
            }

            var json = File.ReadAllText(path);
            var dto = JsonSerializer.Deserialize<SushiDto>(json, Options) ?? new SushiDto();

            Guest.SetExtent(dto.Guests);
            Reservation.SetExtent(dto.Reservations);

            FullTimeWaiter.SetExtent(dto.FullTimeWaiters);
            PartTimeWaiter.SetExtent(dto.PartTimeWaiters);

            FullTimeManager.SetExtent(dto.FullTimeManagers);
            PartTimeManager.SetExtent(dto.PartTimeManagers);

            FullTimeCook.SetExtent(dto.FullTimeCooks);
            PartTimeCook.SetExtent(dto.PartTimeCooks);

            FullTimeCleaner.SetExtent(dto.FullTimeCleaners);
            PartTimeCleaner.SetExtent(dto.PartTimeCleaners);

            return true;
        }
        catch
        {
            ClearAllExtents();
            return false;
        }
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
