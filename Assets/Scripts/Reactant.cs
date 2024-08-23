using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Reactant : MonoBehaviour
{
    public float rotateSpeed;

    [SerializeField] private GameObject h1Product;
    [SerializeField] private GameObject h2Product;
    [SerializeField] private GameObject originReactant;

    [SerializeField] private Transform bigCenter;
    [SerializeField] private Transform axis;
    [SerializeField] private Transform h1Sphere;
    [SerializeField] private Transform h2Sphere;
    [SerializeField] private Transform bigSphere;
    [SerializeField] private AudioSource incorrectSound;

    public static event Action S2GameWinEvent;
    
    private void Update()
    {
        bigCenter.RotateAround(bigCenter.position, axis.up, rotateSpeed * Time.deltaTime);
    }

    public void JudgeWin(string smallSphereName)
    {
        if (smallSphereName.Equals("h1") && 
            Vector3.Angle(h1Sphere.forward, bigSphere.forward) <= 177 && 
            Vector3.Angle(h1Sphere.forward, bigSphere.forward) >= 167)
        {
            h1Product.SetActive(true);
            originReactant.SetActive(false);
            var bulletObject = GameObject.FindGameObjectWithTag("Bullet");
            Destroy(bulletObject);
            S2GameWinEvent?.Invoke();
        }
        
        if (smallSphereName.Equals("h2") && 
            Vector3.Angle(h2Sphere.forward, bigSphere.forward) <= 177 && 
            Vector3.Angle(h2Sphere.forward, bigSphere.forward) >= 167)
        {
            h2Product.SetActive(true);
            originReactant.SetActive(false);
            var bulletObject = GameObject.FindGameObjectWithTag("Bullet");
            Destroy(bulletObject);
            S2GameWinEvent?.Invoke();
        }
    }
}
