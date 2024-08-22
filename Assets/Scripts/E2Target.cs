using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E2Target : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            Debug.Log(123);
        }
    }
}
