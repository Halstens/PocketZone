using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float offset;
    public FixedJoystick Joystick;
    public Transform weaponArmR;
    public Transform weaponArmL;
    public Transform head;
    public GameObject partsPlayer;
    public LayerMask enemyLayerMask;
    public CircleCollider2D detectionZone;
    
    private Camera _mainCamera;
    private float _angleAim;
    private List<Transform> _enemiesInRange = new List<Transform>();
    private Transform _currentTarget;
    private bool _wasAiming = false; // Добавляем флаг для отслеживания состояния прицеливания

    private void Start()
    {
        if (detectionZone == null)
            detectionZone = GetComponent<CircleCollider2D>();
    }

    void Update()
    {
       MovementJoystick();
       UpdateCurrentTarget();
       
       if (_currentTarget != null)
       {
           AimToTarget();
           _wasAiming = true;
       }
       else
       {
           // Если только что перестали прицеливаться - возвращаем в исходное положение
           if (_wasAiming)
           {
               ResetToDefaultPosition();
               _wasAiming = false;
           }
       }
    }
    
    void UpdateCurrentTarget()
    {
        if (_enemiesInRange.Count == 0)
        {
            _currentTarget = null;
            return;
        }
        
        _enemiesInRange.RemoveAll(enemy => enemy == null);
        
        _currentTarget = _enemiesInRange.OrderBy(enemy => Vector2.Distance(transform.position, enemy.position))
            .FirstOrDefault();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & enemyLayerMask) != 0)
        {
            if (!_enemiesInRange.Contains(other.transform))
            {
                _enemiesInRange.Add(other.transform);
            }
        }
    }

    void MovementJoystick()
    {
        float moveX = Joystick.Horizontal;
        float moveY = Joystick.Vertical;
        gameObject.transform.position += new Vector3(moveX, moveY, 0) * speed * Time.deltaTime;
    }
    
    void AimToTarget()
    {
        if (_currentTarget == null) return;
        
        Vector3 direction = (_currentTarget.position - transform.position).normalized;
        direction.z = 0;
    
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    
        if (direction.x < 0)
        {
            partsPlayer.transform.localRotation = Quaternion.Euler(0, 180, 0);
            _angleAim = 180;
            angle = 180 - angle;
            
            if (angle > 180) angle -= 360;
            if (angle < -180) angle += 360;
        }
        else
        {
            partsPlayer.transform.localRotation = Quaternion.Euler(0, 0, 0);
            _angleAim = 0;
        }
    
        if (weaponArmR != null && weaponArmL != null)
        {
            weaponArmR.rotation = Quaternion.Euler(0, _angleAim, angle + offset);
            weaponArmL.rotation = Quaternion.Euler(0, _angleAim, angle + offset);
        }
        
        if (head != null)
        {
            float headAngle;
        
            if (_angleAim == 180) 
            {
                headAngle = Mathf.Clamp(angle, -135f, 135f);
            }
            else 
            {
                headAngle = Mathf.Clamp(angle, -45f, 45f);
            }
        
            head.rotation = Quaternion.Euler(0, _angleAim, headAngle);
        }
    }
    
    // 🔥 НОВЫЙ МЕТОД: Возврат в исходное положение
    void ResetToDefaultPosition()
    {
        // Возвращаем тело в положение "смотрит вправо"
        partsPlayer.transform.localRotation = Quaternion.Lerp(
            partsPlayer.transform.localRotation,
            Quaternion.Euler(0, 0, 0),
            Time.deltaTime * 5f
        );
            
        // Возвращаем оружие в нейтральное положение (вправо)
        if (weaponArmR != null && weaponArmL != null)
        {
            weaponArmR.rotation = Quaternion.Lerp(
                weaponArmR.rotation,
                Quaternion.Euler(0, 0, 0 + offset),
                Time.deltaTime * 5f
            );
            weaponArmL.rotation = Quaternion.Lerp(
                weaponArmR.rotation,
                Quaternion.Euler(0, 0, 0 + offset),
                Time.deltaTime * 5f
            );
        }
        
        // Возвращаем голову в нейтральное положение
        if (head != null)
        {
            head.rotation = Quaternion.Lerp(
                head.rotation,
                Quaternion.Euler(0, 0, 0),
                Time.deltaTime * 5f
            );
        }
        
        _angleAim = 0;
        //Debug.Log("🎯 Персонаж возвращен в исходное положение");
    }
    
    void OnTriggerExit2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & enemyLayerMask) != 0)
        {
            _enemiesInRange.Remove(other.transform);
            
            if (_currentTarget == other.transform)
            {
                _currentTarget = null;
            }
        }
    }
    
    public bool HasTarget()
    {
        return _currentTarget != null;
    }
    
    public Transform GetCurrentTarget()
    {
        return _currentTarget;
    }
    
    public void RemoveEnemy(Transform enemy)
    {
        _enemiesInRange.Remove(enemy);
        if (_currentTarget == enemy)
        {
            _currentTarget = null;
        }
    }
}