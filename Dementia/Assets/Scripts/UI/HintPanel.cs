using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HintPanel : MonoBehaviour
{
    [SerializeField] private GameObject hint;
    [SerializeField] private TMP_Text text;
    private float finalOpacity = .9f;

    public void Show(string hintString)
    {
        GameController gameController = GameController.instance;
        text.text = hintString;
        float duration = gameController.QuestAndHintController.duration;
        StartCoroutine(Fade());
    }

    private IEnumerator Fade()
    {
        GameController gameController = GameController.instance;
        float duration = gameController.QuestAndHintController.duration;
        float hintShowDuration = gameController.QuestAndHintController.hintShowDuration;
        StartCoroutine(gameController.FadeInAndOut(hint, true, duration, finalOpacity));
        yield return new WaitForSeconds(duration + hintShowDuration);
        StartCoroutine(gameController.FadeInAndOut(hint, false, duration, finalOpacity));
    }
    
}
