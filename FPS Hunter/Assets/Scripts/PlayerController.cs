using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    private Vector2 _movement = Vector2.zero;
    private Vector2 _deltaLook = Vector2.zero;
    private bool _sprint;
    private bool _jumped;
    private bool _attacked;
    
    private CharacterController _characterController;
    private Vector3 _moveDirection = Vector3.zero;
    private float _rotationX = 0;

    public float walkingSpeed = 7.5f;
    public float runningSpeed = 11.5f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;
    public Camera playerCamera;
    public float lookSpeed = 2.0f;
    public float lookXLimit = 45.0f;
    public Animator animator;
    
    
    [HideInInspector]
    public bool canMove = true;

    
    void Start()
    {
        _characterController = GetComponent<CharacterController>();

        // Lock cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void OnMovement(InputAction.CallbackContext ctx)
    {
        _movement = ctx.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext ctx)
    {
        _jumped = ctx.action.triggered;
    }

    public void OnSprint(InputAction.CallbackContext ctx)
    {
        _sprint = ctx.performed;
    }

    public void OnLook(InputAction.CallbackContext ctx)
    {
        _deltaLook = ctx.ReadValue<Vector2>();
    }

    public void OnAttack(InputAction.CallbackContext ctx)
    { 
        _attacked = ctx.action.triggered;
       animator.SetBool("isAttacking", _attacked);
    }
    
    void Update()
    {
        
        // We are grounded, so recalculate move direction based on axes
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);
        
        // Press Left Shift to run
        bool isRunning = _sprint;
        float curSpeedX = canMove ? (isRunning ? runningSpeed : walkingSpeed) * _movement.y : 0;
        float curSpeedY = canMove ? (isRunning ? runningSpeed : walkingSpeed) * _movement.x : 0;
        float movementDirectionY = _moveDirection.y;
        
        
        _moveDirection = (forward * curSpeedX) + (right * curSpeedY);
        
        if (_jumped && canMove && _characterController.isGrounded)
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
            Vector2 deltaInput = _deltaLook;
            _rotationX += -deltaInput.y * lookSpeed;
            _rotationX = Mathf.Clamp(_rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(_rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, deltaInput.x * lookSpeed, 0);
        }
    }
}