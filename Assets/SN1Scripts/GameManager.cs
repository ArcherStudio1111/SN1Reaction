using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using PathologicalGames;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public float shakeTolerate;
    public static bool isShaked;
    public static bool isWin;
    
    [SerializeField] private TextMeshProUGUI playTimeText;
    [SerializeField] private TextMeshProUGUI endTimeText;
    [SerializeField] private GameObject shakeText;
    [SerializeField] private GameObject settingPanel;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private SpawnPool spawnPool;

    private float playTime;
    private Transform spawnedBullsEye;
    
    private void Awake()
    {
        BullsEye.GameWinEvent += OnGameWin;
    }

    private void Start()
    {
        SpawnBullsEye();
    }

    private void Update()
    {
        CountTime();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            settingPanel.SetActive(true);
            PauseGame();
        }
        
        if (Input.GetKeyDown(KeyCode.Space) && !isShaked)
        {
            var bullsEyeScript = spawnedBullsEye.GetComponent<BullsEye>();
            if (bullsEyeScript.bigSphere.transform.localPosition.z <= bullsEyeScript.shakeRange - shakeTolerate)
            {
                isShaked = true;
                shakeText.SetActive(false);
                bullsEyeScript.ShakeOffBigSphere();
                OnGameWin();
            }
        }
    }
    

    public Transform SpawnBullsEye()
    {
        if (spawnedBullsEye != null)
        {
            spawnPool.Despawn(spawnedBullsEye);
        }

        var sceneName = SceneManager.GetActiveScene().name;
        if (sceneName == "Level_1")
        {
            spawnedBullsEye = spawnPool.Spawn("BullsEye");
        }
        else if(sceneName == "Level_2")
        {
            spawnedBullsEye = spawnPool.Spawn("PartBullsEye");
        }
        return spawnedBullsEye;
    }

    private void CountTime()
    {
        playTime += Time.deltaTime;
        var formattedPlayTime = TimeSpan.FromSeconds(playTime);
        playTimeText.text = formattedPlayTime.ToString("mm':'ss");
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
    }

    public void ResetGame()
    {
        ResumeGame();
        isWin = false;
        isShaked = false;
    }

    private void OnGameWin()
    {
        isWin = true;
        Time.timeScale = 0.33f;
        StartCoroutine(OpenWinPanel());
    }

    private IEnumerator OpenWinPanel()
    {
        yield return new WaitForSeconds(1.3f);
        PauseGame();
        endTimeText.text = playTimeText.text;
        winPanel.SetActive(true);
    }

    private void OnDisable()
    {
        BullsEye.GameWinEvent -= OnGameWin;
    }
}
