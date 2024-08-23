using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuProduct : MonoBehaviour
{
    public float rotateSpeed = 5;
    
    private void Update()
    {
        transform.Rotate(Time.deltaTime * rotateSpeed * Vector3.down);
    }
}
