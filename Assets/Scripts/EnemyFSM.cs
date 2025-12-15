using System;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI; //네비메시에이젼트를 사용하려면 반드시 필요함


public class EnemyFSM : MonoBehaviour
{
    /*
    유한 상태 머신 (FSM)
    => 유한한 수의 상태(state)와 상태들 사이의 전환(transition)을 정의해서 
    시스템 동작을 하게 하는 것을 말하고 당연히 전환이 이루어 질려면 조건(condition)이 필요하다
    이것은 FSM 디자인 패턴이다.
    - 상태: 간단하게 행동들 (걷기, 달리기, 점프, 공격, 죽음 등등)
    - 전환: 상태에서 상태로 넘어가는 변화
    - 조건: 전환이 발생하기 위한 필요한 기준 (키입력, HP감소, 특정 아이템을 획득 등 다양한 이벤트)
    => 결론은 이미 여러분은 애니메이터를 사용하면서 한번씩은 겪어 봤다

    => 플레이어 캐릭터 행동 제어
    => 대표적으로 에너미 AI 구현
    => 예) 몬스터가 플레이어를 발견하기 전에는 (순찰)상태, 플레이어를 발견하면 (추격)상태,
    공격범위안에 들어오면 (공격)상태, HP가 일정이하로 떨어졌을때 (도망, 버서커)
    */


    //몬스터 상태 이넘문
    enum EnemyState
    {
        Idel, Move, Attack, Return, Damaged, Die
    }

    EnemyState state;       //몬스터 상태 변수

    public float findRange = 10f;       //플레이어를 찾는 범위
    public float moveRange = 30f;       //시작지점에서 최대 이동가능한 범위
    public float attackRange = 2f;      //공격 가능 범위
    Vector3 startPoint;                 //몬스터 시작위치
    Transform player;                   //플레이어를 찾기 위해서 (코드로 처리하기)
    CharacterController cc;             //몬스터 이동을 위한 캐릭터컨트롤러 컴포넌트

    //유니티에서 길찾기 알고리즘이 적요이된 네비게이션을 사용하려면 반드시 UnityEngine.AI 추가해야 함
    //네비게이션은 맵전체를 베이크를 해서 에이전트가 어느 위치에 있던 미리 계산된 정보를 사용한다
    NavMeshAgent agent;
    //에이전트 사용시 주의사항
    //충돌은 콜리더로 하고
    //이동만 네비메시에이전트를 사용해야
    //enemyFSM을 제대로 사용할 수 있다.
    //충돌이 제대로 작동안할 수도 있다
    //따라서 시작할때 네비메이에이전트는 꺼줘야 한다.



    //몬스터 일반변수
    int hp = 100;
    int att = 5;
    float speed = 5f;

    //공격 딜레이
    float attTime = 2f;                 //2초에 한번 공격하기
    float timer = 0;                    //타이머

    //애니메이션을 제어하기 위한 에니메이션 컴포넌트
    //Animator anim;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //몬스터 상태 초기화
        state = EnemyState.Idel;
        //시작지점 저장
        startPoint = transform.position;
        //플레이어 트렌스폼
        player = GameObject.Find("Player").transform;
        //캐릭터 컨트롤러
        cc = GetComponent<CharacterController>();

