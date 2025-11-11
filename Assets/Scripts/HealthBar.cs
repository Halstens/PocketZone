using UnityEngine;
using UnityEngine.UI;

public class WorldHealthBar : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private HealthSystem healthSystem;
    [SerializeField] private Slider healthSlider;
    
    private Transform cameraTransform;
    
    void Start()
    {
        cameraTransform = Camera.main.transform;
        
        if (healthSystem == null)
            healthSystem = GetComponentInParent<HealthSystem>();
        
        if (healthSystem == null)
        {
            Debug.LogError("HealthSystem не найден!");
            return;
        }
        
        // ПРАВИЛЬНОЕ использование UnityEvent - через AddListener
        healthSystem.OnHealthChanged.AddListener(UpdateHealthBar);
        healthSystem.OnDeath.AddListener(OnCharacterDeath);
        
        UpdateHealthBar(healthSystem.currentHealth);
        
        healthSlider.gameObject.SetActive(healthSystem.currentHealth < healthSystem.maxHealth);
    }
    
    void Update()
    {
        // Поворачиваем healthbar к камере
        if (cameraTransform != null)
        {
            transform.LookAt(transform.position + cameraTransform.forward);
        }
    }
    
    void UpdateHealthBar(float currentHealth)
    {
        if (healthSlider != null)
        {
            healthSlider.value = healthSystem.HealthPercent;
            healthSlider.gameObject.SetActive(currentHealth < healthSystem.maxHealth);
        }
    }
    
    void OnCharacterDeath()
    {
        if (healthSlider != null)
            healthSlider.gameObject.SetActive(false);
    }
    
    void OnDestroy()
    {
        // Важно отписаться при уничтожении
        if (healthSystem != null)
        {
            healthSystem.OnHealthChanged.RemoveListener(UpdateHealthBar);
            healthSystem.OnDeath.RemoveListener(OnCharacterDeath);
        }
    }
}