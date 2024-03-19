using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class CanvasManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI levelStartText;
    [SerializeField] private GameObject canvasLevel_4;
    [SerializeField] private GameObject bulleyes;

    // Start is called before the first frame update
    void Start()
    {
        levelStartText.DOFade(0, 1)
                .SetDelay(4)
                .OnComplete(() => levelStartText.gameObject.SetActive(false));

        if (SceneManager.GetActiveScene().name == "Level_4")
        {
            StartCoroutine(ShowLevel_4Canvas());
        }

    }

    private IEnumerator ShowLevel_4Canvas()
    {
        yield return new WaitForSeconds(5);
        canvasLevel_4.SetActive(true);
        bulleyes.SetActive(true);
    }
}
