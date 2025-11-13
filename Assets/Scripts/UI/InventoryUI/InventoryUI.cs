using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class InventoryUI : MonoBehaviour
{
    [Header("UI References")]
    public List<InventorySlotUI> slots;
    public Button deleteButton; 
    
    private InventorySystem _inventory;
    private int _selectedSlotIndex = -1; 
    
    void Start()
    {
        _inventory = FindObjectOfType<InventorySystem>();
        if (_inventory == null)
        {
            return;
        }
        
        deleteButton.gameObject.SetActive(false);
        deleteButton.onClick.AddListener(OnDeleteButtonClicked);
        
        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i] != null)
                slots[i].Initialize(i, this);
        }
        
        _inventory.OnInventoryChanged += UpdateUI;
        UpdateUI();
    }
    
    void UpdateUI()
    {
        if (_inventory == null) return;
        
        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i] == null) continue;
            
            if (i < _inventory.inventory.Count)
            {
                slots[i].UpdateSlot(_inventory.inventory[i]);
            }
        }
        
        if (_selectedSlotIndex != -1 && _inventory.inventory[_selectedSlotIndex] == null)
        {
            DeselectSlot();
        }
    }
    
    public void OnSlotClicked(int slotIndex)
    {
        
        if (_selectedSlotIndex == slotIndex)
        {
            DeselectSlot();
            return;
        }
        
        if (_inventory.inventory[slotIndex] != null)
        {
            SelectSlot(slotIndex);
        }
        else
        {
            DeselectSlot();
        }
    }
    
    void SelectSlot(int slotIndex)
    {
        _selectedSlotIndex = slotIndex;
        deleteButton.gameObject.SetActive(true);
    }
    
    void DeselectSlot()
    {
        _selectedSlotIndex = -1;
        deleteButton.gameObject.SetActive(false);
    }
    
    void OnDeleteButtonClicked()
    {
        if (_selectedSlotIndex != -1)
        {
            Debug.Log($"🗑️ Удаление предмета из слота {_selectedSlotIndex}");
            _inventory.RemoveItem(_selectedSlotIndex);
            DeselectSlot();
        }
    }
    
    void OnDestroy()
    {
        if (_inventory != null)
            _inventory.OnInventoryChanged -= UpdateUI;
    }
}