using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MutantFootsteps : MonoBehaviour
{
    private AudioManager _audioManager;
    
    
    private void OnEnable()
    {
        _audioManager = GameManager.instance.AudioManager;
    }

    private void OnTriggerEnter(Collider other)
    {
        _audioManager.play("FootStep");
    }
}
