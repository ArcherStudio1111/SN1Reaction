using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reactant : MonoBehaviour
{
    public float rotateSpeed;
    
    [SerializeField] private Transform bigCenter;
    [SerializeField] private Transform axis;

    private void Update()
    {
        bigCenter.RotateAround(bigCenter.position, axis.up, rotateSpeed * Time.deltaTime);
    }
}
