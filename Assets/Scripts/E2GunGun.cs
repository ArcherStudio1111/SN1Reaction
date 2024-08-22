using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class E2Gun : MonoBehaviour
{
    public float bulletVelocity;
    
    [SerializeField] private Transform bullet;

    private readonly float _shootInterval = 2f;
    private float _shootTimer;
    private bool _isShooted;
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !_isShooted)
        {
            Shoot();
            _isShooted = true;
        }

        if (_isShooted)
        {
            _shootTimer += Time.deltaTime;
            if (_shootTimer >= _shootInterval)
            {
                _shootTimer = 0;
                _isShooted = false;
            }
        }
    }

    private void Shoot()
    {
        var bulletClone = Instantiate(bullet, transform.position, Quaternion.identity);
        bulletClone.GetComponent<Rigidbody>().velocity = Vector3.forward * bulletVelocity;
    }
}
