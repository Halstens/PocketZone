using UnityEngine;
using System.Collections.Generic;

public class InventoryUI : MonoBehaviour
{
    public List<InventorySlotUI> slots;
    private InventorySystem inventory;
    
    void Start()
    {
        inventory = FindObjectOfType<InventorySystem>();
        if (inventory == null)
        {
            Debug.LogError("InventorySystem не найден на сцене!");
            return;
        }
        
        // 🔥 Подписываемся на событие изменения инвентаря
        inventory.OnInventoryChanged += UpdateUI;
        
        // Инициализируем слоты
        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i] != null)
                slots[i].Initialize(i, this);
        }
        
        // Первоначальное обновление
        UpdateUI();
    }
    
    void UpdateUI()
    {
        if (inventory == null) return;
        
        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i] == null) continue;
            
            if (i < inventory.inventory.Count)
            {
                slots[i].UpdateSlot(inventory.inventory[i]);
            }
        }
    }
    
    public void OnSlotClicked(int slotIndex)
    {
        Debug.Log($"Клик по слоту {slotIndex}");
        inventory?.RemoveItem(slotIndex);
    }
    
    void OnDestroy()
    {
        // 🔥 Важно отписаться от события при уничтожении
        if (inventory != null)
            inventory.OnInventoryChanged -= UpdateUI;
    }
}