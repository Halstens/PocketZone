// Scripts/Test/DebugInventory.cs
using UnityEngine;

public class DebugInventory : MonoBehaviour
{
    void Start()
    {
        InventorySystem inv = FindObjectOfType<InventorySystem>();
        InventoryUI ui = FindObjectOfType<InventoryUI>();
        
        Debug.Log($"InventorySystem найден: {inv != null}");
        Debug.Log($"InventoryUI найден: {ui != null}");
        
        if (ui != null && ui.slots != null)
        {
            Debug.Log($"Количество слотов в UI: {ui.slots.Count}");
            for (int i = 0; i < ui.slots.Count; i++)
            {
                Debug.Log($"Слот {i}: {ui.slots[i] != null}");
            }
        }
    }
}