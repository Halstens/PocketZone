using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private HealthSystem healthSystem;
    [SerializeField] private Slider healthSlider;
    
    [Header("Settings")]
    [SerializeField] private bool alwaysVisible = true;
    
    private Transform _cameraTransform;
    
    void Start()
    {
        _cameraTransform = Camera.main.transform;
        
        if (healthSystem == null)
            healthSystem = GetComponentInParent<HealthSystem>();
        
        if (healthSystem == null)
        {
            return;
        }
        
        if (healthSlider == null)
        {
            return;
        }
        
        healthSystem.OnHealthChanged.AddListener(UpdateHealthBar);
        healthSystem.OnDeath.AddListener(OnCharacterDeath);
        
        UpdateHealthBar(healthSystem.currentHealth);
        
    }
    
    
    void UpdateHealthBar(float currentHealth)
    {

        if (healthSlider != null)
        {
            healthSlider.value = healthSystem.HealthPercent;
            
            bool shouldShow = alwaysVisible || currentHealth < healthSystem.maxHealth;
            healthSlider.gameObject.SetActive(shouldShow);
            
        }
    }
    
    void OnCharacterDeath()
    {
        if (healthSlider != null)
            healthSlider.gameObject.SetActive(false);
    }
    
    void OnDestroy()
    {
        if (healthSystem != null)
        {
            healthSystem.OnHealthChanged.RemoveListener(UpdateHealthBar);
            healthSystem.OnDeath.RemoveListener(OnCharacterDeath);
        }
    }
}