using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [HideInInspector] public bool IsOpen;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void ChangeDoorState()
    {
        if (!IsOpen)
        {
            animator.SetBool("Open", true);
            IsOpen = true;
        }
        else
        {
            animator.SetBool("Open", false);
            IsOpen = false;
        }
    }
    
    public void ChangeDoorState(bool open)
    {
        animator.SetBool("Open", open);
        IsOpen = open;
    }

}
