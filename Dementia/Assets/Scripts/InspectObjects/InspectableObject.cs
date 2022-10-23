using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class InspectableObject : MonoBehaviour
{
    public int id;
    public string name;
    public GameObject prefab;
    public float rotationSpeed = 10;
    [HideInInspector] public Transform transform;
    private InspectObjectProcess _inspectObjectProcess;
    

    private void Awake()
    {
        transform = this.gameObject.transform;
        _inspectObjectProcess = GameController.instance.InspectObjectProcess;
    }

    // private void OnMouseDown()
    // {
    //     Debug.Log("AAAAAAAAAAAAAAAAAA");
    // }
    //
    // void OnMouseDrag()
    // {
    //     
    //     Debug.Log("OOOOOOOOOOOOOOOOOO");
    //     if(!_inspectObjectProcess.isInInspectMode)
    //         return;
    //     float XaxisRotation = Input.GetAxis("Mouse X") * rotationSpeed * Mathf.Deg2Rad;
    //     float YaxisRotation = Input.GetAxis("Mouse Y") * rotationSpeed * Mathf.Deg2Rad;
    //     transform.RotateAround(Vector3.down, XaxisRotation);
    //     transform.RotateAround(Vector3.right, YaxisRotation);
    // }
    //
    // public void OnDrag(PointerEventData eventData)
    // {
    //     // Pick one peice of code depending on Step 1 in the instructions and comment out the other. In this example we are using a cube so I commented out the sprite example
    //
    //     // For Game Objects: Use below to use this script for Game Objects like our cube because we convert where our mouse points to world coordinates with ScreenPointToRay()
    //     Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    //     transform.position = (new Vector3(ray.origin.x, ray.origin.y, 0));
    //
    //     // For UI Sprites: Uncomment below to use this script for UI objects like sprites, raw images, etc because we use the actual mouse input without converstion needed
    //     //transform.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
    //     // Warning: Adding a Canvas to the sprite object will make the object unmoveable.
    // }
    
}
