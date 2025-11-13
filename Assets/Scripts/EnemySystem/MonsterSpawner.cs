using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject monsterPrefab;
    public int monsterCount = 3;
    public float spawnRadius = 10f;
    public float minDistanceFromPlayer = 3f;
    
    private Transform player;
    
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        SpawnMonsters();
    }
    
    void SpawnMonsters()
    {
        for (int i = 0; i < monsterCount; i++)
        {
            Vector2 spawnPosition = GetValidSpawnPosition();
            if (spawnPosition != Vector2.zero)
            {
                Instantiate(monsterPrefab, spawnPosition, Quaternion.identity);
            }
        }
    }
    
    Vector2 GetValidSpawnPosition()
    {
        int attempts = 0;
        while (attempts < 20) 
        {
            Vector2 randomPos = (Vector2)transform.position + Random.insideUnitCircle * spawnRadius;
            
            if (Vector2.Distance(randomPos, player.position) > minDistanceFromPlayer)
            {
                Collider2D hit = Physics2D.OverlapCircle(randomPos, 0.5f);
                if (hit == null || hit.isTrigger)
                {
                    return randomPos;
                }
            }
            
            attempts++;
        }
        
        return Vector2.zero;
    }
    
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }
}