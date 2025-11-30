using System;
using System.Collections.Generic;
using System.Linq;

public class Ingredient
{
    public string Name { get; private set; }
    public int Quantity { get; private set; }

    private Dictionary<DateTime, Inventory> _inventoryBatches = new Dictionary<DateTime, Inventory>();

    public Ingredient(string name, int quantity)
    {
        Name = name;
        Quantity = quantity;
    }

    public IReadOnlyDictionary<DateTime, Inventory> GetInventoryBatches()
    {
        return new Dictionary<DateTime, Inventory>(_inventoryBatches);
    }

    public void AddInventoryBatch(Inventory inventoryItem)
    {
        if (inventoryItem == null || _inventoryBatches.ContainsKey(inventoryItem.PurchaseDate))
        {
            Console.WriteLine($"Error: Cannot add Inventory batch. Either it is null or a batch with purchase date {inventoryItem?.PurchaseDate:yyyy-MM-dd} already exists for {Name}.");
            return;
        }

        _inventoryBatches.Add(inventoryItem.PurchaseDate, inventoryItem);
        Console.WriteLine($"Ingredient '{Name}' linked to Inventory batch purchased on {inventoryItem.PurchaseDate:yyyy-MM-dd}.");
    }

    public Inventory GetInventoryBatch(DateTime purchaseDate)
    {
        if (_inventoryBatches.TryGetValue(purchaseDate, out Inventory inventory))
        {
            return inventory;
        }
        return null;
    }
    
    public void UseQuantityFromBatch(DateTime purchaseDate)
    {
        var inventory = GetInventoryBatch(purchaseDate);
        if (inventory == null)
        {
            Console.WriteLine($"Error: Inventory batch for {Name} with purchase date {purchaseDate:yyyy-MM-dd} not found.");
            return;
        }

        int required = Quantity;
        
        if (inventory.QuantityLeft >= required)
        {
            inventory.QuantityLeft -= required;
            Console.WriteLine($"Used {required} units of {Name} from batch purchased on {purchaseDate:yyyy-MM-dd}. Remaining: {inventory.QuantityLeft}.");
        }
        else
        {
            int quantityToTake = inventory.QuantityLeft;
            int remainingNeeded = required - quantityToTake;

            Console.WriteLine($"\n--- Insufficient Quantity in Batch {purchaseDate:yyyy-MM-dd} ---");
            Console.WriteLine($"Taking available {quantityToTake} units of {Name}. Still need {remainingNeeded} units.");
            
            inventory.QuantityLeft = 0;
            RemoveInventoryBatch(inventory.PurchaseDate);
            HandleInsufficientQuantity(remainingNeeded);
        }
    }

    private void HandleInsufficientQuantity(int remainingNeeded)
    {
        var availableUnusedBatches = Inventory.ListAllInventory()
            .Where(i => i.ProductName == Name)
            .OrderBy(i => i.PurchaseDate)
            .ToList();
        
        Inventory nextBatch = availableUnusedBatches.FirstOrDefault(batch => 
             batch.QuantityLeft > 0
        );

        if (nextBatch == null)
        {
            Console.WriteLine($"\n--> FAILURE: No other Inventory batch of '{Name}' found with sufficient quantity left. The recipe cannot be completed.");
            return;
        }

        Console.WriteLine($"\n--> Switching to new batch: {nextBatch.PurchaseDate:yyyy-MM-dd} (Quantity Left: {nextBatch.QuantityLeft})");

        AddInventoryBatch(nextBatch);
        
        if (nextBatch.QuantityLeft >= remainingNeeded)
        {
            nextBatch.QuantityLeft -= remainingNeeded;
            Console.WriteLine($"Used remaining {remainingNeeded} units from batch {nextBatch.PurchaseDate:yyyy-MM-dd}. Remaining: {nextBatch.QuantityLeft}.");
        }
        else
        {
            int quantityTaken = nextBatch.QuantityLeft;
            nextBatch.QuantityLeft = 0;
            int stillNeeded = remainingNeeded - quantityTaken;
            
            Console.WriteLine($"Used all {quantityTaken} units from batch {nextBatch.PurchaseDate:yyyy-MM-dd}. Still need {stillNeeded} units.");
            HandleInsufficientQuantity(stillNeeded);
        }
    }
    
    public void RemoveInventoryBatch(DateTime purchaseDate)
    {
        if (_inventoryBatches.ContainsKey(purchaseDate))
        {
            _inventoryBatches.Remove(purchaseDate);
            Console.WriteLine($"Removed link for '{Name}' to Inventory batch purchased on {purchaseDate:yyyy-MM-dd}.");
        }
        else
        {
            Console.WriteLine($"Warning: No Inventory batch found with purchase date {purchaseDate:yyyy-MM-dd} to remove.");
        }
    }
}
