using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySlotUI : MonoBehaviour
{
    [Header("Assign Manually")]
    private SpriteRenderer _icon;
   
    private TextMeshProUGUI _quantityText;
    private Button _button;
    
    private int _slotIndex;
    private InventoryUI _inventoryUI;
    
    void Awake()
    {
        _icon = GetComponentInChildren<SpriteRenderer>();
        _button = GetComponent<Button>();
        _quantityText = GetComponentInChildren<TextMeshProUGUI>();
        
        if (_button != null)
            _button.onClick.AddListener(OnClick);
    }
    
    public void Initialize(int index, InventoryUI ui)
    {
        _slotIndex = index;
        _inventoryUI = ui;
        
        UpdateSlot(null);
    }
    
    public void UpdateSlot(InventoryItem item)
    {
        if (item != null && item.data != null)
        {
            if (_icon != null)
            {
                _icon.sprite = item.data.icon;
                _icon.color = Color.white;
            }
            
            if (_quantityText != null)
            {
                _quantityText.text = item.quantity > 1 ? item.quantity.ToString() : "";
            }
        }
        else
        {
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
    }
}