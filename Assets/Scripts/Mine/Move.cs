using UnityEngine;

public class Move : MonoBehaviour
{
    [SerializeField]
    float moveSpeed = 5.0f;
    CharacterController cc;

    float gravity = -10f;        //중력
    float velocityY;            //낙하속도
    float jumpPower = 3f;      //점프파워
    int jumpCount = 0;          //점프카운트
    int jumpMaxCount = 2;

    Animator animator;

    void Start()
    {
        cc = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        MoveWithGetAxis();
    }

    void MoveWithGetAxis()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        if (moveX != 0 || moveZ != 0)
        {
            animator.SetBool("isWalking", true);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }

        Vector3 dir = new Vector3(moveX, 0f, moveZ);

        dir.Normalize();

        //카메라가 보는 방향으로 이동하기
        dir = Camera.main.transform.TransformDirection(dir);
        //아잇 공중으로 날아가요

        //땅에 닿아있으면 수직속도 초기화
        /*
        if(cc.isGrounded) //바닥에 있니
        {
            velocityY = 0f;
        }
        */
        if(cc.collisionFlags == CollisionFlags.Below) //캡슐 아랫부분하고 닿아있니
        {
            velocityY = 0f;
            jumpCount = 0;
        }
        else
        {
            //중력 적용
            velocityY += gravity * Time.deltaTime;
            dir.y = velocityY;
        }

        //점프
        if(Input.GetButtonDown("Jump") && jumpCount < jumpMaxCount)
        {
            animator.SetBool("isJumping", true);
            jumpCount++;
            velocityY = jumpPower;
        }
        else { animator.SetBool("isJumping", false); }

            cc.Move(dir * moveSpeed * Time.deltaTime);
    }
}
