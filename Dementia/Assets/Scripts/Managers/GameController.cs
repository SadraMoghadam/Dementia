using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public enum Places
{
    HallwayFirstFloor,
    KitchenFirstFloor,
    DiningRoomFirstFloor,
    BathRoomFirstFloor,
    Park,
    PatientRoomFirstFloor
}

public class GameController : MonoBehaviour
{
    public DamageController DamageController;
    public StaminaController StaminaController;
    public PlayerController PlayerController;
    public LevelsController LevelsController;
    public InteractableItemsScriptableObject InteractableItemsScriptableObject;
    [HideInInspector] public Inventory Inventory;
    [HideInInspector] public FlashlightController FlashlightController;
    [HideInInspector] public LightsController LightsController;
    [HideInInspector] public JumpScareController JumpScareController;
    [HideInInspector] public QuestAndHintController QuestAndHintController;
    [HideInInspector] public Transform PlayerTransform;
    [HideInInspector] public bool KeysDisabled;

    [HideInInspector]
    public bool isInInventory;

    public static GameController instance;
    private GameManager _gameManager;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        
        _gameManager = GameManager.instance;
        SavedData savedData = _gameManager.playerPrefsManager.LoadGame();
        PlayerController.transform.position = savedData.playerTransform.position;
        PlayerController.transform.rotation = savedData.playerTransform.rotation;
        Inventory = GetComponent<Inventory>();
        FlashlightController = GetComponent<FlashlightController>();
        LightsController = GetComponent<LightsController>();
        JumpScareController = GetComponent<JumpScareController>();
        QuestAndHintController = GetComponent<QuestAndHintController>();
        PlayerTransform = PlayerController.transform;
        isInInventory = false;
        Time.timeScale = 1;
        KeysDisabled = false;
        LightsController.TurnAllLightsOnOrOff(_gameManager.playerPrefsManager.GetBool(PlayerPrefsKeys.LightsEnabled, true));
        
    }

    public void HideCursor()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    
    public void ShowCursor()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public Transform GetPlayerTransform()
    {
        PlayerTransform = PlayerController.transform;
        return PlayerTransform;
    }
    
    public void SetPlayerTransform(Transform transform)
    {
        PlayerController.transform.position = transform.position;
        PlayerController.transform.rotation = transform.rotation;
    }

    public void DisableAllKeys()
    {
        KeysDisabled = true;
    }
    
    public void EnableAllKeys()
    {
        KeysDisabled = false;
    }

    public IEnumerator FadeInAndOut(GameObject objectToFade, bool fadeIn, float duration, float finalOpacity = 1)
    {
        float counter = 0f;

        //Set Values depending on if fadeIn or fadeOut
        float a, b;
        if (fadeIn)
        {
            a = 0;
            b = finalOpacity;
        }
        else
        {
            a = finalOpacity;
            b = 0;
        }

        Color currentColor = Color.clear;

        SpriteRenderer tempSPRenderer = objectToFade.GetComponentInChildren<SpriteRenderer>();
        Image tempImage = objectToFade.GetComponentInChildren<Image>();
        RawImage tempRawImage = objectToFade.GetComponentInChildren<RawImage>();
        MeshRenderer tempRenderer = objectToFade.GetComponentInChildren<MeshRenderer>();
        TMP_Text tempText = objectToFade.GetComponentInChildren<TMP_Text>();

        // //Check if this is a Sprite
        // if (tempSPRenderer != null)
        // {
        //     currentColor = tempSPRenderer.color;
        //     mode = 0;
        // }
        // //Check if Image
        // else if (tempImage != null)
        // {
        //     currentColor = tempImage.color;
        //     mode = 1;
        // }
        // //Check if RawImage
        // else if (tempRawImage != null)
        // {
        //     currentColor = tempRawImage.color;
        //     mode = 2;
        // }
        // //Check if Text 
        // else if (tempText != null)
        // {
        //     currentColor = tempText.color;
        //     mode = 3;
        // }
        //
        // //Check if 3D Object
        // else if (tempRenderer != null)
        // {
        //     currentColor = tempRenderer.material.color;
        //     mode = 4;
        //
        //     //ENABLE FADE Mode on the material if not done already
        //     tempRenderer.material.SetFloat("_Mode", 2);
        //     tempRenderer.material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        //     tempRenderer.material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        //     tempRenderer.material.SetInt("_ZWrite", 0);
        //     tempRenderer.material.DisableKeyword("_ALPHATEST_ON");
        //     tempRenderer.material.EnableKeyword("_ALPHABLEND_ON");
        //     tempRenderer.material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        //     tempRenderer.material.renderQueue = 3000;
        // }
        // else
        // {
        //     yield break;
        // }

        while (counter < duration)
        {
            counter += Time.deltaTime;
            float alpha = Mathf.Lerp(a, b, counter / duration);

            if (tempSPRenderer != null)
            {
                currentColor = tempSPRenderer.color;
                tempSPRenderer.color = new Color(currentColor.r, currentColor.g, currentColor.b, alpha);
            }
            
            if (tempImage != null)
            {
                currentColor = tempImage.color;
                tempImage.color = new Color(currentColor.r, currentColor.g, currentColor.b, alpha);
            }
            
            if (tempRawImage != null)
            {
                currentColor = tempRawImage.color;
                tempRawImage.color = new Color(currentColor.r, currentColor.g, currentColor.b, alpha);
            }
            
            if (tempText != null)
            {
                currentColor = tempText.color;
                tempText.color = new Color(currentColor.r, currentColor.g, currentColor.b, alpha);
            }
            
            if (tempRenderer != null)
            {
                currentColor = tempRenderer.material.color;
                tempRenderer.material.color = new Color(currentColor.r, currentColor.g, currentColor.b, alpha);
            }

            yield return null;
        }
    }
}
