using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathologicalGames;

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
        ControlTime();
    }

    private void LateUpdate()
    {
        if (!GameManager.isWin)
        {
            mainCamera.localPosition = new Vector3(-0.986074448f, 1.85965788f, -2.61851311f);
        }
    }

    private void SpawnBullet()
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
            bulletClone.SetParent(null);
            var bullsEye = FindFirstObjectByType<BullsEye>().GetComponent<Transform>();
            bulletClone.LookAt(bullsEye.position);
            var bulletRb = bulletClone.GetComponent<Rigidbody>();
            bulletRb.velocity = (bullsEye.position - bulletClone.position).normalized * bulletVelocity;
            aimRay.SetActive(false);
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
        aimRay.SetActive(true);
        FindFirstObjectByType<GameManager>().SpawnBullsEye();
        RecycleBullets();
        missTextObject.SetActive(false);
        SpawnBullet();
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

    private void ControlTime()
    {
        if (Input.GetMouseButtonDown(1))
        {
            var bullsEye = FindFirstObjectByType<BullsEye>();
            bullsEye.FreezeResumeRotate();
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
