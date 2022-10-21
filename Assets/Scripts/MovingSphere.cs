using UnityEngine;
using UnityEngine.InputSystem;

public class MovingSphere : MonoBehaviour
{
    [SerializeField, Range(10.0f, 100.0f)]
    private float _maxSpeed        = 10.0f;
    [SerializeField, Range(10.0f, 100.0f)]
    private float _maxAcceleration = 10.0f;
    [SerializeField] 
    private Rect  _allowedArea     = new(-10.0f, -10.0f, 20.0f, 20.0f);
    [SerializeField, Range(0.0f, 1.0f)]
    private float _bounciness      = 0.5f;

    private Vector2 _movement;
    private Vector3 _velocity      = Vector3.zero;

    public void Movement(InputAction.CallbackContext context) => 
        _movement = context.ReadValue<Vector2>();

    private void Update()
    {
        var desiredVelocity = new Vector3(_movement.x, 0.0f, _movement.y) * _maxSpeed;
        var maxSpeedChange = _maxAcceleration * Time.deltaTime;

        _velocity.x = Mathf.MoveTowards(_velocity.x, desiredVelocity.x, 
            maxSpeedChange);
        _velocity.z = Mathf.MoveTowards(_velocity.z, desiredVelocity.z, 
            maxSpeedChange);

        var displacement = _velocity * Time.deltaTime;
        var newPosition = transform.localPosition + displacement;

        if (newPosition.x < _allowedArea.xMin)
        {
            newPosition.x = _allowedArea.xMin;
            _velocity.x = -_velocity.x * _bounciness;
        }

        if (newPosition.x > _allowedArea.xMax)
        {
            newPosition.x = _allowedArea.xMax;
            _velocity.x = -_velocity.x * _bounciness;
        }

        if (newPosition.z < _allowedArea.yMin)
        {
            newPosition.z = _allowedArea.yMin;
            _velocity.z = -_velocity.z * _bounciness;
        }

        if (newPosition.z > _allowedArea.yMax)
        {
            newPosition.z = _allowedArea.yMax;
            _velocity.z = -_velocity.z * _bounciness;
        }

        transform.localPosition = newPosition;
    }
}