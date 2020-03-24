using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using GameSparks.Core;

public class SceneLoading : MonoBehaviour
{
    [Header ("Load scene")]
    public int SceneId;
    [Header ("Other objects")]
    public Image LoadingImg;
    public Text LoadingText;

    void Start()
    {
        StartCoroutine(AsyncLoad());
    }
    public IEnumerator AsyncLoad()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(SceneId);
        while (!operation.isDone)
        {
            float progress = operation.progress / 0.9f;
            LoadingImg.fillAmount = progress;
            LoadingText.text = string.Format("{0:0}%", progress * 100);
            yield return null;
        }
    }

    public void LoadGameScene()
    {
        SceneId = 2;
        StartCoroutine(AsyncLoad());
    }
    public void LoadMenuScene()
    {
        SceneId = 1;
        StartCoroutine(AsyncLoad());
    }

    public void LoadSignInScene()
    {
        GameSparks.Core.GS.Reset();
        SceneId = 0;
        StartCoroutine(AsyncLoad());
    }

    public void Exit()
    {
        Application.Quit();
    }

}
