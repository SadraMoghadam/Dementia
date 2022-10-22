using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InspectableObject : MonoBehaviour
{
    public int id;
    public string name;
    public GameObject prefab;
    [HideInInspector] public Transform transform;

    private void Awake()
    {
        transform = this.gameObject.transform;
    }
}
