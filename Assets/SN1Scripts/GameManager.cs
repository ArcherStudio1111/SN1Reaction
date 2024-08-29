using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using PathologicalGames;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public float shakeTolerate;
    public static bool isShaked;
    public static bool isWin;
    public static bool Lv1_h1Win;
    public static bool Lv1_h2Win;
    public static bool Lv2_h1Win;
    public static bool Lv2_h2Win;

    [SerializeField] private TextMeshProUGUI playTimeText;
    [SerializeField] private TextMeshProUGUI endTimeText;
    [SerializeField] private GameObject shakeText;
    [SerializeField] private GameObject settingPanel;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private SpawnPool spawnPool;
    [SerializeField] private Image lv1_Star1;
    [SerializeField] private Image lv1_Star2;
    [SerializeField] private Image lv2_Star1;
    [SerializeField] private Image lv2_Star2;

    private float playTime;
    private Transform spawnedBullsEye;
    
    private void Awake()
    {
        BullsEye.GameWinEvent += OnGameWin;
        Reactant.S2GameWinEvent += OnGameWin;
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
        
        /*if (Input.GetKeyDown(KeyCode.Space) && !isShaked)
        {
            var bullsEyeScript = spawnedBullsEye.GetComponent<BullsEye>();
            if (bullsEyeScript.bigSphere.transform.localPosition.z <= bullsEyeScript.shakeRange - shakeTolerate)
            {
                isShaked = true;
                shakeText.SetActive(false);
                bullsEyeScript.ShakeOffBigSphere();
                OnGameWin();
            }
        }*/
    }
    

    public Transform SpawnBullsEye()
    {
        if (spawnedBullsEye != null)
        {
            spawnPool.Despawn(spawnedBullsEye);
        }
        
        spawnedBullsEye = spawnPool.Spawn("Reactant",
            new Vector3(0.00627562404f, 0.0124644525f, -0.00520026684f),
            Quaternion.Euler(new Vector3(68.3502197f, 26.4641685f, 358.338013f)));
        
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
        yield return new WaitForSeconds(1.33f);
        //PauseGame();
        endTimeText.text = playTimeText.text;
        winPanel.SetActive(true);
        if (SceneManager.GetActiveScene().name.Equals("Level_1"))
        {
            lv1_Star1.color = Lv1_h1Win ? Color.white : new Color32(47, 47, 47, 255);
            lv1_Star2.color = Lv1_h2Win ? Color.white : new Color32(47, 47, 47, 255);
        }

        if (SceneManager.GetActiveScene().name.Equals("Level_2"))
        {
            lv2_Star1.color = Lv2_h1Win ? Color.white : new Color32(47, 47, 47, 255);
            lv2_Star2.color = Lv2_h2Win ? Color.white : new Color32(47, 47, 47, 255);
        }
    }

    private void OnDisable()
    {
        BullsEye.GameWinEvent -= OnGameWin;
        Reactant.S2GameWinEvent -= OnGameWin;
    }
}
