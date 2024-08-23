using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathologicalGames;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

public class Gun : MonoBehaviour
{
    public float bulletVelocity;
    public float chargeTime;
    public Transform gunMuzzle;

    [SerializeField] private SpawnPool pool;
    [SerializeField] private ProgressBarPro energyBar;
    [SerializeField] private Transform mainCamera;
    [SerializeField] private GameObject bullet;
    [SerializeField] private GameObject aimRay;
    [SerializeField] private GameObject missTextObject;
    [SerializeField] private GameObject winTextObject;
    [SerializeField] private AudioSource shootChanceSound;

    private bool isCharging;
    private float chargeTimer;
    private bool currentShootChance;
    private Transform bulletClone;

    [Header("Shake")] 
    public float shakeAmount;
    public  float shakeDuration;

    private readonly Vector3 cameraOriginPos = new Vector3(-0.986074448f, 1.85965788f, -2.61851311f);
    private float shakeTimer;
    private bool isShaking;
    
    private void Awake()
    {
        mainCamera.SetParent(transform);
        BullsEye.GameWinEvent += OnGameWin;
    }

    private void Start()
    {
        SpawnBullet();
    }

    private void Update()
    {
        Charge();
    }

    private void LateUpdate()
    {
        ShakeCamera();
        
        if (!GameManager.isWin && !isShaking)
        {
            mainCamera.localPosition = cameraOriginPos;
        }
    }
    
    private void ShakeCamera()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !GameManager.isShaked)
        {
            //isShaking = true;
        }
        
        if (isShaking)
        {
            mainCamera.localPosition = cameraOriginPos + Random.insideUnitSphere * shakeAmount;
            shakeTimer += Time.deltaTime;
            if (shakeTimer >= shakeDuration)
            {
                isShaking = false;
                shakeTimer = 0;
            }
        } 
    }

    public void SpawnBullet()
    {
        bulletClone = pool.Spawn(bullet, transform);
        bulletClone.localPosition = gunMuzzle.localPosition;
        bulletClone.localRotation = Quaternion.Euler(-10f, 0, 0);
        var bulletRb = bulletClone.GetComponent<Rigidbody>();
        bulletRb.velocity = Vector3.zero;
    }

    public void Shoot()
    {
        if (!isCharging && !GameManager.isWin)
        {
            isCharging = true;
            bulletClone.SetParent(null);
            var bullsEye = GameObject.FindGameObjectWithTag("BullsEye").transform;
            bulletClone.LookAt(bullsEye.position);
            var bulletRb = bulletClone.GetComponent<Rigidbody>();
            bulletRb.velocity = (bullsEye.position - bulletClone.position).normalized * bulletVelocity;
            aimRay.SetActive(false);
            var reactantRb = GameObject.FindGameObjectWithTag("Reactant").GetComponent<Rigidbody>();
            reactantRb.constraints = RigidbodyConstraints.None;
        }
    }

    public void RemindShootChance(bool isShootChance)
    {
        if (isShootChance != currentShootChance)
        {
            if (currentShootChance == false && !GameManager.isWin)
            {
                shootChanceSound.Play();
            }
            currentShootChance = isShootChance;
        }
        aimRay.GetComponent<LineRenderer>().startColor = isShootChance ? Color.yellow : Color.red;
        aimRay.GetComponent<LineRenderer>().endColor = isShootChance ? Color.yellow : Color.red;
    }

    public void Restart()
    {
        StartCoroutine(ResetObjects());
    }

    private IEnumerator ResetObjects()
    {
        isCharging = true;
        missTextObject.SetActive(true);
        yield return new WaitForSeconds(chargeTime);
        var gameManager = FindFirstObjectByType<GameManager>();
        gameManager.ResetGame();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void OnGameWin()
    {
        winTextObject.SetActive(true);
    }

    private void Charge()
    {
        if (isCharging)
        {
            chargeTimer += Time.deltaTime;
            energyBar.SetValue(chargeTimer, chargeTime);
            if (chargeTimer >= chargeTime)
            {
                chargeTimer = 0;
                isCharging = false;
            }
        }
    }

    public void RecyicleBullet(Transform outBullet)
    {
        pool.Despawn(outBullet);
    }

    public void RecycleBullets()
    {
        var bullets = GameObject.FindGameObjectsWithTag("Bullet");
        foreach (var bullet in bullets)
        {
            RecyicleBullet(bullet.GetComponent<Transform>());
        }
    }

    private void OnDisable()
    {
        BullsEye.GameWinEvent -= OnGameWin;
    }
}
