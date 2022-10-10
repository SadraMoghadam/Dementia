using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySightSensor : MonoBehaviour
{
    private bool _isFinallyEscaped;

    public void Awake()
    {
        _isFinallyEscaped = false;
    }

    public bool Ping()
    {
        EnemyUtility enemyUtility = EnemyUtility.Instance;
        Collider[] playerInRange = Physics.OverlapSphere(transform.position, enemyUtility.viewRadius, enemyUtility.playerMask);
 
        for (int i = 0; i < playerInRange.Length; i++)
        {
            Transform player = playerInRange[i].transform;
            Vector3 dirToPlayer = (player.position - transform.position).normalized;
            float dstToPlayer = Vector3.Distance(transform.position, player.position);
            if (dstToPlayer < 2)
            {
                return true;
            }
            else if (Vector3.Angle(transform.forward, dirToPlayer) < enemyUtility.viewAngle / 2)
            {
                if (!Physics.Raycast(transform.position, dirToPlayer, dstToPlayer, enemyUtility.obstacleMask))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            if (Vector3.Distance(transform.position, player.position) > enemyUtility.viewRadius)
            {
                return false;
            }
        }
        return false;
    }
    
    public bool Escaped(Transform playerTransform, Transform enemyTransform, float escapeDistance)
    {
        if (Vector3.Distance(playerTransform.position, enemyTransform.position) > escapeDistance)
        {
            return true;
        }
        return false;
    }

    public void ChangeEscapedState(bool state)
    {
        _isFinallyEscaped = state;
    }

    public bool IsFinallyEscaped()
    {
        return _isFinallyEscaped;
    }
    
}
