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
    bool isMouseDragging;
    private Vector3 screenPosition;
    private Vector3 offset;
    private GameObject target;
    Vector3 startDragDir;
    Vector3 currentDragDir;
    Quaternion initialRotation;
    float angleFromStart;
    

    private void Awake()
    {
        _gameController = GameController.instance;
    }
    
    private void Update()
    {
        if(!isInInspectMode)
            return;
        if (Input.GetMouseButtonDown(0))
        {
            if (_insObject != null)
            {
                isMouseDragging = true;
                Debug.Log("our target position :" + _insObject.transform.position);
                //Here we Convert world position to screen position.
                screenPosition = Camera.main.WorldToScreenPoint(_insObject.transform.position);
                offset = _insObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPosition.z));
                // startDragDir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - target.transform.position;
                // initialRotation = target.transform.rotation;
            }
        }
    
        if (Input.GetMouseButtonUp(0))
        {
            isMouseDragging = false;
        }
    
        if (isMouseDragging)
        {
            _insGameObject.transform.Rotate((Input.GetAxis("Mouse Y") * _insObject.rotationSpeed * Time.deltaTime), (Input.GetAxis("Mouse X") * _insObject.rotationSpeed * Time.deltaTime), 0, Space.World);
        }
    
    }
    //
    //
    // private GameObject ReturnClickedObject(out RaycastHit hit)
    // {
    //     GameObject targetObject = null;
    //     Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    //     if (Physics.Raycast(ray.origin, ray.direction * 10, out hit))
    //     {
    //         targetObject = hit.collider.gameObject;
    //     }
    //     return targetObject;
    // }
    
    
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

    float rotationSpeed = 0.2f;

    
    
    // Vector3 startDragDir;
    // Vector3 currentDragDir;
    // Quaternion initialRotation;
    // float angleFromStart;
    // void OnMouseDown()
    // {
    //     startDragDir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - _insGameObject.transform.position;
    //     initialRotation = _insGameObject.transform.rotation;
    // }
    // void OnMouseDrag()
    // {
    //     if(!isInInspectMode)
    //         return;
    //     currentDragDir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - _insGameObject.transform.position;
    //     angleFromStart = Vector3.Angle(startDragDir, currentDragDir);
    //     _insGameObject.transform.rotation = initialRotation;
    //     _insGameObject.transform.Rotate(0.0f, angleFromStart*100, 0.0f);
    //     // _insGameObject.transform.eulerAngles = new Vector3(-angleFromStart.y, -angleFromStart.x);
    // }
    // public void OnMouseDrag()
    // {
    //     
    // }

    // public void OnDrag(PointerEventData eventData)
    // {
    //     if(isInInspectMode)
    //         _insGameObject.transform.eulerAngles = new Vector3(-eventData.delta.y, -eventData.delta.x);
    // }
    
    
    //////////// Drag Objects
    
    // private bool isMouseDragging;
    // private Vector3 screenPosition;
    // private Vector3 offset;
    // private GameObject target;
    //
    // GameObject ReturnClickedObject(out RaycastHit hit)
    // {
    //     GameObject targetObject = null;
    //     Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    //     if (Physics.Raycast(ray.origin, ray.direction * 10, out hit))
    //     {
    //         targetObject = hit.collider.gameObject;
    //     }
    //     return targetObject;
    // }
    //
    // void Update()
    // {
    //     if(!isInInspectMode)
    //         return;
    //     if (Input.GetMouseButtonDown(0))
    //     {
    //         RaycastHit hitInfo;
    //         target = ReturnClickedObject(out hitInfo);
    //         if (target != null)
    //         {
    //             isMouseDragging = true;
    //             Debug.Log("our target position :" + target.transform.position);
    //             //Here we Convert world position to screen position.
    //             screenPosition = Camera.main.WorldToScreenPoint(target.transform.position);
    //             offset = target.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPosition.z));
    //         }
    //     }
    //
    //     if (Input.GetMouseButtonUp(0))
    //     {
    //         isMouseDragging = false;
    //     }
    //
    //     if (isMouseDragging)
    //     {
    //         //tracking mouse position.
    //         Vector3 currentScreenSpace = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPosition.z);
    //
    //         //convert screen position to world position with offset changes.
    //         Vector3 currentPosition = Camera.main.ScreenToWorldPoint(currentScreenSpace) + offset;
    //
    //         //It will update target gameobject's current postion.
    //         target.transform.position = currentPosition;
    //     }
    //
    // }
    
}
