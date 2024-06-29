using System;
using ConfigScripts;
using Managers.SaveLoadManagers;
using Player.InputSystem;
using Unity.Mathematics;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerHumanoid : Humanoid
{
    public bool isFreeze { get; set; }
    public Action<ItemConfig, int> dropAction;
    [Space]
    [SerializeField] private float _movementSpeed;
    [SerializeField] private float _rotationSpeed;
    
    [Header("FOV settings")]
    [SerializeField] private FieldOfView _fieldOfView;

    [SerializeField] private float _minFov = 50f;
    [SerializeField] private float _maxFov = 90f;

    [Header("Points")] [SerializeField] private SpawnPoint _dropPoint;
    
    [Header("AUTOSERIALIZED FIELD")] [SerializeField]
    private Rigidbody2D _rigidbody2D;

    private IInputSystem _inputSystem;
    
    private float _fovDifference;

    private void OnValidate()
    {
        UpdateFields();
    }
    
    private void OnApplicationQuit()
    {
        PlayerSaveLoadManager.Instance.SetPlayerTransformData(transform.position, transform.rotation.eulerAngles.z); 
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        PlayerSaveLoadManager.Instance.SetPlayerTransformData(transform.position, transform.rotation.eulerAngles.z);
    }

    private void Start()
    {
        _inputSystem = GameBus.Instance.PlayerInputSystem;
        _inputSystem.IsActive = true;
        
        _fovDifference = _minFov < _maxFov ? _maxFov - _minFov : 0;
        dropAction += _dropPoint.SpawnItem;
        UpdateFields();
    }

    private void Update()
    {
        if(isFreeze) 
            return;
        
        RotateLogic();
        UpdateFOV();
    }
    
    private void FixedUpdate()
    {
        if (isFreeze) 
            return;
        
        MovementLogic();
    }

    private void UpdateFOV()
    {
        _fieldOfView.SetAimDirection(Utils.GetVectorFromAngle(transform.rotation.eulerAngles.z + 90));
        _fieldOfView.SetOrigin(transform.position);
        _fieldOfView.SetFov(_maxFov - _fovDifference * math.clamp(math.abs(_inputSystem.HorizontalMoveInput)  + math.abs(_inputSystem.VerticalMoveInput), 0f, 1f));
    }

    private void MovementLogic()
    {
        var movement = transform.TransformVector(_inputSystem.HorizontalMoveInput, _inputSystem.VerticalMoveInput, transform.position.z);
        _rigidbody2D.velocity = movement * _movementSpeed * Time.fixedDeltaTime;
    }

    private void RotateLogic()
    {
        transform.Rotate(new Vector3(0,0, _inputSystem.HorizontalRotateInput  * _rotationSpeed * Time.deltaTime));
    }

    private void UpdateFields()
    {
        if (_rigidbody2D == null || _rigidbody2D == default)
            _rigidbody2D = GetComponent<Rigidbody2D>();
    }
}