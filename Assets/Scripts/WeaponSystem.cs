using UnityEngine;

public class WeaponSystem : MonoBehaviour
{
    // Public поля для настройки в Inspector
    [Header("Shooting")]
    public Transform firePoint;
    public GameObject bulletPrefab;
    public float bulletForce = 15f;
    
    [Header("Ammo")]
    public int maxAmmo = 30;
    public int currentAmmo;
    
    [Header("Settings")]
    public float fireRate = 5.2f;
    
    // Private поля (не видны в Inspector)
    private float nextFireTime = 0f;  // ← ЭТОЙ СТРОКИ НЕ ХВАТАЛО!
    private bool isShooting = false;
    
    // Свойство только для чтения
    public bool CanShoot => currentAmmo > 0 && Time.time >= nextFireTime;
    
    // Остальной код без изменений...
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
        
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Debug.Log($"Выстрел! Патронов осталось: {currentAmmo}");
    }
    
    public void AddAmmo(int amount)
    {
        currentAmmo = Mathf.Clamp(currentAmmo + amount, 0, maxAmmo);
        Debug.Log($"Добавлено {amount} патронов. Теперь: {currentAmmo}");
    }
}