using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyFSM_FPS : MonoBehaviour
{
    //유한 상태 머신 => 유한한 수의 상태(state)와 상태들 사이의 전환(transition)을 조건(condition)으로 정의

    enum EnemyState
    {
        Idle,
        Move,
        Return,
        Attack,
        Damaged,
        Die
    }

    EnemyState state;

    public float findRange = 10f;       //플레이어 찾는 범위
    public float attackRange = 2f;      //공격 가능 범위
    public float moveRange = 15f;       //최대 이동 범위

    Vector3 startPoint;                 //몬스터 시작 위치
    Transform player;                   //공격할 대상 (플레이어) 코드로 처리
    CharacterController cc;             //이동 제어 cc 컴포넌트

    NavMeshAgent agent; //이동만! 충돌은 콜라이더로

    //몬스터 일반 변수
    int hp = 100;
    //int att = 5;
    //float speed = 1f;

    float attTime = 2f; //공격 딜레이
    float timer = 0;

    Animator animator;

    void Start()
    {
        //시작 지점 저장
        startPoint = transform.position;
        //플레이어 트랜스폼
        player = GameObject.Find("Player").transform;
        //캐릭터 컨트롤러
        cc = GetComponent<CharacterController>();

        agent = GetComponent<NavMeshAgent>();
        agent.enabled = false;

        state = EnemyState.Idle;

        animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        switch (state)
        {
            case EnemyState.Idle:
                Idle();
                break;
            case EnemyState.Move:
                Move();
                break;
            case EnemyState.Return:
                Return();
                break;
            case EnemyState.Attack:
                Attack();
                break;
        }
    }

    void Idle()
    {
        animator.SetBool("isMoving", false);
        if (Vector3.Distance(transform.position, player.position) < findRange)
        {
            state = EnemyState.Move;
        }
    }

    void Move()
    {
        agent.enabled = true;
        animator.SetBool("isMoving", true);

        //시작 지점에서 너무 멀어지면 돌아가기
        if (Vector3.Distance(transform.position, startPoint) > moveRange)
        { 
            state = EnemyState.Return; 
        }
        //공격 범위 안에 플레이어가 있으면 공격
        else if(Vector3.Distance(transform.position, player.position) < attackRange)
        {
            state = EnemyState.Attack;
        }
        else
        {
            agent.SetDestination(player.position);

            /*
            //플레이어 추격
            Vector3 dir = (player.position - transform.position).normalized;
            //타겟 방향으로 자연스럽게 회전
            //transform.forward = Vector3.Lerp(transform.forward, dir, 5 * Time.deltaTime); //얘는 백덤블링 가능
            //쿼터니온을 사용해서 회전하자
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), 5 * Time.deltaTime);

            cc.SimpleMove(dir * speed);
            */
        }
    }

    void Attack()
    {
        agent.enabled = false;

        //플레이어 바라보기
        transform.LookAt(player.position);

        //공격 범위 안이면 계속 공격
        if(Vector3.Distance(transform.position, player.position) < attackRange)
        {
            timer += Time.deltaTime;
            if(timer > attTime)
            {
                animator.SetTrigger("isAttacking");
                //때찌
                timer = 0;
            }
        }
        else
        {
            state = EnemyState.Move;
            timer = 0;
        }
    }

    void Return()
    {
        if (Vector3.Distance(transform.position, startPoint) > 0.1)
        {
            agent.SetDestination(startPoint);

            /*
            //시작 지점 방향
            Vector3 dir = (startPoint - transform.position).normalized;
            // 돌아가는 중에도 자연스럽게 회전
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), 5 * Time.deltaTime);
            // 이동
            cc.SimpleMove(dir * speed);
            */
        }
        else
        {
            transform.position = startPoint;
            transform.rotation = Quaternion.identity;

            state = EnemyState.Idle;

            agent.enabled = false;
        }
    }

    public void HitDamage(int value)
    {
        if (state == EnemyState.Damaged || state == EnemyState.Die) return;

        hp -= value;

        if (hp > 0)
        {
            state = EnemyState.Damaged;
            Damaged();
        }
        else
        {
            state = EnemyState.Die;
            Die();
        }
    }

    void Damaged()
    {
        StartCoroutine(DamageProc());
    }

    IEnumerator DamageProc()
    {
        yield return new WaitForSeconds(1f);
        state = EnemyState.Move;
    }

    void Die()
    {
        StopAllCoroutines();
        StartCoroutine(DieProc());
    }

    IEnumerator DieProc()
    {
        yield return new WaitForSeconds(1f);
        state = EnemyState.Die;
        Destroy(gameObject);
    }


    private void OnDrawGizmos()
    {
        //공격 가능 범위
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        //플레이어 찾을 수 있는 범위
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, findRange);

        //이동 가능한 최대 범위
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(startPoint, moveRange);
    }

}
