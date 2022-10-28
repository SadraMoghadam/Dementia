using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenObject : MonoBehaviour
{
    [SerializeField] private GameObject forceSphere;
    [SerializeField] private float destroyTime = 3;
    private float _timer = 0;

    private void Update()
    {
        _timer += Time.deltaTime;
        if(_timer > .2f)
            Destroy(forceSphere);
        // if(_timer > destroyTime)
        //     Destroy(gameObject);
    }
}
