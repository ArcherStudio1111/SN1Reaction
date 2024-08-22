using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AimingPoint : MonoBehaviour
{
    public float moveSpeed;

    // Update is called once per frame
    private void Update()
    {
        Move();
    }

    private void Move()
    {
        transform.Rotate(Vector3.right, Time.deltaTime * moveSpeed * Input.GetAxisRaw("Vertical"));
        transform.Rotate(Vector3.down, Time.deltaTime * moveSpeed * Input.GetAxisRaw("Horizontal"));
    }
}
 