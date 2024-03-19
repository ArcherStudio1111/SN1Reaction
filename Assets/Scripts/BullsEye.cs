using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class BullsEye : MonoBehaviour
{
    public float winTolerant;
    public float rotateVelocity;

    public static event Action GameWinEvent;

    [SerializeField] private Rigidbody rb;
    [SerializeField] private GameObject bulletPart;
    [SerializeField] private GameObject afterProduct;
    [SerializeField] private AudioSource incorrectSound;

    [Header("Self Atoms")]
    [SerializeField] private Transform cube;
    [SerializeField] private Transform smallSphere;
    [SerializeField] private Transform triangle;

    private Vector3 RandomVector3;
    private float moveFasterPara = 5f;
    private Gun gun;
    private float gunAngle;
    private Transform mainCamera;

    private void Awake()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").transform;
    }

    private void OnEnable()
    {
        gun = FindFirstObjectByType<Gun>();
        SetVelocities();
    }

    private void Update()
    {
        DetectGun();
        ChangeAngularVelocities();
        if (GameManager.isWin)
        {
            afterProduct.transform.SetParent(null);
            afterProduct.transform.Translate(new Vector3(0, -1, -1) * Time.deltaTime * 1f);
        }
    }

    private void DetectGun()
    {
        if (gun != null)
        {
            gunAngle = Vector3.Angle(gun.gunMuzzle.forward, transform.forward);
            gun.RemindShootChance(gunAngle >= 187.5f - winTolerant);
        }
    }

    private void SetVelocities()
    {
        rb.velocity = Vector3.zero;

        switch (SceneManager.GetActiveScene().name)
        {
            case "Level_1":
                rb.angularVelocity = Vector3.forward * rotateVelocity;
                break;
            case "Level_2":
                rb.angularVelocity = Vector3.up * rotateVelocity;
                break;
            default:
                RandomVector3 = new Vector3(Random.Range(-180f, 180f), Random.Range(-180f, 180f), Random.Range(-180f, 180f));
                //rb.angularVelocity = RandomVector3.normalized * rotateVelocity;
                break;
        }
    }

    int zeroPos = 1;
    float angularVelocityChangeTimer;
    private float angularVelocityChangeTime = 2.5f;

    private void ChangeAngularVelocities()
    {
        switch (SceneManager.GetActiveScene().name)
        {
            case "Level_1":
                break;
            case "Level_2":
                break;
            default:
                switch (zeroPos)
                { 
                    case 1:
                        rb.angularVelocity = new Vector3(1, 0, 1).normalized * rotateVelocity;

                        angularVelocityChangeTimer += Time.deltaTime;
                        if (angularVelocityChangeTimer >= angularVelocityChangeTime)
                        {
                            angularVelocityChangeTimer = 0;
                            zeroPos = 2;
                        }
                        break;
                    case 2:
                        rb.angularVelocity = new Vector3(0, 1, 1).normalized * rotateVelocity;

                        angularVelocityChangeTimer += Time.deltaTime;
                        if (angularVelocityChangeTimer >= angularVelocityChangeTime)
                        {
                            angularVelocityChangeTimer = 0;
                            zeroPos = 3;
                        }
                        break;
                    case 3:
                        rb.angularVelocity = new Vector3(-1, 0, 1).normalized * rotateVelocity;

                        angularVelocityChangeTimer += Time.deltaTime;
                        if (angularVelocityChangeTimer >= angularVelocityChangeTime)
                        {
                            angularVelocityChangeTimer = 0;
                            zeroPos = 4;
                        }
                        break;
                    case 4:
                        rb.angularVelocity = new Vector3(0, 1, 1).normalized * rotateVelocity;

                        angularVelocityChangeTimer += Time.deltaTime;
                        if (angularVelocityChangeTimer >= angularVelocityChangeTime)
                        {
                            angularVelocityChangeTimer = 0;
                            zeroPos = 1;
                        }
                        break;
                }
                break;
        }
    }

    public void FreezeResumeRotate()
    {
        if (SceneManager.GetActiveScene().name != "Level_3")
        {
            rb.angularVelocity = rb.angularVelocity == Vector3.zero ? RandomVector3.normalized * rotateVelocity : Vector3.zero;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            MoveFaster();
            var bulletAngle = Vector3.Angle(collision.transform.forward, transform.forward);
            if (bulletAngle >= 180 - winTolerant)
            {
                GameWin();
            }
            else
            {
                incorrectSound.Play();
                gun.Restart();
            }
        }
    }

    private void MoveFaster()
    {
        rb.velocity *= moveFasterPara;
        rb.angularVelocity *= moveFasterPara;
    }

    private void GameWin()
    {
        bulletPart.SetActive(true);
        FindFirstObjectByType<Gun>().RecycleBullets();
        cube.localRotation = Quaternion.Euler(-19.5f, 0, 0);
        triangle.localRotation = Quaternion.Euler(0, -19.5f, 0);
        smallSphere.localRotation = Quaternion.Euler(0, 19.5f, 0);
        StartCoroutine(ShortPauseGame());
    }

    private IEnumerator ShortPauseGame()
    {
        Time.timeScale = 0.33f;
        yield return new WaitForSeconds(4 * Time.timeScale);
        GameWinEvent?.Invoke();
    }
}
