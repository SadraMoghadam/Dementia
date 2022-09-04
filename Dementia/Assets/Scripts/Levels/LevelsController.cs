using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelsController : MonoBehaviour
{
    [HideInInspector] public List<GameObject> levels;
    private GameManager _gameManager;
    private int _currentLevel;

    private void Awake()
    {
        _gameManager = GameManager.instance;
        GetLevels();
        _currentLevel = _gameManager.playerPrefsManager.GetInt(PlayerPrefsKeys.Level, -1) + 1;
        SetLevelActive(_currentLevel);
    }

    private void GetLevels()
    {
        Transform[] levelChildren = GetComponentsInChildren<Transform>();
        for (int i = 0; i < levelChildren.Length; i++)
        {
            if (levelChildren[i].CompareTag("Level"))
            {
                levels.Add(levelChildren[i].gameObject);
            }
        }
    }
    
    public void SetLevelActive(int level)
    {
        for (int i = 0; i < levels.Count; i++)
        {
            if (i == level)
            {
                levels[i].SetActive(true);
            }
            else
            {
                levels[i].SetActive(false);
            }
        }
    }
    
}
