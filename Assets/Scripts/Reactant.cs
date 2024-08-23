using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            return;
        }
        
        if (smallSphereName.Equals("h2") && 
            Vector3.Angle(h2Sphere.forward, bigSphere.forward) <= 177 && 
            Vector3.Angle(h2Sphere.forward, bigSphere.forward) >= 167)
        {
            h2Product.SetActive(true);
            originReactant.SetActive(false);
            return;
        }

        incorrectSound.Play();
        StartCoroutine(MissShoot());
    }

    private IEnumerator MissShoot()
    {
        yield return new WaitForSeconds(2);
        FindFirstObjectByType<GameManager>().ResetGame();
    }
}
