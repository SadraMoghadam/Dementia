using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestPanel : MonoBehaviour
{
    [SerializeField] private GameObject quest;
    [SerializeField] private TMP_Text text;
    private float finalOpacity = .9f;

    public void Show(string questString)
    {
        GameController gameController = GameController.instance;
        text.text = questString;
        float duration = gameController.QuestAndHintController.duration;
        StartCoroutine(gameController.FadeInAndOut(quest, true, duration, finalOpacity));
    }
    
    public void ShowNext(string questString)
    {
        GameController gameController = GameController.instance;
        text.text = questString;
        StartCoroutine(Fade());
    }

    private IEnumerator Fade()
    {
        GameController gameController = GameController.instance;
        float duration = gameController.QuestAndHintController.duration;
        StartCoroutine(gameController.FadeInAndOut(quest, false, duration, finalOpacity));
        yield return new WaitForSeconds(duration);
        StartCoroutine(gameController.FadeInAndOut(quest, true, duration, finalOpacity));
    }
    
}
