using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapBorder : MonoBehaviour
{
    [SerializeField] private Gun gun;

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            //gun.RecyicleBullet(other.transform);
        }
    }
}
