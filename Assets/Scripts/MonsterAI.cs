using UnityEngine;

public class MonsterAI : MonoBehaviour
{
    [Header("AI Settings")]
    public float moveSpeed = 2f;
    public float detectionRange = 5f;
    public float attackRange = 1.5f;
    public float stopDistance = 1f; // Новое: расстояние остановки
    public float attackDamage = 10f;
    public float attackCooldown = 2f;
    
    [Header("Loot")]
    public GameObject lootItem;
    
    private Transform player;
    private Rigidbody2D rb;
    private HealthSystem healthSystem;
    private HealthSystem playerHealth;
    private float lastAttackTime;
    private bool isDead = false;
    
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        healthSystem = GetComponent<HealthSystem>();
        playerHealth = player.GetComponent<HealthSystem>();
        
        if (healthSystem != null)
        {
            healthSystem.OnDeath.AddListener(OnMonsterDeath);
        }
        
        // Убедимся что Rigidbody2D настроен правильно
        if (rb != null)
        {
            rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        }
    }
    
    void Update()
    {
        if (isDead || player == null) return;
        
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        
        if (distanceToPlayer <= detectionRange)
        {
            // Останавливаемся на stopDistance от игрока
            if (distanceToPlayer > stopDistance)
            {
                // Движение к игроку
                Vector2 direction = (player.position - transform.position).normalized;
                
                // Используем MovePosition для плавного движения
                Vector2 newPosition = Vector2.MoveTowards(
                    rb.position, 
                    player.position, 
                    moveSpeed * Time.deltaTime
                );
                rb.MovePosition(newPosition);
            }
            else
            {
                // Останавливаемся если слишком близко
                rb.velocity = Vector2.zero;
            }
            
            // Атака если в радиусе
            if (distanceToPlayer <= attackRange && Time.time >= lastAttackTime + attackCooldown)
            {
                AttackPlayer();
            }
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }
    
    void AttackPlayer()
    {
        lastAttackTime = Time.time;
        
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(attackDamage);
            Debug.Log($"Монстр атаковал игрока! Урон: {attackDamage}");
        }
    }
    
    void OnMonsterDeath()
    {
        isDead = true;
        rb.velocity = Vector2.zero;
        DropLoot();
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
        Destroy(gameObject, 2f);
    }
    
    void DropLoot()
    {
        if (lootItem != null)
        {
            Instantiate(lootItem, transform.position, Quaternion.identity);
        }
    }
    
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        
        // Новая: визуализация дистанции остановки
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, stopDistance);
    }
}