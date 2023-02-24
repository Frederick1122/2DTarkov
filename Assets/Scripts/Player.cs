using System;
using Unity.Mathematics;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    [SerializeField] private float _movementSpeed;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private Joystick _movementJoystick;

    [Header("AUTOSERIALIZED FIELD")] [SerializeField]
    private Rigidbody2D _rigidbody2D;


    private Vector2 _tapPosition;
    private bool _isMobile;
    private bool _isSwiping;

    private float _deadZone = 0.1f;
    private Vector2 _resolution;
    private float _zoneForRotation;
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
            if (Input.touchCount > 0)
            {
                var firstTouch = Input.GetTouch(0);
                switch (firstTouch.phase)
                {
                    case TouchPhase.Began:
                        _isSwiping = true;
                        _tapPosition = firstTouch.position;
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
            if (!_isMobile && Input.GetMouseButton(0))
                swipeDelta = (Vector2) Input.mousePosition - _tapPosition;
            else if (Input.touchCount > 0)
                swipeDelta = Input.GetTouch(0).position - _tapPosition;

            if ((swipeDelta.x * swipeDelta.x)  > (_deadZone * _deadZone))
            {
                 transform.Rotate(new Vector3(0,0, -swipeDelta.x * _rotationSpeed * Time.deltaTime));
                _tapPosition = Input.mousePosition;
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