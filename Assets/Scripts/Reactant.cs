using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reactant : MonoBehaviour
{
    public float rotateSpeed;
    
    [SerializeField] private Transform bigCenter;
    
    private bool _isShooted;

    private void Update()
    {
        if (!_isShooted)
        { 
            bigCenter.RotateAround(bigCenter.position, Vector3.forward, rotateSpeed * Time.deltaTime);
        }
    }
}
