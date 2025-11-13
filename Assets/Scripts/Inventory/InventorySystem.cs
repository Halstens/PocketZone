using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    public int inventorySize = 4;
    public List<InventoryItem> inventory = new List<InventoryItem>();
    public System.Action OnInventoryChanged;
    
    void Start()
    {
        for (int i = 0; i < inventorySize; i++)
        {
            inventory.Add(null);
        }
        
        OnInventoryChanged?.Invoke();
    }
    
    public bool AddItem(ItemData itemData, int amount = 1)
    {
        if (itemData == null) return false;
        
        if (itemData.maxStackSize > 1) 
        {
            for (int i = 0; i < inventory.Count; i++)
            {
                if (inventory[i] != null && 
                    inventory[i].data == itemData && 
                    inventory[i].quantity < itemData.maxStackSize)
                {
                    
                    inventory[i].quantity += amount;
                    OnInventoryChanged?.Invoke();
                    return true;
                }
            }
        }
        
        for (int i = 0; i < inventory.Count; i++)
        {
            if (inventory[i] == null)
            {
                inventory[i] = new InventoryItem(itemData, amount);
                OnInventoryChanged?.Invoke();
                return true;
            }
        }
        
        return false;
    }
    
    public void RemoveItem(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= inventory.Count) return;
        
        if (inventory[slotIndex] != null)
        {
            inventory[slotIndex] = null;
            OnInventoryChanged?.Invoke(); 
        }
    }
}