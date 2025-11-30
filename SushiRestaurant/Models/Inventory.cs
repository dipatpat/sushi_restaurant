using System;
using System.Collections.Generic;
using System.Linq;
namespace SushiRestaurant;

public class Inventory
{
    public string ProductName { get; private set; }
    public int QuantityLeft { get; set; }
    public DateTime PurchaseDate { get; } 
    public DateTime? ExpirationDate { get; set; }

    private static List<Inventory> _allInventory = new List<Inventory>();

    public Inventory(string productName, int quantityLeft, DateTime purchaseDate, DateTime? expirationDate = null)
    {
        ProductName = productName;
        QuantityLeft = quantityLeft;
        PurchaseDate = purchaseDate;
        ExpirationDate = expirationDate;
    }

    public static void ListInventory()
    {
        Console.WriteLine("\n--- Current Inventory Status ---");
        if (!_allInventory.Any())
        {
            Console.WriteLine("Inventory is empty.");
            return;
        }

        foreach (var item in _allInventory.OrderBy(i => i.PurchaseDate))
        {
            string expiry = item.ExpirationDate.HasValue
                ? $"Expires: {item.ExpirationDate.Value:yyyy-MM-dd}"
                : "No Expiration Date";
            
            Console.WriteLine($"- {item.ProductName} (Purchased: {item.PurchaseDate:yyyy-MM-dd}), Quantity Left: {item.QuantityLeft}, {expiry}");
        }
        Console.WriteLine("------------------------------");
    }

    public static void AddProduct(Inventory item)
    {
        _allInventory.Add(item);
        Console.WriteLine($"Added new batch of {item.ProductName} purchased on {item.PurchaseDate:yyyy-MM-dd} with initial quantity {item.QuantityLeft}.");
    }

    public static void ClearAllInventory()
    {
        _allInventory.Clear();
    }
    
    public static IReadOnlyList<Inventory> ListAllInventory()
    {
        return _allInventory.AsReadOnly();
    }
}