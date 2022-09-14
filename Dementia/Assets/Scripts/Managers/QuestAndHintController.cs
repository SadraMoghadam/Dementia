using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestAndHintController : MonoBehaviour
{
    [SerializeField] private QuestPanel questPanel;
    [SerializeField] private HintPanel hintPanel;
    public float duration = 1;
    public float hintShowDuration = 3;
    private GameController _gameController;

    private void Start()
    {
        _gameController = GameController.instance;
        questPanel.gameObject.SetActive(false);
    }

    public void ShowQuest(int id)
    {
        questPanel.gameObject.SetActive(true);
        string quest = _gameController.QuestDataReader.GetQuestData(id).Quest;
        questPanel.Show(quest);
    }
    
    public void ShowHint(int id)
    {
        hintPanel.gameObject.SetActive(true);
        string hint = _gameController.HintDataReader.GetHintData(id).Hint;
        hintPanel.Show(hint);
    }
    
}
