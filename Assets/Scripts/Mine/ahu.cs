using UnityEngine;
using UnityEngine.InputSystem;

public class ahu : MonoBehaviour
{
    Animator animator;

    [SerializeField]
    float speed = 1.0f;

    PlayerInput playerInput;
    InputAction moveAction;
    InputAction jumpAction;

    Vector2 moveInput;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponentInChildren<Animator>();

        playerInput = GetComponent<PlayerInput>();

        moveAction = playerInput.actions.FindAction("Move");
        jumpAction = playerInput.actions["Jump"];

        if( moveAction != null )
        {
            moveAction.performed += OnMove;
            moveAction.canceled += OnMove;
        }
        if ( jumpAction != null )
        {
            jumpAction.performed += OnJump;
        }
    }

    private void OnDisable()
    {
        if (moveAction != null)
        {
            moveAction.performed -= OnMove;
            moveAction.canceled -= OnMove;
        }
        if (jumpAction != null)
        {
            jumpAction.performed -= OnJump;
        }
    }

    private void OnMove(InputAction.CallbackContext obj)
    {
        if (obj.performed) moveInput = obj.ReadValue<Vector2>();
        if (obj.canceled) moveInput = Vector2.zero;
    }

    private void OnJump(InputAction.CallbackContext obj)
    {
        throw new System.NotImplementedException();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMove();
    }

    void UpdateMove()
    {
        Vector3 dir = new Vector3(moveInput.x, 0, moveInput.y);
        dir.Normalize();
        transform.position += dir * speed * Time.deltaTime;
    }

}
