using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class Reactant : MonoBehaviour
{
    public float rotateSpeed;
    public float wholeRotateSpeed = 5f;
    
    [SerializeField] private GameObject h1Product;
    [SerializeField] private GameObject h2Product;
    [SerializeField] private GameObject originReactant;
    [SerializeField] private Transform byProduct_1;
    [SerializeField] private Transform byProduct_2;

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
        if (SceneManager.GetActiveScene().name.Equals("Level_2"))
        {
            transform.RotateAround(transform.position, axis.up, wholeRotateSpeed * Time.deltaTime);
        }
    }

    public void JudgeWin(string smallSphereName)
    {
        if (smallSphereName.Equals("h1") && 
            Vector3.Angle(h1Sphere.forward, bigSphere.forward) <= 179.5f && 
            Vector3.Angle(h1Sphere.forward, bigSphere.forward) >= 164.5f)
        {
            h1Product.SetActive(true);
            originReactant.SetActive(false);
            var bulletObject = GameObject.FindGameObjectWithTag("Bullet");
            Destroy(bulletObject);

            if (SceneManager.GetActiveScene().name.Equals("Level_1"))
            {
                GameManager.Lv1_h1Win = true;
            }
            else if (SceneManager.GetActiveScene().name.Equals("Level_2"))
            {
                GameManager.Lv2_h1Win = true;
            }
            S2GameWinEvent?.Invoke();
            GetByProduct();
        }
        
        if (smallSphereName.Equals("h2") && 
            Vector3.Angle(h2Sphere.forward, bigSphere.forward) <= 179.5f && 
            Vector3.Angle(h2Sphere.forward, bigSphere.forward) >= 164.5f)
        {
            h2Product.SetActive(true);
            originReactant.SetActive(false);
            var bulletObject = GameObject.FindGameObjectWithTag("Bullet");
            Destroy(bulletObject);
            if (SceneManager.GetActiveScene().name.Equals("Level_1"))
            {
                GameManager.Lv1_h2Win = true;
            }
            else if (SceneManager.GetActiveScene().name.Equals("Level_2"))
            {
                GameManager.Lv2_h2Win = true;
            }
            S2GameWinEvent?.Invoke();
            GetByProduct();
        }
    }

    private void GetByProduct()
    {
        byProduct_1.gameObject.SetActive(true);
        byProduct_2.gameObject.SetActive(true);
        byProduct_1.SetParent(null);
        byProduct_2.SetParent(null);
        var rb1 = byProduct_1.GetComponent<Rigidbody>();
        var rb2 = byProduct_2.GetComponent<Rigidbody>();
        rb1.constraints = RigidbodyConstraints.None;
        rb2.constraints = RigidbodyConstraints.None;
        rb1.velocity = new Vector3(-1, -1, 1) * 0.5f;
        rb2.velocity = new Vector3(1, 1, 1) * 0.5f;
    }
}
