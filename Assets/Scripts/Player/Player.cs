using System;
using Unity.Mathematics;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : Humanoid
{
    [SerializeField] private float _movementSpeed;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private Joystick _movementJoystick;
    [SerializeField] private FieldOfView _fieldOfView;
    

    [Header("AUTOSERIALIZED FIELD")] [SerializeField]
    private Rigidbody2D _rigidbody2D;

    private Vector2 _tapPosition;
    private bool _isMobile;
    private bool _isSwiping;

    private float _deadZone = 0.1f;
    private Vector2 _resolution;
    private float _zoneForRotation;
    private int _touchNumber;
    
    private void OnValidate()
    {
        UpdateFields();
    }

    private void Start()
    {
        _isMobile = Application.isMobilePlatform;
        _resolution = new Vector2(Screen.width, Screen.height);
        _zoneForRotation = _resolution.x / 4 * 3;
        UpdateFields();
    }

    private void Update()
    {
        RotateLogic();
        _fieldOfView.SetAimDirection(Utils.GetVectorFromAngle(transform.rotation.eulerAngles.z + 90));
        _fieldOfView.SetOrigin(transform.position);
    }

    private void FixedUpdate()
    {
        MovementLogic();
    }

    private void MovementLogic()
    {
        if (_movementJoystick == null)
            return;
        var xMovement = _movementJoystick.Horizontal;
        var yMovement = _movementJoystick.Vertical;
        var movement = transform.TransformVector(xMovement, yMovement, transform.position.z);

        _rigidbody2D.velocity = movement * _movementSpeed * Time.fixedDeltaTime;
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
                if(touch.position.x > _zoneForRotation)
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
        
        if(_tapPosition.x > _zoneForRotation) 
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
                
                if(newTapPosition.x > _zoneForRotation)
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