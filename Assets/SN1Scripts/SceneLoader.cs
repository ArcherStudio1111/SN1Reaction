using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void ChangeScene(string sceneName)
    {
        if (SceneManager.GetActiveScene().name.Equals("MainMenu"))
        {
            GameManager.Lv1_h1Win = false;
            GameManager.Lv1_h2Win = false;
            GameManager.Lv2_h1Win = false;
            GameManager.Lv2_h2Win = false;
            SceneManager.LoadScene("Level_1");
        }
        else if (SceneManager.GetActiveScene().name.Equals("Level_1") && GameManager.Lv1_h1Win && GameManager.Lv1_h2Win)
        {
            SceneManager.LoadScene("Level_2");
        }
        else if (SceneManager.GetActiveScene().name.Equals("Level_2") && GameManager.Lv2_h1Win && GameManager.Lv2_h2Win)
        {
            SceneManager.LoadScene("MainMenu");
            GameManager.Lv1_h1Win = false;
            GameManager.Lv1_h2Win = false;
            GameManager.Lv2_h1Win = false;
            GameManager.Lv2_h2Win = false;
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