        //네비메시에이전트
        agent = GetComponent<NavMeshAgent>();
        agent.enabled = false;

    }

    // Update is called once per frame
    void Update()
    {
        //상태에 따른 행동처리
        switch(state)
        {
            case EnemyState.Idel:
                Idel();
                break;
            case EnemyState.Move:
                Move();
                break;
            case EnemyState.Attack:
                Attack();
                break;
            case EnemyState.Return:
                Return();
                break;
            case EnemyState.Damaged:
                Damaged();
                break;
            case EnemyState.Die:
                Die();
                break;
        }
        
    }

    //대기상태
    private void Idel()
    {
        //1. 플레이어와 일정범위가 되면 이동상태로 변경 (탐지범위)
        //- 플레이어 찾기
        //- 일정거리 비교 (Distance, magnitude, sqrMagnitue 아무거나)
        //- 상태변경 state = EnemyState.Move;
        //- 상태전환 출력 print("Idle -> Move");
        //- 애니메이션 anim.SetTrigger("Move");

        //탐지범위 안에 들어왔음
        if (Vector3.Distance(transform.position, player.position) < findRange)
        {
            state = EnemyState.Move;
            print("상태전환: Idle -> Move");
        }
    }

    //이동상태
    private void Move()
    {
        //시작할때 꺼주고 무브상태가 아닐때 꺼줘야 한다
        if(!agent.enabled) agent.enabled = true;

        //1. 플레이어를 향해 이동 후 공격범위 안에 들어오면 공격상태로 변경
        //2. 플레이어를 추격하더라도 처음위치에서 일정범위를 넘어가면 리턴상태로 변경
        //- 플레이어 처럼 캐릭터 컨트롤러 이용하기 (cc.Move 대신 cc.SimpleMove 이용하자)
        //- 공격범위 2미터
        //- 상태변경
        //- 상태전환 출력
        //- 애니메이션

        //이동중 이동할 수 있는 최대범위를 벗어났을때
        if(Vector3.Distance(transform.position, startPoint) > moveRange)
        {
            state = EnemyState.Return;
            print("상태전환: Move -> Return");
        }
        //리턴상태가 아니면 플레이어 추격해야 한다
        else if(Vector3.Distance(transform.position, player.position) > attackRange)
        {
            //플레이어를 향해서 이동해라
            //네비메시에이전트가 회전처리부터 이동까지 전부다 처리해준다
            agent.SetDestination(player.position);


            //플레이어를 향해서 이동해라
            //네비메시에이전트가 회전처리부터 이동까지 전부다 처리해준다
            //NavMeshAgent.SetDestination(player.position);


            //플레이어를 추격
            //이동방향(벡터 뺄셈)
            //Vector3 dir = (player.position - transform.position).normalized;
            //dir.Normalize();

            //몬스터가 자신이 서있는 위치에서 회전값없이 백스텝으로 쫓아온다
            //몬스터가 타겟을 바로보도록 하자
            //1
            //transform.forward = dir;
            //2
            //transform.LookAt(dir);

            //좀더 자연스럽게 회전처리를 하자
            //transform.forward = Vector3.Lerp(transform.forward, dir, 5 * Time.deltaTime);
            //여기에 문제가 한가지 있는데 
            //타겟과 몬스터가 일직선상일경우 왼쪽으로 회전해야 할지 오른쪽으로 회전해야 할지 정하질 못해서
            //그냥 백덤블링으로 회전을 하게 된다

            //최종적으로 자연스런 회전처리를 하려면 결국 쿼터니온을 사용해야 한다
            //transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), 
            //    5 * Time.deltaTime);


            //cc.SimpleMove(dir * speed);
        }
        //공격범위 안에 들어옴
        else
        {
            state = EnemyState.Attack;
            print("상태전환: Move -> Attack");
        }
    }

    //공격상태
    private void Attack()
    {
        //에이전트 오프
        agent.enabled = false;


        //1. 플레이어가 공격범위 안에 있다면 일정한 시간 간격으로 플레이어 공격
        //2. 플레이어가 공격범위를 벗어났다면 이동상태(재추격)로 변경
        //- 공격범위 2미터
        //- 상태변경
        //- 상태전환 출력
        //- 애니메이션

        //공격범위 안에 들어옴
        if(Vector3.Distance(transform.position, player.position) < attackRange)
        {
            //공격할때 거리로만 처리되다보니 엉뚱한곳을 공격할 수 있다
            transform.LookAt(player.position);

            //일정 시간마다 플레이어 공격하기
            timer += Time.deltaTime;
            if(timer > attTime)
            {
                print("공격");
                //플레이어의 필요한 스크립트 컴포넌트를 가져와서 데미지를 주면 된다
                //player.GetComponent<PlayerMove>().hitDamage(att);

                //타이머 초기화
                timer = 0;

                //애니메이션 공격
            }
        }
        //재추격상태
        else
        {
            state = EnemyState.Move;
            print("상태전환: Attack -> Move");
            //애니메이션 무브
        }
    }

    //복귀상태
    private void Return()
    {
        //1. 몬스터가 플레이어를 추격하더라도 처음 위치에서 일점 범위를 벗어나면 다시 돌아옴
        //- 처음 위치에서 일정범위 30미터
        //- 상태변경
        //- 상태전환 출력
        //- 애니메이션

        //시작위치까지 도착하지 않았을때는 이동
        //도착하면 대기상태로 변경
        if(Vector3.Distance(transform.position, startPoint) > 0.1)
        {
            //이동처리
            agent.SetDestination(startPoint);
        }
        else
        {
            //위치값을 초기값으로
            transform.position = startPoint;
            transform.rotation = Quaternion.identity;

            //상태변경
            state = EnemyState.Idel;
            print("상태전환: Return -> Idle");

            //에이전트 오프
            agent.enabled = false;
        }
    }

    //플레이어쪽에서 충돌감지를 할 수 있으니 이함수는 퍼블릭으로 만들자
    public void HitDamage(int value)
    {
        //예외처리
        //피격상태거나, 죽은상태일때는 데미지 중첩으로 주지 않는다
        if (state == EnemyState.Damaged || state == EnemyState.Die) return;

        //체력깎기
        hp -= value;
        //몬스터 체력이 1이상이면 피격상태
        if(hp > 0)
        {
            state = EnemyState.Damaged;
            print("상태전환: Anystate -> Damaged");
            print("HP: " + hp);
            Damaged();
        }
        //0이하면 죽음상태
        else
        {
            state = EnemyState.Die;
            print("상태전환: Anystate -> Die");
            Die();
        }
    }


    //피격상태 (Any State)
    private void Damaged()
    {
        //1. 몬스터 체력이 1이상
        //2. 다시 이전상태로 변경
        //- 상태변경
        //- 상태전환 출력

        //피격상태를 처리하기 위해서는 간단한 코루틴 사용하자
        StartCoroutine(DamageProc());
    }

    IEnumerator DamageProc()
    {
        //피격모션 시간만큼 기다리기
        yield return new WaitForSeconds(1f);
        //현재상태를 이동으로 전환
        state = EnemyState.Move;
        print("상태전환: Damaged -> Move");
    }

    //죽음상태 (Any State)
    private void Die()
    {
        //1. 체력이 0이하
        //2. 몬스터 오브젝트 삭제
        //- 상태변경
        //- 상태전환 출력

        //죽음상태를 처리하기 위해서는 간단한 코루틴 사용하자
        StartCoroutine(DieProc());
    }

    IEnumerator DieProc()
    {
        //에이전트 오프
        agent.enabled = false;
        //2초후에 자기자신 제거한다
        yield return new WaitForSeconds(2f);
        print("죽었다");
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        //공격가능범위
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        //플레이어 찾을 수 있는 범위
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, findRange);
        //이동가능한 최대 범위
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(startPoint, moveRange);
    }
}
