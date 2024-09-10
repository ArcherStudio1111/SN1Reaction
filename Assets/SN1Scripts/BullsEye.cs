using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using Random = UnityEngine.Random;

public class BullsEye : MonoBehaviour
{
    public float shakeRange = -0.075f;
    public float shakeTime;
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
    public Transform bigSphere;
    public Transform bulletEgg;

    private Vector3 randomVector3;
    private float moveFasterPara = 5f;
    private Gun gun;
    private float gunAngle;
    private bool isFreezing;

    private void Start()
    {
        MoveLoop();
    }

    private void OnEnable()
    {
        gun = FindFirstObjectByType<Gun>();
        SetVelocities();
    }

    private void MoveLoop()
    {
        cube.DOLocalMoveY(0.0715f, shakeTime + 0.2f).SetLoops(-1, LoopType.Yoyo);
        smallSphere.DOLocalMove(new Vector3(0.071f, -0.0438f, 0.0233f), shakeTime + 0.4f).SetLoops(-1, LoopType.Yoyo);
        triangle.DOLocalMove(new Vector3(-0.056f, -0.05f, 0.02f), shakeTime + 0.6f).SetLoops(-1, LoopType.Yoyo);
        bigSphere.DOLocalMoveZ(shakeRange, shakeTime).SetLoops(-1, LoopType.Yoyo);
        bulletEgg.DOLocalMoveZ(0.0692f, shakeTime + 0.8f).SetLoops(-1, LoopType.Yoyo);
    }
    
    private void Update()
    {
        DetectGun();
        ChangeAngularVelocities();

        if (Input.GetMouseButtonDown(1))
        {
            FreezeResumeRotate();
        }
        
        if (GameManager.isShaked)
        {
            bigSphere.transform.SetParent(null);
            bigSphere.transform.Translate(new Vector3(0, -1, -1) * Time.deltaTime * 1f);
        }
        
        if (GameManager.isWin)
        {
            afterProduct.transform.SetParent(null);
            afterProduct.transform.Translate(new Vector3(0, -1, -1) * Time.deltaTime * 1f);
        }
    }

    private void DetectGun()
    {
        if (SceneManager.GetActiveScene().name == "Level_1")
        {
            return;
        }
        
        if (gun != null)
        {
            gunAngle = Vector3.Angle(gun.gunMuzzle.forward, transform.forward);
            gun.RemindShootChance(gunAngle >= 187.5f - winTolerant || (gunAngle <= winTolerant - 5f && gunAngle >= 5f));
        }
    }

    private void SetVelocities()
    {
        rb.velocity = Vector3.zero;

        switch (SceneManager.GetActiveScene().name)
        {
            /*
            case "Level_1":
                rb.angularVelocity = Vector3.forward * rotateVelocity;
                break;
            case "Level_2":
                rb.angularVelocity = Vector3.up * rotateVelocity;
                break;
                */
            default:
                randomVector3 = new Vector3(Random.Range(-180f, 180f), Random.Range(-180f, 180f), Random.Range(-180f, 180f));
                //rb.angularVelocity = RandomVector3.normalized * rotateVelocity;
                break;
        }
    }

    int zeroPos = 1;
    float angularVelocityChangeTimer;
    private float angularVelocityChangeTime = 2.5f;

    private void ChangeAngularVelocities()
    {
        if (isFreezing)
        {
            return;
        }
        switch (SceneManager.GetActiveScene().name)
        {
            /*
            case "Level_1":
                break;
            case "Level_2":
                break;
                */
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
        isFreezing = !isFreezing;
        rb.angularVelocity = rb.angularVelocity == Vector3.zero ? randomVector3.normalized * rotateVelocity : Vector3.zero;
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
                cube.localRotation = Quaternion.Euler(-19.5f, 0, 0);
                triangle.localRotation = Quaternion.Euler(0, -19.5f, 0);
                smallSphere.localRotation = Quaternion.Euler(0, 19.5f, 0);
            }
            else if(bulletAngle <= winTolerant && bulletAngle >=0)
            {
                bulletPart.transform.localRotation = Quaternion.Euler(new Vector3(0, 180, 0));
                bulletPart.transform.localPosition = new Vector3(0, 0, -0.02f);
                bulletPart.transform.DOKill();
                GameWin();
                cube.localRotation = Quaternion.Euler(19.5f, 0, 0);
                triangle.localRotation = Quaternion.Euler(0, 19.5f, 0);
                smallSphere.localRotation = Quaternion.Euler(0, -19.5f, 0);
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
        //FindFirstObjectByType<Gun>().RecycleBullets();
        StartCoroutine(ShortPauseGame());
    }

    private IEnumerator ShortPauseGame()
    {
        Time.timeScale = 0.33f;
        yield return new WaitForSeconds(4 * Time.timeScale);
        GameWinEvent?.Invoke();
    }

    public void ShakeOffBigSphere()
    {
        bigSphere.DOKill(); 
        bigSphere.SetParent(null);
        smallSphere.localRotation = Quaternion.identity;
        triangle.localRotation = Quaternion.identity;
        cube.localRotation = Quaternion.identity;
    }
}
