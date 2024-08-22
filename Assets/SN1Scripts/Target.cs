using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public float rotateVelocity;

    [SerializeField] private Rigidbody rb;

    private Vector3 RandomVector3;

    private void Start()
    {
            RandomVector3 = new Vector3(Random.Range(-rotateVelocity, rotateVelocity),
            Random.Range(-rotateVelocity, rotateVelocity),
            Random.Range(-rotateVelocity, rotateVelocity));
        rb.angularVelocity = RandomVector3 * rotateVelocity;
    }

    public void FreezeResumeRotate()
    {
        rb.angularVelocity = rb.angularVelocity == Vector3.zero ? RandomVector3 * rotateVelocity : Vector3.zero;
    }
}
