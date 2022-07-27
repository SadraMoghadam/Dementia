using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEditor.SceneManagement;
using UnityEngine;

public class CinemachinePOVExtension : CinemachineExtension
{
    [SerializeField] private float horizontalSpeed = 10;
    [SerializeField] private float verticalSpeed = 10;
    private float horizontalClampAngle = 70f;
    private float verticalClampAngle = 110f;
    private InputManager inputManager;
    private Vector3 startingRotation;

    protected override void Awake()
    {
        inputManager = InputManager.Instance;
        base.Awake();
    }

    protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        if (vcam.Follow)
        {
            if (stage == CinemachineCore.Stage.Aim)
            {
                if (startingRotation == null)
                {
                    startingRotation = transform.localRotation.eulerAngles;
                }
                Vector2 deltaInput = inputManager.GetMouseDelta();
                startingRotation.x += deltaInput.x * verticalSpeed * Time.deltaTime;
                startingRotation.y -= deltaInput.y * horizontalSpeed * Time.deltaTime;
                startingRotation.x = Mathf.Clamp(startingRotation.x, -verticalClampAngle, verticalClampAngle);
                startingRotation.y = Mathf.Clamp(startingRotation.y, -horizontalClampAngle, horizontalClampAngle);
                state.RawOrientation = Quaternion.Euler(startingRotation.y, startingRotation.x, 0); 
            }
        }
    }
}
