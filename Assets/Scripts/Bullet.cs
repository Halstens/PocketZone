using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Bullet Settings")]
    public float damage = 25f;
    public float lifetime = 3f;
    public float speed = 15f;
    
    private Rigidbody2D rb;
    private bool isDestroyed = false;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
        if (rb != null)
        {
            rb.velocity = transform.right * speed;
        }
        
        Invoke("DestroyBullet", lifetime);
    }
    
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDestroyed) return;
        
        // Проверяем ТЕГИ, а не слои
        if (collision.CompareTag("Monster"))
        {
            HealthSystem health = collision.GetComponent<HealthSystem>();
            if (health != null)
            {
                health.TakeDamage(damage);
            }
            DestroyBullet();
        }
        else if (collision.CompareTag("Obstacle") || collision.CompareTag("Wall"))
        {
            DestroyBullet();
        }
        // Игнорируем столкновения с другими пулями и игроком
        else if (collision.CompareTag("Bullet") || collision.CompareTag("Player"))
        {
            // Ничего не делаем
        }
    }
    
    void DestroyBullet()
    {
        if (isDestroyed) return;
        isDestroyed = true;
        Destroy(gameObject);
    }
}