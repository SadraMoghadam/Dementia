using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // [HideInInspector] public GameSetting gameSetting;
    [HideInInspector] public PlayerPrefsManager playerPrefsManager;
    [HideInInspector] public AudioManager AudioManager;
    
    public static GameManager instance;
    private void Awake()
    {
        // Application.targetFrameRate = 60;
        if (instance != null)
        {
            Destroy(gameObject);
        }
        GameManager[] gameManagers = FindObjectsOfType<GameManager>();
        if(gameManagers.Length > 1)
        {
            for (int i = 0; i < gameManagers.Length - 1; i++)
            {
                Destroy(gameManagers[i].gameObject);
            }
        }
        // gameSetting = GetComponent<GameSetting>();
        playerPrefsManager = GetComponent<PlayerPrefsManager>();
        AudioManager = GetComponent<AudioManager>();
        DontDestroyOnLoad(this.gameObject);
        instance = this;
    }

    public async void LoadScene(string sceneName)
    {
        var scene = SceneManager.LoadSceneAsync(sceneName);
        SceneManager.LoadScene("Loading");
        scene.allowSceneActivation = false;
        await Task.Delay(200);
        var slider = FindObjectOfType<Slider>();
        do
        {
            await Task.Delay(100);
            slider.value = scene.progress;
        } while (scene.progress < 0.9f);

        await Task.Delay(1000);
        scene.allowSceneActivation = true;
        SceneManager.LoadScene(sceneName);
    }
    
}

