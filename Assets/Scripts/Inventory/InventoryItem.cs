[System.Serializable]
public class InventoryItem
{
    public ItemData data;
    public int quantity;
    
    public InventoryItem(ItemData itemData, int amount = 1)
    {
        data = itemData;
        quantity = amount;
    }
}