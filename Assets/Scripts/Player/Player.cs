using System;
using Base;
using Unity.Mathematics;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : Humanoid
{
    public bool isFreeze { get; set; }
    public Action<Item, int> dropAction;
    [Space]
    [SerializeField] private float _movementSpeed;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private Joystick _movementJoystick;
    
    [Header("FOV settings")]
    [SerializeField] private FieldOfView _fieldOfView;

    [SerializeField] private float _minFov = 50f;
    [SerializeField] private float _maxFov = 90f;

    [Header("Points")] [SerializeField] private SpawnPoint _dropPoint;
    
    [Header("AUTOSERIALIZED FIELD")] [SerializeField]
    private Rigidbody2D _rigidbody2D;

    private Vector2 _tapPosition;
    private bool _isMobile;
    private bool _isSwiping;

    private float _deadZone = 0.1f;
    private Vector2 _resolution;
    private float _nonRotationZone;
    private int _touchNumber;
    private Vector3 _joystickParameters = Vector3.zero;
    private float _fovDifference;

    private void OnValidate()
    {
        UpdateFields();
    }

    private void Start()
    {
        _isMobile = Application.isMobilePlatform;
        _resolution = new Vector2(Screen.width, Screen.height);
        _nonRotationZone = _resolution.x / 4;
        _fovDifference = _minFov < _maxFov ? _maxFov - _minFov : 0;
        dropAction += _dropPoint.SpawnItem;
        _movementJoystick = GameBus.Instance.GetJoystick();
        UpdateFields();
    }

    private void Update()
    {
        if(isFreeze) 
            return;
        
        _joystickParameters = GetJoystickParameters();
        RotateLogic();
        UpdateFOV();
    }
    
    private void FixedUpdate()
    {
        if(isFreeze) 
            return;
        
        MovementLogic();
    }

    private void UpdateFOV()
    {
        _fieldOfView.SetAimDirection(Utils.GetVectorFromAngle(transform.rotation.eulerAngles.z + 90));
        _fieldOfView.SetOrigin(transform.position);
        _fieldOfView.SetFov(_maxFov - _fovDifference * math.clamp(math.abs(_joystickParameters.x)  + math.abs(_joystickParameters.y), 0f, 1f));
    }

    private void MovementLogic()
    {
        var movement = transform.TransformVector(_joystickParameters.x, _joystickParameters.y, transform.position.z);
        _rigidbody2D.velocity = movement * _movementSpeed * Time.fixedDeltaTime;
    }

    private Vector3 GetJoystickParameters()
    {
        if (_movementJoystick == null)
            return Vector3.zero;
        var xMovement = _movementJoystick.Horizontal;
        var yMovement = _movementJoystick.Vertical;
        
        return new Vector3(xMovement, yMovement, 0);
    }

    private void RotateLogic()
    {
        if (!_isMobile)
        {
            if (Input.GetMouseButtonDown(0))
            {
                _isSwiping = true;
                _tapPosition = Input.mousePosition;
            }
            else if (Input.GetMouseButtonUp(0))
                ResetSwipe();
        }
        else
        {
            for (var i = 0; i < Input.touchCount; i++)
            {
                var touch = Input.GetTouch(i);
                if(touch.position.x < _nonRotationZone)
                    continue;

                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        _isSwiping = true;
                        _tapPosition = touch.position;
                        _touchNumber = i;
                        break;
                    case TouchPhase.Ended:
                    case TouchPhase.Canceled:
                        ResetSwipe();
                        break;
                }
            }
        }

        var swipeDelta = Vector2.zero;
        
        if(_tapPosition.x < _nonRotationZone) 
            return;

        if (_isSwiping)
        {
            var newTapPosition = new Vector2();

            if (!_isMobile && Input.GetMouseButton(0))
            {
                newTapPosition = Input.mousePosition;
                swipeDelta = newTapPosition - _tapPosition;
            }
            else if (Input.touchCount > _touchNumber)
            {
                newTapPosition = Input.GetTouch(_touchNumber).position;
                
                if(newTapPosition.x < _nonRotationZone)
                    return;
                
                swipeDelta = newTapPosition - _tapPosition;
            }

            if (swipeDelta.x * swipeDelta.x > _deadZone * _deadZone)
            {
                 transform.Rotate(new Vector3(0,0, -swipeDelta.x * _rotationSpeed * Time.deltaTime));
                _tapPosition = newTapPosition;
            }
        }
    }

    private void ResetSwipe()
    {
        _isSwiping = false;
        _tapPosition = Vector2.zero;
    }

    private void UpdateFields()
    {
        if (_rigidbody2D == null || _rigidbody2D == default)
            _rigidbody2D = GetComponent<Rigidbody2D>();
    }
}