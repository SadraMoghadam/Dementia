using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpScareController : MonoBehaviour
{
    [SerializeField] private GameObject jumpScarePrefab;
    private GameController _gameController;
    private GameObject _jummpScareGameObject;

    private void Start()
    {
        _gameController = GameController.instance;
        // JSBehindPlayer(5);
    }

    public void SetJumpScare(float time = 0, bool sticked = false, float placementDegree = 180)
    {
        Transform playerTransform = _gameController.GetPlayerTransform();
        float sin = Mathf.Sin(Mathf.Deg2Rad * placementDegree);
        float cos = Mathf.Cos(Mathf.Deg2Rad * placementDegree);
        int right = Mathf.RoundToInt(sin);
        int forward = Mathf.RoundToInt(cos);
        Vector3 directionOfJS = Vector3.zero;
        if (right != 0)
        {
            directionOfJS += playerTransform.right * right;
        } 
        if (forward != 0)
        {
            directionOfJS += playerTransform.forward * forward;
        } 
        jumpScarePrefab.transform.position = playerTransform.position + directionOfJS;
        GameObject jumpScareGameObject = Instantiate(jumpScarePrefab);
        _jummpScareGameObject = jumpScareGameObject;
        LookAtPlayer();
        if (sticked)
        {
            jumpScareGameObject.transform.parent = _gameController.PlayerTransform;
        }

        if (time != 0)
        {
            Destroy(jumpScareGameObject, time);
        }
        // if (_jumpScareGameObject == null)
        // {
        //     _jumpScareGameObject = jumpScarePrefab;
        //     Destroy(Instantiate(jumpScarePrefab), time);   
        // }
    }

    public void LookAtPlayer()
    {
        Transform playerTransform = _gameController.GetPlayerTransform();
        _jummpScareGameObject.transform.LookAt(playerTransform);
    }

    
}
