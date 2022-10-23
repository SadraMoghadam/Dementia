using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InspectPanel : MonoBehaviour
{
    [SerializeField] private Button closeButton;

    private void OnEnable()
    {
        closeButton.onClick.AddListener(OnCloseClicked);
    }

    private void OnCloseClicked()
    {
        GameController.instance.InspectObjectProcess.Close();
        gameObject.SetActive(false);
    }
    
}
