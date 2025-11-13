using System.IO;
using UnityEngine;

public class SimpleSave : MonoBehaviour
{
    public void SaveInventory()
    {
        InventorySystem inventory = FindObjectOfType<InventorySystem>();
        if (inventory == null) return;
        
        string[] savedItems = new string[4];
        for (int i = 0; i < 4; i++)
        {
            if (inventory.inventory[i] != null)
                savedItems[i] = inventory.inventory[i].data.name;
            else
                savedItems[i] = "";
        }
        
        string json = JsonUtility.ToJson(savedItems);
        File.WriteAllText(Application.persistentDataPath + "/inventory.json", json);
    }
    
    public void LoadInventory()
    {
        string path = Application.persistentDataPath + "/inventory.json";
        if (!File.Exists(path)) return;
        
        string json = File.ReadAllText(path);
        string[] savedItems = JsonUtility.FromJson<string[]>(json);
        
        InventorySystem inventory = FindObjectOfType<InventorySystem>();
        if (inventory == null) return;
        
        for (int i = 0; i < 4; i++)
        {
            if (!string.IsNullOrEmpty(savedItems[i]))
            {
                ItemData item = Resources.Load<ItemData>($"Items/{savedItems[i]}");
                if (item != null)
                    inventory.AddItem(item);
            }
        }
    }
}