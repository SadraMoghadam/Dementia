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
        animator = transform.parent.GetComponent<Animator>();
    }

    public void ChangeDoorState()
    {
        try
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
        catch (Exception e)
        {
            Console.WriteLine("Door");
        }
    }
    
    public void ChangeDoorState(bool open)
    {
        animator.SetBool("Open", open);
        IsOpen = open;
    }

}
