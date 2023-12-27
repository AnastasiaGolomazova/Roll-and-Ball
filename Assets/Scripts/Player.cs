using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PlayerInput))]
public class Player : MonoBehaviour
{
    [Space(5)]
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _jumpHeight;
    [SerializeField] private float _smoothTime;

    [Space(5)]
    [Header("Sensitivity")]
    [SerializeField] private float _sensitivityX;
    [SerializeField] private float _sensitivityY;

    [Space(5)]
    [Header("Cinemachine")]
    [SerializeField] private GameObject _cinemachineCameraTarget;
    [SerializeField] private float _topClamp = 70.0f;
    [SerializeField] private float _bottomClamp = -30.0f;
    [SerializeField] private float _cameraAngleOverride = 0.0f;
    [SerializeField] private bool _lockCameraPosition = false;

    [Space(5)]
    [SerializeField] private Spawner _spawner;

    [Space(5)]
    [SerializeField] private bool _isRollAndBoll = false;

    private float _cinemachineTargetYaw;
    private float _cinemachineTargetPitch;

    private Vector2 _direction;
    private Vector2 _rotateDirection;

    private Rigidbody _rigidbody;
    private GameObject _mainCamera;

    private float _smoothVelocity;
    private const float _threshold = 0.01f;

    private bool _isGround = false;

    private void Awake()
    {
        if (_mainCamera == null)
            _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
    }

    private void Start()
    {
        Time.timeScale = 1;
        _rigidbody = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        _cinemachineTargetYaw = _cinemachineCameraTarget.transform.rotation.eulerAngles.y;
    }

    private void FixedUpdate()
    {
        Move(_direction);
    }

    private void LateUpdate()
    {
        CameraRotation(_rotateDirection);
    }

    private void OnMove(InputValue value)
    {
        if (!_isGround)
            return;

        _direction = value.Get<Vector2>();
    }

    private void OnLook(InputValue value)
    {
        _rotateDirection = value.Get<Vector2>();
    }

    private void OnJump(InputValue value)
    {
        if (!_isGround)
            return;

        _rigidbody.AddForce(Vector3.up * _jumpHeight, ForceMode.Impulse);
    }
 

    private void Move(Vector2 direction)
    {
        if (direction.sqrMagnitude >= _threshold)
        {
            float rotationAngle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg + _mainCamera.transform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, rotationAngle, ref _smoothVelocity, _smoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 move = Quaternion.Euler(0f, angle, 0f) * Vector3.forward;

            _rigidbody.velocity = move * _moveSpeed;
        }
    }

    private void CameraRotation(Vector2 rotate)
    {
        if (rotate.sqrMagnitude >= _threshold && !_lockCameraPosition)
        {
            _cinemachineTargetYaw += rotate.x * _sensitivityX;
            _cinemachineTargetPitch += rotate.y * (_sensitivityY * -1);
        }

        _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
        _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, _bottomClamp, _topClamp);

        _cinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + _cameraAngleOverride, _cinemachineTargetYaw, 0.0f);
    }

    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) 
            lfAngle += 360f;

        if (lfAngle > 360f)
            lfAngle -= 360f;

        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }

    private void TakeGreen()
    {
        Score.instance.AddScore2x();
        StartCoroutine(SpeedControl(10, 15));
    }

    private void TakeBlue()
    {
        Score.instance.AddScore();
        _spawner.SetCount();
    }

    private void TakeRed()
    {
        Score.instance.RemoveScore();
        StartCoroutine(SpeedControl(10, 5));
    }

    private IEnumerator SpeedControl(float time, float speed)
    {
        _moveSpeed = speed;
        yield return new WaitForSeconds(time);
        _moveSpeed = 10f;
    }

    private void OnCollisionEnter(Collision collision)
    {
        _isGround = true;

        if (_isRollAndBoll)
        {
            if (collision.gameObject.TryGetComponent(out SetColor box))
            {
                Destroy(box.gameObject);

                _spawner.RaiseObject();

                if (box._colorType == SetColor.ColorType.green)
                    TakeGreen();
                else if (box._colorType == SetColor.ColorType.blue)
                    TakeBlue();
                else
                    TakeRed();
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        _isGround = false;
    }
}