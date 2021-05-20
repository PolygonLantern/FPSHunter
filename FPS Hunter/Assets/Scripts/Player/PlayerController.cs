using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    private Animator _animator;
    private float _nextAttackTime = 0f;
    public float attackRate = 2f;
    
    [HideInInspector]
    public Vector2 movement = Vector2.zero;
    [HideInInspector]
    public Vector2 deltaLook = Vector2.zero;
    [HideInInspector]
    public bool sprint;
    [HideInInspector]
    public bool jumped;
    [HideInInspector]
    public bool attacked;
    
    private CharacterController _characterController;
    private Vector3 _moveDirection = Vector3.zero;
    private float _rotationX = 0;

    public float walkingSpeed = 7.5f;
    public float runningSpeed = 11.5f;
    public float jumpSpeed = 20f;
    public float gravity = 20.0f;
    public Camera playerCamera;
    public float lookSpeed = 2.0f;
    public float lookXLimit = 45.0f;

    [HideInInspector]
    public bool canMove = true;

    public bool canAttack;
    void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponentInChildren<Animator>();
        canAttack = true;
        
        // Lock cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    void Update()
    {
        _animator.SetBool("IsGrounded", _characterController.isGrounded);
        
        if (Time.time >= _nextAttackTime && !canAttack)
        {
            if (attacked)
            {
                canAttack = true;
                _nextAttackTime = Time.time + 6f / attackRate;
            }
            canAttack = false;
        }
        // We are grounded, so recalculate move direction based on axes
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);
        
        // Press Left Shift to run
        bool isRunning = sprint;
        float curSpeedX = canMove ? (isRunning ? runningSpeed : walkingSpeed) * movement.y : 0;
        float curSpeedY = canMove ? (isRunning ? runningSpeed : walkingSpeed) * movement.x : 0;
        float movementDirectionY = _moveDirection.y;
        
        
        _moveDirection = (forward * curSpeedX) + (right * curSpeedY);
        
        if (jumped && canMove && _characterController.isGrounded)
        {
            _moveDirection.y = jumpSpeed;
        }
        else
        {
            _moveDirection.y = movementDirectionY;
        }

        // Apply gravity. Gravity is multiplied by deltaTime twice (once here, and once below
        // when the moveDirection is multiplied by deltaTime). This is because gravity should be applied
        // as an acceleration (ms^-2)
        if (!_characterController.isGrounded)
        {
            _moveDirection.y -= gravity * Time.deltaTime;
        }

        // Move the controller
        _characterController.Move(_moveDirection * Time.deltaTime);

        // Player and Camera rotation
        if (canMove)
        {
            Vector2 deltaInput = deltaLook;
            _rotationX += -deltaInput.y * lookSpeed;
            _rotationX = Mathf.Clamp(_rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(_rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, deltaInput.x * lookSpeed, 0);
        }
    }
}