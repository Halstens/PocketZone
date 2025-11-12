using UnityEngine;
using UnityEngine.Events;

public class HealthSystem : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 100f;
    public float currentHealth;
    
    [Header("Events")]
    public UnityEvent<float> OnHealthChanged; // UnityEvent с параметром
    public UnityEvent OnDeath;
    
    public float HealthPercent => currentHealth / maxHealth;
    public bool IsDead => currentHealth <= 0;
    
    void Start()
    {
        currentHealth = maxHealth;
        OnHealthChanged?.Invoke(currentHealth);
    }
    
    public void TakeDamage(float damage)
    {
        if (IsDead) return;
        
        currentHealth = Mathf.Clamp(currentHealth - damage, 0, maxHealth);
        OnHealthChanged?.Invoke(currentHealth);
        
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    
    public void Heal(float healAmount)
    {
        if (IsDead) return;
        
        currentHealth = Mathf.Clamp(currentHealth + healAmount, 0, maxHealth);
        OnHealthChanged?.Invoke(currentHealth);
    }
    
    private void Die()
    {
        OnDeath?.Invoke();
        
        PlayerController playerController = GetComponent<PlayerController>();
        if (playerController != null)
        {
            playerController.enabled = false;
        }
        
        Collider2D collider = GetComponent<Collider2D>();
        if (collider != null) collider.enabled = false;
        
        Debug.Log($"{gameObject.name} умер!");
    }
}