using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    public int inventorySize = 4;
    public List<InventoryItem> inventory = new List<InventoryItem>();
    
    // 🔥 Добавляем события для уведомления об изменениях
    public System.Action OnInventoryChanged;
    
    void Start()
    {
        for (int i = 0; i < inventorySize; i++)
        {
            inventory.Add(null);
        }
        
        // 🔥 Уведомляем о готовности инвентаря
        OnInventoryChanged?.Invoke();
    }
    
    public bool AddItem(ItemData itemData, int amount = 1)
    {
        if (itemData == null) return false;

        Debug.Log($"🔄 Пытаемся добавить: {itemData.itemName}");

        // 🔥 ПЕРВОЕ: Пытаемся добать к существующему стаку
        if (itemData.maxStackSize > 1) // Проверяем, можно ли вообще стакать
        {
            for (int i = 0; i < inventory.Count; i++)
            {
                // Проверяем что слот не пустой, тот же предмет и есть место в стаке
                if (inventory[i] != null && 
                    inventory[i].data == itemData && 
                    inventory[i].quantity < itemData.maxStackSize)
                {
                    // Добавляем к существующему стаку
                    inventory[i].quantity += amount;
                    Debug.Log($"✅ Добавлено к существующему стаку в слот {i}. Теперь: {inventory[i].quantity}");
                    OnInventoryChanged?.Invoke();
                    return true;
                }
            }
        }

        // 🔥 ВТОРОЕ: Ищем пустой слот
        for (int i = 0; i < inventory.Count; i++)
        {
            if (inventory[i] == null)
            {
                inventory[i] = new InventoryItem(itemData, amount);
                Debug.Log($"✅ Предмет добавлен в пустой слот {i}");
                OnInventoryChanged?.Invoke();
                return true;
            }
        }
    
        Debug.Log("❌ Инвентарь полон!");
        return false;
    }
    
    public void RemoveItem(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= inventory.Count) return;
        
        if (inventory[slotIndex] != null)
        {
            inventory[slotIndex] = null;
            OnInventoryChanged?.Invoke(); // 🔥 Уведомляем об изменении
        }
    }
}