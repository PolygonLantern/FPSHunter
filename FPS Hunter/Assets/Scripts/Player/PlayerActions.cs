using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Script that handles all the input callbacks
/// </summary>
public class PlayerActions : MonoBehaviour
{
    private PlayerController _playerController;
    private Animator _animator;
    private SingletonManager _singletonManager;
    private void Awake()
    {   
        _singletonManager = SingletonManager.Instance;
        _playerController = GetComponent<PlayerController>();
        _animator = GetComponentInChildren<Animator>();
    }

    public void OnMovement(InputAction.CallbackContext ctx)
    {
        _playerController.movement = ctx.ReadValue<Vector2>();
        bool isMoving = _playerController.movement.x > 0 || _playerController.movement.x < 0 ||
                        _playerController.movement.y > 0 || _playerController.movement.y < 0;
        
        _animator.SetFloat("VerticalMovement", _playerController.movement.y);
        _animator.SetFloat("HorizontalMovement", _playerController.movement.x);
        _animator.SetBool("IsMoving", isMoving);
    }

    public void OnJump(InputAction.CallbackContext ctx)
    {
        _playerController.jumped = ctx.action.triggered;
        if (_playerController.jumped && !_animator.GetBool("IsJumping"))
        {
            _animator.SetBool("IsJumping", true);
        }
        else if (!_playerController.jumped && _animator.GetBool("IsJumping"))
        {
            _animator.SetBool("IsJumping", false);
        }
        
    }

    public void OnSprint(InputAction.CallbackContext ctx)
    {
        _playerController.sprint = ctx.performed;
        _animator.SetBool("IsRunning", _playerController.sprint);
    }

    public void OnLook(InputAction.CallbackContext ctx)
    {
        _playerController.deltaLook = ctx.ReadValue<Vector2>();
    }

    public void OnAttack(InputAction.CallbackContext ctx)
    {
        _playerController.attacked = ctx.action.triggered;
        
        if (_playerController.canAttack)
        {
            _animator.SetBool("IsAttacking", _playerController.attacked); 
        }
        
    }

    public void OnReady(InputAction.CallbackContext ctx)
    {
        _singletonManager.EnemySpawnManager.KillAllEnemies();
      
    }

}
