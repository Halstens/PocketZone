using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class ShootButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private WeaponSystem weaponSystem;
    
    void Start()
    {
        if (weaponSystem == null)
            weaponSystem = FindObjectOfType<WeaponSystem>();
    }
    
    public void OnPointerDown(PointerEventData eventData)
    {
        if (weaponSystem != null)
            weaponSystem.StartShooting();
    }
    
    public void OnPointerUp(PointerEventData eventData)
    {
        if (weaponSystem != null)
            weaponSystem.StopShooting();
    }
}