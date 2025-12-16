using System.Text.Json;

namespace SushiRestaurant;

public static class Persistence
{
    private static readonly JsonSerializerOptions Options = new()
    {
        WriteIndented = true,
        PropertyNameCaseInsensitive = true
    };

    public static void SaveAll(string path = "sushi.json")
    {
        var dto = new SushiDto
        {
            Guests       = Guest.Extent.ToList(),
            Reservations = Reservation.Extent.ToList(),

            Orders       = Order.Extent.ToList(),          
            DishInOrders = DishInOrder.Extent.ToList(),    

            // FLATTENING CHANGE: We now save the single Employee extent
            Employees    = Employee.Extent.ToList(),

            Dishes      = Dish.Extent.ToList(),           
            Ingredients = Ingredient.Extent.ToList(),      
            Inventory   = Inventory.Extent.ToList()        
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
            if (string.IsNullOrWhiteSpace(json))
            {
                ClearAllExtents();
                return false;
            }

            var dto = JsonSerializer.Deserialize<SushiDto>(json, Options) ?? new SushiDto();

            ClearAllExtents();

            Guest.SetExtent(dto.Guests);
            Reservation.SetExtent(dto.Reservations);

            Order.SetExtent(dto.Orders);                 
            DishInOrder.SetExtent(dto.DishInOrders);    

            // FLATTENING CHANGE: Load the single Employee list
            Employee.SetExtent(dto.Employees);

            Dish.SetExtent(dto.Dishes);                  
            Ingredient.SetExtent(dto.Ingredients);       
            Inventory.SetExtent(dto.Inventory);          

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

        Order.ClearExtent();            
        DishInOrder.ClearExtent();      

        // FLATTENING CHANGE: Clear the single Employee extent
        Employee.ClearExtent();

        Dish.ClearExtent();             
        Ingredient.ClearExtent();       
        Inventory.ClearExtent();        
    }
}