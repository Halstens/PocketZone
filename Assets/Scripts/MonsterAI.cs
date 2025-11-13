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
    
    private Transform _player;
    private Rigidbody2D _rb;
    private HealthSystem _system;
    private HealthSystem _playerHealth;
    private float _lastAttackTime;
    private bool _isDead = false;
    
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _rb = GetComponent<Rigidbody2D>();
        _system = GetComponent<HealthSystem>();
        _playerHealth = _player.GetComponent<HealthSystem>();
        
        if (_system != null)
        {
            _system.OnDeath.AddListener(OnMonsterDeath);
        }
        
        // Убедимся что Rigidbody2D настроен правильно
        if (_rb != null)
        {
            _rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        }
    }
    
    void Update()
    {
        if ((_isDead || _player).Equals(null)) return;
        
        float distanceToPlayer = Vector2.Distance(transform.position, _player.position);
        
        if (distanceToPlayer <= detectionRange)
        {
            // Останавливаемся на stopDistance от игрока
            if (distanceToPlayer > stopDistance)
            {
                // Движение к игроку
                Vector2 direction = (_player.position - transform.position).normalized;
                
                // Используем MovePosition для плавного движения
                Vector2 newPosition = Vector2.MoveTowards(
                    _rb.position, 
                    _player.position, 
                    moveSpeed * Time.deltaTime
                );
                _rb.MovePosition(newPosition);
            }
            else
            {
                // Останавливаемся если слишком близко
                _rb.velocity = Vector2.zero;
            }
            
            // Атака если в радиусе
            if (distanceToPlayer <= attackRange && Time.time >= _lastAttackTime + attackCooldown)
            {
                AttackPlayer();
            }
        }
        else
        {
            _rb.velocity = Vector2.zero;
        }
    }
    
    void AttackPlayer()
    {
        _lastAttackTime = Time.time;
        
        if (!_playerHealth.Equals(null))
        {
            _playerHealth.TakeDamage(attackDamage);
            //Debug.Log($"Монстр атаковал игрока! Урон: {attackDamage}");
        }
    }
    
    void OnMonsterDeath()
    {
        _isDead = true;
        _rb.velocity = Vector2.zero;
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