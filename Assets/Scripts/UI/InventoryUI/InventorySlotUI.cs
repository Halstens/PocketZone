using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySlotUI : MonoBehaviour
{
    private SpriteRenderer _icon;
    private TextMeshProUGUI _quantityText;
    private Button _button;
    
    private int _slotIndex;
    private InventoryUI _inventoryUI;
    
    
    void Awake()
    {
        _icon = GetComponentInChildren<SpriteRenderer>();
        _quantityText = GetComponentInChildren<TextMeshProUGUI>();
        _button = GetComponent<Button>();
        
        if (_button != null)
            _button.onClick.AddListener(OnClick);
        
    }
    
    public void Initialize(int index, InventoryUI ui)
    {
        _slotIndex = index;
        _inventoryUI = ui;
        UpdateSlot(null);
    }
    
    // public void UpdateSlot(InventoryItem item)
    // {
    //     if (item != null && item.data != null)
    //     {
    //         if (_icon != null)
    //         {
    //             _icon.sprite = item.data.icon;
    //             //icon.color = Color.white;
    //             Debug.Log($"Иконка установлена: {_icon.sprite}");
    //         }
    //         
    //         if (_quantityText != null)
    //         {
    //             _quantityText.text = item.quantity > 1 ? item.quantity.ToString() : "";
    //         }
    //     }
    //     else
    //     {
    //         if (_icon != null)
    //         {
    //             _icon.sprite = null;
    //             _icon.color = Color.clear;
    //         }
    //         
    //         if (_quantityText != null)
    //             _quantityText.text = "";
    //     }
    // }
    public void UpdateSlot(InventoryItem item)
    {
        Debug.Log($"UpdateSlot вызван для слота {_slotIndex}. Item: {item != null}, Data: {item?.data}");

        if (item != null && item.data != null)
        {
            Debug.Log($"Item icon: {item.data.icon}");

            if (_icon != null)
            {
                _icon.sprite = item.data.icon;
                _icon.color = Color.white;
                Debug.Log($"Иконка установлена: {_icon.sprite}");
            }
            else
            {
                Debug.LogError("Icon не найден в слоте!");
            }

            if (_quantityText != null)
            {
                _quantityText.text = item.quantity > 1 ? item.quantity.ToString() : "";
            }
        }
        else
        {
            Debug.Log("Очищаем слот");

            if (_icon != null)
            {
                _icon.sprite = null;
                _icon.color = Color.clear;
            }

            if (_quantityText != null)
                _quantityText.text = "";
        }
    }
    
    void OnClick()
    {
        _inventoryUI?.OnSlotClicked(_slotIndex);
        Debug.Log("Click on slot!");
    }
}