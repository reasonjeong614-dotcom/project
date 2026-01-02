using UnityEngine;
using System;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]

public class InputManagerYa : MonoBehaviour
{
    PlayerInput playerInput;

    //===옵저버===
    public static event Action<Vector2> OnMove;
    public static event Action OnJump;
    public static event Action OnAttack;
    public static event Action<bool> OnSprint;

    InputAction moveAction;
    InputAction jumpAction;
    InputAction attackAction;
    InputAction sprintAction;

    //콜백함수를 저장할 변수들
    Action<InputAction.CallbackContext> onMovePerformed;
    Action<InputAction.CallbackContext> onMoveCanceled;
    Action<InputAction.CallbackContext> onJumpPerformed;
    Action<InputAction.CallbackContext> onAttackPerformed;
    Action<InputAction.CallbackContext> onSprintPerformed;
    Action<InputAction.CallbackContext> onSprintCanceled;

    private void OnEnable()
    {
        playerInput = GetComponent<PlayerInput>();
        playerInput.defaultActionMap = "Player";
        playerInput.defaultControlScheme = "Default";
        playerInput.notificationBehavior = PlayerNotifications.InvokeCSharpEvents;

        moveAction = playerInput.actions.FindAction("Move");
        jumpAction = playerInput.actions.FindAction("Jump");
        attackAction = playerInput.actions.FindAction("Attack");
        sprintAction = playerInput.actions.FindAction("Sprint");

        if (moveAction != null)
        {
            onMovePerformed = ctx =>
            {
                Vector2 input = ctx.ReadValue<Vector2>();
                OnMove?.Invoke(input);
            };
            onMoveCanceled = ctx => { Vector2 input = Vector2.zero; OnMove?.Invoke(input); };
            moveAction.performed += onMovePerformed;
            moveAction.canceled += onMoveCanceled;
        }
    }

    private void OnDisable()
    {
        if(moveAction != null)
        {
            if(onMovePerformed  != null)
            {
                moveAction.performed -= onMovePerformed;
                onMovePerformed = null;
            }
            if(onMoveCanceled != null)
            {
                moveAction.canceled -= onMoveCanceled;
                onMoveCanceled = null;
            }
        }
    }

}
