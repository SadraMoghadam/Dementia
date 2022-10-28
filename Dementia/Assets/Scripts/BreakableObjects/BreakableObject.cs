using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class BreakableObject : MonoBehaviour
{
    [SerializeField] private GameObject brokenPrefab;
    
    public void Break()
    {
        Transform originalTransform = this.transform; 
        Destroy(gameObject);
        GameObject brokenObj = Instantiate(brokenPrefab, originalTransform.position, originalTransform.rotation);
    }
    
}
