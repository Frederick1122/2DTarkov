using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    [SerializeField] private float _movementSpeed;
    [SerializeField] private Joystick _movementJoystick;

    [Header("AUTOSERIALIZED FIELD")] [SerializeField]
    private Rigidbody2D _rigidbody2D;

    private void OnValidate()
    {
        UpdateFields();
    }

    private void Start()
    {
        UpdateFields();
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
        Vector3 movement = new Vector3(xMovement, yMovement, transform.position.z);

        _rigidbody2D.velocity = movement * _movementSpeed * Time.fixedDeltaTime;

    }

    private void UpdateFields()
    {
        if (_rigidbody2D == null || _rigidbody2D == default)
            _rigidbody2D = GetComponent<Rigidbody2D>();
    }
}