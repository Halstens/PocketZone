using UnityEngine;

public class WeaponSystem : MonoBehaviour
{
    [Header("Shooting")]
    public Transform firePoint;
    public GameObject bulletPrefab;
    public float bulletForce = 15f;
    
    [Header("Ammo")]
    public int maxAmmo = 30;
    public int currentAmmo;
    
    [Header("Settings")]
    public float fireRate = 0.2f;
    
    private float nextFireTime = 0f;
    private bool isShooting = false;
    
    public bool CanShoot => currentAmmo > 0 && Time.time >= nextFireTime;
    
    void Start()
    {
        currentAmmo = maxAmmo;
    }
    
    void Update()
    {
        if (isShooting && CanShoot)
        {
            Shoot();
        }
    }
    
    public void StartShooting()
    {
        isShooting = true;
    }
    
    public void StopShooting()
    {
        isShooting = false;
    }
    
    private void Shoot()
    {
        currentAmmo--;
        nextFireTime = Time.time + fireRate;
        
        // Создаем пулю
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        
        if (rb != null)
        {
            rb.AddForce(firePoint.right * bulletForce, ForceMode2D.Impulse);
        }
        
        Debug.Log($"Выстрел! Патронов осталось: {currentAmmo}");
    }
    
    public void AddAmmo(int amount)
    {
        currentAmmo = Mathf.Clamp(currentAmmo + amount, 0, maxAmmo);
        Debug.Log($"Добавлено {amount} патронов. Теперь: {currentAmmo}");
    }
}