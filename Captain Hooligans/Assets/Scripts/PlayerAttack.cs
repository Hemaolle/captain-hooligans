using UnityEngine;
using System.Collections;

public class PlayerAttack : MonoBehaviour {
    
    public GameObject bulletPrefab;
    
    public Transform gunPoint;
    
    public float bulletSpeed = 10f;
    public float fireRate = 1f;
    
    public float baseDamage = 1f;
    
    private float _playerDamage;
    
    private float _timer1 = 0f;
    private float _timer2 = 0f;
    
    void Start() {
        _playerDamage = baseDamage;
    }
    
    void Update() {
        if (Input.GetButton("Fire1")) {
            if (_timer1 > 1f / fireRate) {
                shoot(bulletPrefab, gunPoint);
                _timer1 = 0f;
            }
            
        }
        
        _timer1 += Time.deltaTime; 
    }
    
    private void shoot(GameObject bulletPrefab, Transform gunPoint) {
        GameObject o = (GameObject) Instantiate(bulletPrefab, gunPoint.position, Quaternion.identity);
        o.rigidbody.velocity = transform.TransformDirection(Vector3.forward) * bulletSpeed;
        o.transform.Rotate(gunPoint.eulerAngles);    
    }
    
    
    
}