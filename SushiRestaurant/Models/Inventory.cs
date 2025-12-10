using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace SushiRestaurant;

public class Inventory
{
    private static readonly List<Inventory> _extent = new();
    public static IReadOnlyList<Inventory> Extent => _extent.AsReadOnly();

    public static void ClearExtent() => _extent.Clear();

    internal static void SetExtent(List<Inventory>? items)
    {
        _extent.Clear();
        if (items is { Count: > 0 })
            _extent.AddRange(items);
    }

    private Ingredient? _ingredient;
    
    [JsonIgnore]
    public Ingredient? Ingredient => _ingredient;

    public void SetIngredient(Ingredient ingredient)
    {
        if (ingredient == null) throw new ArgumentNullException(nameof(ingredient));
        if (ReferenceEquals(_ingredient, ingredient)) return;

        if (_ingredient != null)
        {
            throw new InvalidOperationException("Ingredient is already set. Use ChangeIngredient to switch.");
        }

        _ingredient = ingredient;
        ingredient.InternalAddInventoryBatch(this);
    }

    public void ChangeIngredient(Ingredient newIngredient)
    {
        if (newIngredient == null) throw new ArgumentNullException(nameof(newIngredient));
        if (ReferenceEquals(_ingredient, newIngredient)) return;

        var oldIngredient = _ingredient;
        
        if (oldIngredient != null)
        {
            oldIngredient.InternalRemoveInventoryBatch(this);
        }

        _ingredient = newIngredient;
        newIngredient.InternalAddInventoryBatch(this);
    }
    
    public void RemoveIngredient()
    {
        if (_ingredient == null) return;
        
        var oldIngredient = _ingredient;
        _ingredient = null;
        oldIngredient.InternalRemoveInventoryBatch(this);
    }

    internal void InternalSetIngredientFromIngredient(Ingredient ingredient)
    {
        if (ingredient == null) throw new ArgumentNullException(nameof(ingredient));
        if (ReferenceEquals(_ingredient, ingredient)) return;
        
        _ingredient = ingredient;
    }

    internal void InternalRemoveIngredientFromIngredient(Ingredient ingredient)
    {
         if (ReferenceEquals(_ingredient, ingredient))
         {
             _ingredient = null;
         }
    }

    private string _productName = default!;
    private int _quantityLeft;
    private DateTime _purchaseDate;
    private DateTime? _expirationDate;

    public string ProductName
    {
        get => _productName;
        private set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Product name is required.", nameof(ProductName));
            _productName = value.Trim();
        }
    }

    public int QuantityLeft
    {
        get => _quantityLeft;
        set
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException(nameof(QuantityLeft), "Quantity left cannot be negative.");
            _quantityLeft = value;
        }
    }

    public DateTime PurchaseDate
    {
        get => _purchaseDate;
        private set
        {
            if (value > DateTime.Today)
                throw new ArgumentOutOfRangeException(nameof(PurchaseDate), "Purchase date cannot be in the future.");
            _purchaseDate = value;
        }
    }
    
    public DateTime? ExpirationDate
    {
        get => _expirationDate;
        set
        {
            if (value.HasValue && value.Value < PurchaseDate)
            {
                throw new ArgumentOutOfRangeException(nameof(ExpirationDate), "Expiration date cannot be before the purchase date.");
            }
            _expirationDate = value;
        }
    }

    private static List<Inventory> _allInventory = new List<Inventory>();

    public Inventory(string productName, int quantityLeft, DateTime purchaseDate, DateTime? expirationDate = null)
    {
        ProductName = productName;
        QuantityLeft = quantityLeft;
        PurchaseDate = purchaseDate;
        ExpirationDate = expirationDate;

        _extent.Add(this);
    }

    public Inventory() {}

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
            
            // Show Ingredient link if it exists
            string linkedInfo = item.Ingredient != null ? $" [Linked to: {item.Ingredient.Name}]" : "";

            Console.WriteLine($"- {item.ProductName}{linkedInfo} (Purchased: {item.PurchaseDate:yyyy-MM-dd}), Quantity Left: {item.QuantityLeft}, {expiry}");
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