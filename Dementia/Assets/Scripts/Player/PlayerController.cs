using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using Random = UnityEngine.Random;


public class PlayerController : MonoBehaviour
{
    [HideInInspector] public Animator animator;
    [HideInInspector] public bool isStopped;
    
    [SerializeField] private float animBlendSpeed = 8.9f;
    [SerializeField] private Transform cameraRoot;
    [SerializeField] private Transform camera;
    [SerializeField] private GameObject stickyCamera;
    [SerializeField] private List<Collider> cutsceneColliders;

    [Space(5)]
    [SerializeField] private float upperLimit = -40f;
    [SerializeField] private float bottomLimit = 70f;
    [SerializeField] private float mouseSensitivity = 20f;
    
    [Space(5)]
    [SerializeField] private float _walkSpeed = 3f;
    [SerializeField] private float _runSpeed = 7f;
    
    [Space(5)]
    [SerializeField] private PostProcessVolume postProcessVolumeMainCam;
    [SerializeField] private PostProcessVolume postProcessVolumeStickyCam;
    
    [Header("Controller Adjustments")]
    [SerializeField] private float normalHeight = 2.0f;
    [SerializeField] private float crouchHeight = 1.4f;
    
    [Space(5)]
    [SerializeField] private Vector3 normalCenter = new Vector3(0, 1f, 0);
    [SerializeField] private Vector3 crouchCenter = new Vector3(0, .5f, 0);
    
    
    private Rigidbody _playerRigidbody;
    private CapsuleCollider _collider;
    private InputManager _inputManager;
    private bool _grounded = false;
    private bool _hasAnimator;
    private int _xVelocityHash;
    private int _yVelocityHash;
    private float _xRotation;
    private Vector2 _currentVelocity;
    private GameController _gameController;
    private GameManager _gameManager;
    private UIController _uiController;
    private float _medkitTimer;
    private float _medkitNotificationTime = 20;
    private float _keyTimer;
    private float _keyNotificationTime = 5 * 60;
    
    
        
    private void Awake() 
    {
        _hasAnimator = TryGetComponent<Animator>(out animator);
        _playerRigidbody = GetComponent<Rigidbody>();
        _collider = GetComponent<CapsuleCollider>();
        _inputManager = GetComponent<InputManager>();
        _xVelocityHash = Animator.StringToHash("XVelocity");
        _yVelocityHash = Animator.StringToHash("YVelocity");
        _gameController = GameController.instance;
        _gameManager = GameManager.instance;
        _uiController = UIController.instance;
        _collider.enabled = true;
        StartCoroutine(CutSceneColliderActivation(false));
    }

    private void Update()
    {
        if(_gameController.KeysDisabled)
            return;
        CheckPlayerInput();
    }

    private void FixedUpdate() 
    {
        if(_gameController.KeysDisabled)
            return;
        if (_gameController.DamageController.isPlayerDead || _gameController.DamageController.GetHealth() <= 0)
        {
            SetStickyCamera(true);
            StartCoroutine(CutSceneColliderActivation(true));
            return;
        }
        CameraMovement();
        if(!isStopped)
            Move();
        if (_gameController.FlashlightController.flashlight.activeSelf)
        {
            _gameController.FlashlightController.ReduceBatteryOverTime(0.022f);
        }
        // ChangeFlashlightState();
    }
    private void LateUpdate()
    {
        if(_gameController.KeysDisabled)
            return;
        if(_uiController.inventoryPanel.gameObject.activeSelf)
            return;
        CallHints();
    }
    
