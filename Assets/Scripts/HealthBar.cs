using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private HealthSystem healthSystem;
    [SerializeField] private Slider healthSlider;
    
    [Header("Settings")]
    [SerializeField] private bool alwaysVisible = true; // Новая настройка
    
    private Transform cameraTransform;
    
    void Start()
    {
        Debug.Log("WorldHealthBar Start вызван");
        
        cameraTransform = Camera.main.transform;
        
        if (healthSystem == null)
            healthSystem = GetComponentInParent<HealthSystem>();
        
        if (healthSystem == null)
        {
            Debug.LogError("HealthSystem не найден!");
            return;
        }
        
        if (healthSlider == null)
        {
            Debug.LogError("HealthSlider не назначен!");
            return;
        }
        
        healthSystem.OnHealthChanged.AddListener(UpdateHealthBar);
        healthSystem.OnDeath.AddListener(OnCharacterDeath);
        
        // Принудительно обновляем полосу при старте
        UpdateHealthBar(healthSystem.currentHealth);
        
        Debug.Log($"HealthBar инициализирован. Здоровье: {healthSystem.currentHealth}/{healthSystem.maxHealth}");
    }
    
    // void Update()
    // {
    //     // Поворачиваем healthbar к камере
    //     if (cameraTransform  null)
    //     {
    //         transform.LookAt(transform.position + cameraTransform.forward);
    //     }
    // }
    
    void UpdateHealthBar(float currentHealth)
    {
        Debug.Log($"UpdateHealthBar вызван: {currentHealth}");
        
        if (healthSlider != null)
        {
            healthSlider.value = healthSystem.HealthPercent;
            
            // ИЗМЕНЕНИЕ: Всегда показываем, если alwaysVisible = true
            bool shouldShow = alwaysVisible || currentHealth < healthSystem.maxHealth;
            healthSlider.gameObject.SetActive(shouldShow);
            
            Debug.Log($"HealthBar обновлен: {healthSystem.HealthPercent:P0}, видим: {shouldShow}");
        }
    }
    
    void OnCharacterDeath()
    {
        Debug.Log("OnCharacterDeath вызван");
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