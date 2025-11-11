using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float offset;
    public FixedJoystick Joystick;
    public Transform weaponArmR;
    public Transform weaponArmL;
    public Transform head;
    public GameObject partsPlayer;
    
    private Camera _mainCamera;
    private float _angleAim;


    private void Start()
    {
        _mainCamera = Camera.main;
    }

    void Update()
    {
       MovementJoystick();
       AimToPos();
       
    }

    void MovementJoystick()
    {
        float moveX = Joystick.Horizontal;
        float moveY = Joystick.Vertical;
        gameObject.transform.position += new Vector3(moveX, moveY, 0) * speed * Time.deltaTime;
    }
    
    void AimToPos()
    {
        Vector3 mousePos = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = (mousePos - transform.position).normalized;
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
    
        if (!weaponArmR.Equals(null) && !weaponArmL.Equals(null))
        {
            weaponArmR.rotation = Quaternion.Euler(0, _angleAim, angle + offset);
            weaponArmL.rotation = Quaternion.Euler(0, _angleAim, angle + offset);
        }
        
        if (!head.Equals(null))
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
}
