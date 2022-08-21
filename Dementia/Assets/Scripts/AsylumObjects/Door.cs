using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Door : MonoBehaviour
{
    [HideInInspector] public bool IsOpen;
    private Animator animator;
    private NavMeshObstacle _navMeshObstacle;

    private void Awake()
    {
        animator = transform.parent.GetComponent<Animator>();
        _navMeshObstacle = GetComponent<NavMeshObstacle>();
    }

    public void ChangeDoorState()
    {
        try
        {
            if (!IsOpen)
            {
                animator.SetBool("Open", true);
                IsOpen = true;
                StartCoroutine(NavMeshObstacleCarving(true));
            }
            else
            {
                animator.SetBool("Open", false);
                IsOpen = false;
                StartCoroutine(NavMeshObstacleCarving(false));
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
        StartCoroutine(NavMeshObstacleCarving(open));
    }

    private IEnumerator NavMeshObstacleCarving(bool carve)
    {
        yield return new WaitForSeconds(4f);
        _navMeshObstacle.carving = carve;
        StopCoroutine(NavMeshObstacleCarving(carve));
    }

}
