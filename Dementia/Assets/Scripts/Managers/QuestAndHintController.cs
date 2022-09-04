using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestAndHintController : MonoBehaviour
{
    [SerializeField] private QuestPanel questPanel;
    [SerializeField] private HintPanel hintPanel;
    public float duration = 2;
    public float hintShowDuration = 5;
    private GameController _gameController;

    private void Start()
    {
        _gameController = GameController.instance;
        questPanel.gameObject.SetActive(false);
    }

    public void ShowQuest(int id)
    {
        questPanel.gameObject.SetActive(true);
        questPanel.Show("Find the flashlight");
    }
    
    public void ShowHint(int id)
    {
        hintPanel.gameObject.SetActive(true);
        hintPanel.Show("This is a Hint");
    }
    
}
