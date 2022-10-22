using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InspectObjectProcess : MonoBehaviour
{
    public bool isInInspectMode = false;
    [SerializeField] private Transform cameraTransform;
    private Transform _mainObjectTransform;
    private InspectableObject _insObject;
    private GameObject _insGameObject;
    private GameObject _mainGameObject;
    private GameController _gameController;
    

    private void Awake()
    {
        _gameController = GameController.instance;
    }

    public void Inspect(GameObject gameObject)
    {
        isInInspectMode = true;
        UIController.instance.HideGUI();
        _mainGameObject = gameObject;
        _mainObjectTransform = _mainGameObject.transform;
        _mainGameObject.SetActive(false);
        _insObject = _mainGameObject.GetComponent<InspectableObject>();
        _gameController.DisableAllKeys();
        _gameController.ShowCursor();
        _insGameObject = Instantiate(_insObject.prefab, cameraTransform);
        _insGameObject.transform.localPosition = new Vector3(0, 0, .7f);
        _insGameObject.SetActive(true);
    }

    public void Close()
    {
        Destroy(_insGameObject);
        _mainGameObject.SetActive(false);
        _gameController.EnableAllKeys();
        _gameController.HideCursor();
        UIController.instance.ShowGUI();
    }

    
    Vector3 startDragDir;
    Vector3 currentDragDir;
    Quaternion initialRotation;
    float angleFromStart;
    void OnMouseDown()
    {
        startDragDir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - _insGameObject.transform.position;
        initialRotation = _insGameObject.transform.rotation;
    }
    void OnMouseDrag()
    {
        if(!isInInspectMode)
            return;
        currentDragDir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - _insGameObject.transform.position;
        angleFromStart = Vector3.Angle(startDragDir, currentDragDir);
        _insGameObject.transform.rotation = initialRotation;
        _insGameObject.transform.Rotate(0.0f, angleFromStart*100, 0.0f);
        // _insGameObject.transform.eulerAngles = new Vector3(-angleFromStart.y, -angleFromStart.x);
    }
    // public void OnMouseDrag()
    // {
    //     
    // }

    // public void OnDrag(PointerEventData eventData)
    // {
    //     if(isInInspectMode)
    //         _insGameObject.transform.eulerAngles = new Vector3(-eventData.delta.y, -eventData.delta.x);
    // }
    
}