    private void Move()
    {
        if(!_hasAnimator) 
            return;
        float targetSpeed = _inputManager.Run ? _runSpeed : _walkSpeed;
        if (_gameController.StaminaController.GetStamina() < 1)
        {
            targetSpeed = _walkSpeed;
        }
        if(_inputManager.Move ==Vector2.zero) 
            targetSpeed = 0;
        _currentVelocity.x = Mathf.Lerp(_currentVelocity.x, _inputManager.Move.x * targetSpeed, animBlendSpeed * Time.fixedDeltaTime);
        _currentVelocity.y = Mathf.Lerp(_currentVelocity.y, _inputManager.Move.y * targetSpeed, animBlendSpeed * Time.fixedDeltaTime);
        float step = targetSpeed * Time.deltaTime;
        var newPos = new Vector3(_currentVelocity.x, 0, _currentVelocity.y);
        _playerRigidbody.velocity = transform.TransformVector(newPos * step * 20);
        // var direction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;
        // _playerRigidbody.AddForce(transform.TransformVector(direction * step * 10), ForceMode.VelocityChange);
        // transform.position += transform.TransformDirection (newPos / 40);
        animator.SetFloat(_xVelocityHash, _currentVelocity.x);
        animator.SetFloat(_yVelocityHash, _currentVelocity.y);
        if (_inputManager.Run && (_currentVelocity.x > 1 || _currentVelocity.y > 1))
        {
            _gameController.StaminaController.ReduceStaminaOverTime(.1f);   
        }
    }

    private void CameraMovement()
    {
        if (GameController.instance.DamageController.isPlayerDead)
        {
            camera.localRotation = Quaternion.Euler(0, 0 , 0);
            return;
        }
        if(!_hasAnimator) 
            return;
        var Mouse_X = _inputManager.Look.x;
        var Mouse_Y = _inputManager.Look.y;
        camera.position = cameraRoot.position;
        _xRotation -= Mouse_Y * mouseSensitivity * Time.fixedDeltaTime;
        _xRotation = Mathf.Clamp(_xRotation, upperLimit, bottomLimit);
        camera.localRotation = Quaternion.Euler(_xRotation, 0 , 0);
        _playerRigidbody.MoveRotation(_playerRigidbody.rotation * Quaternion.Euler(0, Mouse_X * mouseSensitivity * Time.fixedDeltaTime, 0));
    }

    // private void ChangeFlashlightState()
    // {
    //     if (_inputManager.Flashlight && _gameManager.playerPrefsManager.GetBool(PlayerPrefsKeys.HasFlashlight, false))
    //     {
    //         if (_gameController.FlashlightController.GetBatteryAmount() <= 1)
    //         {
    //             flashlight.SetActive(false);
    //             _gameController.FlashlightController.ChangeFlashlightState(false);
    //             return;
    //         }
    //         // _gameController.LightsController.TurnLightOfPlaceOnOrOff(Places.HallwayFirstFloor, false);
    //     }
    //     else
    //     {
    //         flashlight.SetActive(false);
    //         _gameController.FlashlightController.ChangeFlashlightState(false);
    //         // _gameController.LightsController.TurnLightOfPlaceOnOrOff(Places.HallwayFirstFloor, true);
    //     }
    // }

