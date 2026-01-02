using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerControllingRolling : MonoBehaviour
{
    Animator animator;

    int hashMoveX;
    int hashMoveY;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();

        hashMoveX = Animator.StringToHash("MoveX");
        hashMoveY = Animator.StringToHash("MoveY");
    }

    private void OnEnable()
    {
        InputManagerYa.OnMove += MoveHandler;

    }

    private void OnDisable()
    {
        InputManagerYa.OnMove -= MoveHandler;
    }

    void MoveHandler(Vector2 vector)
    {

    }

}
