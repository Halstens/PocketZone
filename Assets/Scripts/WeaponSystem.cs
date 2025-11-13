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
    public bool autoShoot = true;
    
    // Private поля (не видны в Inspector)
    private float _nextFireTime = 0f;  // ← ЭТОЙ СТРОКИ НЕ ХВАТАЛО!
    private bool _isShooting = false;
    private PlayerController _autoAim;

    
    // Свойство только для чтения
    public bool CanShoot => currentAmmo > 0 && Time.time >= _nextFireTime;
    
    // Остальной код без изменений...
    void Start()
    {
        currentAmmo = maxAmmo;
        _autoAim = GetComponent<PlayerController>();
    }
    
    void Update()
    {
        if (_isShooting && CanShoot)
        {
            Shoot();
        }
    }
    
    public void StartShooting()
    {
        _isShooting = true;
    }
    
    public void StopShooting()
    {
        _isShooting = false;
    }
    
    private void Shoot()
    {
        currentAmmo--;
        _nextFireTime = Time.time + fireRate;
        
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

    }
    
    public void AddAmmo(int amount)
    {
        currentAmmo = Mathf.Clamp(currentAmmo + amount, 0, maxAmmo);
    }
}