    private void CheckPlayerInput()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (_gameManager.playerPrefsManager.GetBool(PlayerPrefsKeys.HasFlashlight, false))
            {
                if (_gameController.FlashlightController.GetBatteryAmount() == 0)
                {
                    _gameController.QuestAndHintController.ShowHint(7);
                }
                else if (_gameManager.playerPrefsManager.GetFloat(PlayerPrefsKeys.BatteryAmount, 0) < 1)
                {
                    _gameController.QuestAndHintController.ShowHint(4);
                }
                else
                {
                    bool flashLightsOn = !_gameController.FlashlightController.flashlight.activeSelf;
                    _gameController.FlashlightController.ChangeFlashlightState(flashLightsOn);   
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(!_uiController.pausePanel.gameObject.activeSelf)
            {
                _uiController.ShowPausePanel();
            }
            else
            {
                _uiController.HidePausePanel();
            }
        }
        else if (Input.GetKeyDown(KeyCode.I))
        {
            if(!_uiController.inventoryPanel.gameObject.activeSelf)
            {
                _gameController.Inventory.OpenInventory();
            }
            else
            {
                _gameController.Inventory.CloseInventory();
            }
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            InventoryPanel inventoryPanel = _uiController.inventoryPanel;
            if (inventoryPanel.gameObject.activeSelf)
            {
                inventoryPanel.DeleteItem();
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.M))
        {
            InteractableItemType type = InteractableItemType.MedKit;
            int itemCount = _gameManager.playerPrefsManager.GetInteractableItemCount(type);
            float health = _gameController.DamageController.GetHealth();
            if (itemCount > 0 && health < 100)
            {
                int firstItemId = _gameManager.playerPrefsManager.GetItemsInInventoryIds(type).Last();
                _gameManager.playerPrefsManager.DeleteItemFromInventory(firstItemId);
                _gameController.DamageController.Heal(_gameController.Inventory.GetItemInfo(type).ItemScriptableObject.effect);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.B))
        {
            InteractableItemType type = InteractableItemType.Battery;
            int itemCount = _gameManager.playerPrefsManager.GetInteractableItemCount(type);
            if (itemCount > 0)
            {
                int firstItemId = _gameManager.playerPrefsManager.GetItemsInInventoryIds(type).Last();
                _gameManager.playerPrefsManager.DeleteItemFromInventory(firstItemId);
                _gameController.FlashlightController.ReloadBattery();
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.P))
        {
            InteractableItemType type = InteractableItemType.Pills;
            int itemCount = _gameManager.playerPrefsManager.GetInteractableItemCount(type);
            if (itemCount > 0 && !_gameController.StaminaController.isInStaminaMode)
            {
                int firstItemId = _gameManager.playerPrefsManager.GetItemsInInventoryIds(type).Last();
                _gameManager.playerPrefsManager.DeleteItemFromInventory(firstItemId);
                _gameController.StaminaController.StaminaMode();
            }
        }
        
    }
    
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("JumpScare"))
        {
            other.gameObject.SetActive(false);
            _gameController.JumpScareController.SetJumpScare(time: 5, sticked: true, placementDegree: 180);
            _gameManager.playerPrefsManager.SaveGame();
        }
        else if (other.CompareTag("Cutscene"))
        {
            
            int currentLevel = _gameManager.playerPrefsManager.GetInt(PlayerPrefsKeys.Level, -1) + 1;
            GameObject currentLevelGO = _gameController.LevelsController.GetCurrentLevel();
            if (other.name.Contains("2") && currentLevel == 2 && currentLevelGO.GetComponent<Level2>().cutsceneTrigger.activeSelf)
            {
                currentLevelGO.GetComponent<Level2>().cutsceneTriggered = true;
            }
        }
    }

    public void SetStickyCamera(bool enable)
    {
        stickyCamera.SetActive(enable);
        camera.gameObject.SetActive(!enable);
    }

    public IEnumerator BlurOutOverTime(float maxVal, float minVal, float duration)
    {
        // postProcessVolumeMainCam.enabled = true;
        postProcessVolumeStickyCam.enabled = true;
        // PostProcessProfile postProcessProfile = stickyCamera.activeSelf ? postProcessVolumeStickyCam.profile : postProcessVolumeMainCam.profile;
        PostProcessProfile postProcessProfile = postProcessVolumeStickyCam.profile;
        DepthOfField dph;
        float time = 0;
        if (postProcessProfile.TryGetSettings<DepthOfField>(out dph))
        {
            float counter = 0;
            while (counter < duration)
            {
                counter += Time.deltaTime;
                float value = Mathf.Lerp(maxVal, minVal, counter / duration);

                dph.focalLength.value = value;
                yield return null;
            }
        }
        // postProcessVolumeMainCam.enabled = false;
        // postProcessVolumeStickyCam.enabled = false;
    }

    private void CallHints()
    {
        int totalKeys = _gameManager.playerPrefsManager.GetInteractableItemCount(InteractableItemType.Key);
        int totalMedkits = _gameManager.playerPrefsManager.GetInteractableItemCount(InteractableItemType.MedKit);
        float health = _gameController.DamageController.GetHealth();
        if (health < 100 && totalMedkits > 0)
        {
            _medkitTimer += Time.deltaTime;
            if (_medkitTimer > _medkitNotificationTime)
            {
                _medkitTimer = 0;
                _gameController.QuestAndHintController.ShowHint(6);
            }
        }
        else
        {
            _medkitTimer = 0;
        }
        if (totalKeys > 0)
        {
            _keyTimer += Time.deltaTime;
            if (_keyTimer > _keyNotificationTime)
            {
                _keyTimer = 0;
                _gameController.QuestAndHintController.ShowHint(5);
            }
        }
        else
        {
            _keyTimer = 0;
        }
    }

    public IEnumerator CutSceneColliderActivation(bool active)
    {
        if (active)
        {
            foreach (var cutsceneCollider in cutsceneColliders)
            {
                cutsceneCollider.enabled = active;
            }
            yield return new WaitForSeconds(.3f);
            _collider.enabled = !active;
        }
        else
        {
            _collider.enabled = !active;
            yield return new WaitForSeconds(.3f);
            foreach (var cutsceneCollider in cutsceneColliders)
            {
                cutsceneCollider.enabled = active;
            }
        }
    }

    public IEnumerator CameraOnJumpScare()
    {
        Animator cameraAnimator = camera.GetComponent<Animator>();
        cameraAnimator.enabled = true;
        cameraAnimator.Play("CameraJumpScare");
        while ((cameraAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime) % 1 < 0.99f)
        {
            yield return null;
        }

        cameraAnimator.enabled = false;
    }
    
    public IEnumerator CameraBlur(bool isStickyCamera = false)
    {
        Animator cameraAnimator = isStickyCamera ? stickyCamera.GetComponent<Animator>() : camera.GetComponent<Animator>();
        cameraAnimator.enabled = true;
        cameraAnimator.Play("Blur");
        while ((cameraAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime) % 1 < 0.99f)
        {
            yield return null;
        }

        cameraAnimator.enabled = false;
    }

    public IEnumerator StepBack(float speed = 2, int numOfSteps = 2)
    {
        _playerRigidbody.velocity = Vector3.zero;
        Vector3 destination = transform.position - numOfSteps * transform.forward * speed;
        while (Vector3.Distance(transform.position, destination) > .1f)
        {
            var step =  speed * Time.deltaTime / 2; 
            transform.position = Vector3.MoveTowards(transform.position, destination, step);
            yield return null;            
        }
    }
    
    public void ThrownBack(float force = .1f)
    {
        float speed = 1.5f;
        _playerRigidbody.AddForce(transform.TransformVector(-1 * transform.forward * force * 10), ForceMode.VelocityChange);
    }
    
    // private void Move()
    // {
    //     if(!_hasAnimator) 
    //         return;
    //     float targetSpeed = _inputManager.Run ? _runSpeed : _walkSpeed;
    //     if (_inputManager.Move == Vector2.zero)
    //         targetSpeed = 0.1f;
    //     _currentVelocity.x = Mathf.Lerp(_currentVelocity.x, targetSpeed * _inputManager.Move.x, animationSpeed * Time.fixedDeltaTime);
    //     _currentVelocity.y = Mathf.Lerp(_currentVelocity.y, targetSpeed * _inputManager.Move.y, animationSpeed * Time.fixedDeltaTime);
    //
    //     var xVelocityDifference = _currentVelocity.x - _playerRigidbody.velocity.x;
    //     var zVelocityDifference = _currentVelocity.y - _playerRigidbody.velocity.z;
    //     
    //     _playerRigidbody.AddForce(transform.TransformVector(new Vector3(xVelocityDifference, 0, zVelocityDifference)), ForceMode.VelocityChange);
    //     
    //     _animator.SetFloat(_xVelocityHash, _currentVelocity.x);
    //     _animator.SetFloat(_yVelocityHash, _currentVelocity.y);
    // }
    //
    // private void CameraMovement()
    // {
    //     if(!_hasAnimator) 
    //         return;
    //     var mouseX = _inputManager.Look.x;
    //     var mouseY = _inputManager.Look.y;
    //     camera.position = cameraRoot.position;
    //     _xRotation -= mouseY * mouseSensitivity * Time.deltaTime;
    //     _xRotation = Mathf.Clamp(_xRotation, upperLimit, bottomLimit);
    //     
    //     camera.localRotation = Quaternion.Euler(_xRotation, 0, 0);
    //     transform.Rotate(Vector3.up, mouseX * mouseSensitivity * Time.deltaTime);
    // }

    
}
