using UnityEngine;

/// <summary>
/// 1. 플레이어 이동
/// 2. 플레이어 화면밖으로 못나가게 막기
/// </summary>
public class PlayerController : MonoBehaviour
{
    // 플레이어 이동 구현하기
    // 1. Transform.position
    // 2. Transform.Translate()
    // 3. Vector3.MoveTowards()
    // 4. 마우스 클릭으로 이동 (이놈은 여기서 사용 안하지만 알아두면 써먹겠지?)
    // 5. Rigidbody를 사용해서 이동 (이건 패쓰)

    public float moveSpeed = 5f; //초당 5유닛 이동

    private Vector3 targetPosition;     //목표 위치

    private Camera mainCamera;          //메인 카메라

    public float paddingX = 0.04f;       //UV 좌표는 0 ~ 1사이, 작은값으로 수치 조정해야 한다.
    public float paddingY = 0.05f;
    // UI 요소중 마진과 패딩이 있다
    // 마진은 가장 바깥, 그 안쪽이 패딩, 중앙에 콘텐츠가 있다


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //초기 목표 위치를 현재 위치로 설정
        targetPosition = transform.position;

        //메인 카메라 참조 가져오기
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        // 플레이어 이동
        Move1();
        //Move2();
        //Move3();
        //Move4();

        // 플레이어 화면안에 가두기
        MoveInScreen();
    }

    void Move1()
    {
        // 1. 입력 받기 (키보드, 마우스 등 입력은 Input매니저가 담당한다)
        // Input.GetAxis("Horizontal") -> -1 ~ 1        (실수)
        // Input.GetAxisRaw("Horizontal") -> -1 ~ 1     (정수)

        float moveX = Input.GetAxis("Horizontal"); //좌우 입력
        float moveY = Input.GetAxis("Vertical");   //상하 입력

        // 2. 이동 벡터 만들기
        Vector3 dir = new Vector3(moveX, moveY, 0f); //z축은 0으로 고정

        // 3. 방향 벡터 정규화 (대각선 이동시 속도 보정)
        //dir = dir.normalized;
        dir.Normalize();

        // 4. 이동해라
        //Vector3 pos = transform.position + dir * moveSpeed * Time.deltaTime;
        //transform.position = pos;
        transform.position += dir * moveSpeed * Time.deltaTime;
    }
    void Move2()
    {
        float moveX = Input.GetAxis("Horizontal"); //좌우 입력
        float moveY = Input.GetAxis("Vertical");   //상하 입력
        Vector3 dir = new Vector3(moveX, moveY, 0f); //z축은 0으로 고정
        dir.Normalize();
        transform.Translate(dir * moveSpeed * Time.deltaTime);
        //Translate는 로컬좌표계 기준 이동
        //Space.World: 월드좌표계 기준 이동 (게임 세계의 절대 좌표)
        //Space.Self: 로컬좌표계 기준 이동 (오브젝트 자신의 좌표 - 자신의 방향 기준)
        //transform.Translate(dir * moveSpeed * Time.deltaTime, Space.World);
    }
    void Move3()
    {
        //MoveTowards는 현재 위치에서 목표 위치로 일정 속도로 이동
        //MoveTowards(현재위치, 목표위치, 속도*시간)

        // 1. 입력 받기
        float moveX = Input.GetAxis("Horizontal"); //좌우 입력
        float moveY = Input.GetAxis("Vertical");   //상하 입력

        // 2. 입력이 있으면 목표 위치 갱신
        if (moveX != 0f || moveY != 0f)
        {
            Vector3 dir = new Vector3(moveX, moveY, 0f); 
            dir.Normalize();
            targetPosition += dir * moveSpeed * Time.deltaTime; //1유닛 이동한 위치를 목표 위치로 설정
        }

        // 3. 현재 위치에서 목표 위치로 이동 (부드럽게 이동)
        transform.position = Vector3.MoveTowards(
            transform.position, 
            targetPosition, 
            moveSpeed);
    }

    void Move4()
    {
        // 메인 카메라의 중요 함수
        // 메인 카메라도한 자주 사용하기때문에 어디서든 접근 할 수 있도록 변수 선언해서 사용한다
        // Camera.main: 씬에서 "MainCamera" 태그가 붙은 카메라를 찾아서 반환

        // 1. ScreenToWorldPoint: 화면 좌표를 월드 좌표로 변환
        // 2. ScreenToViewportPoint: 화면 좌표를 뷰포트 좌표로 변환
        // 3. ViewportToWorldPoint: 뷰포트 좌표를 월드 좌표로 변환
        // 4. WorldToScreenPoint: 월드 좌표를 화면 좌표로 변환
        // 5. WorldToViewportPoint: 월드 좌표를 뷰포트 좌표로 변환
        // 6. ViewportToScreenPoint: 뷰포트 좌표를 화면 좌표로 변환
        // 뷰포트 좌표: (0,0) 왼쪽 아래, (1,1) 오른쪽 위

        // 스크린의 화면을 마우스로 클릭했을때 3D 공간의 클릭지점으로 오브젝트를 움직일때
        // Carmera.main.ScreenToWorldPoint(Input.mousePosition);

        // 마우스 왼쪽버튼을 누르고 있는 동안
        if (Input.GetMouseButton(0))
        {
            // 마우스 스크린 좌표를 월드 좌표로 변환
            // 플레이어 높이(Z)는 유지해줘야 한다
            Vector3 mousePos = Input.mousePosition; //스크린 좌표
            Vector3 worldPos = mainCamera.ScreenToWorldPoint(mousePos);

            worldPos.z = transform.position.z; //플레이어 높이 유지

            print("마우스 클릭 좌표: " + mousePos);
            print("월드 좌표: " + worldPos);
            print("플레이어 월드 좌표: " + transform.position);

            // 만약 원 클릭으로 클릭좌표까지 이동하려면 아래 MoveTowards()함수가 업데이트에 있어야 한다.
            transform.position = Vector3.MoveTowards(
                transform.position,
                worldPos,
                moveSpeed * Time.deltaTime);

        }
    }

    void MoveInScreen()
    {
        //플레이어를 화면밖으로 나가지 못하게 막기
        // 1. 화면밖 공간에 큐브 4개 만들어서 배치하면 충돌체 때문에 밖으로 벗어나지 못한다
        // 2. 플레이어 트렌스폼의 포지션 x, y 값을 고정시킨다
        //Vector3 position = transform.position;
        //if (position.x > 2.5f) position.x = 2.5f;
        //if (position.y < -2.5f) position.y = -2.5f;
        //유니티에서는 클램프 사용이 더 권장된다 (성능차이 없음)
        //position.x = Mathf.Clamp(position.x, -2.5f, 2.5f);
        //position.y = Mathf.Clamp(position.y, -2.5f, 2.5f);

        // 3. 메인카메라의 뷰포트를 가져와서 처리한다
        // 스크린좌표: 모니터 해상도의 픽셀
        // 뷰포트좌표: 카메라의 사각뿔 끝에 있는 사각형 - 왼쪽하단(0, 0), 우측상단(1, 1)
        // UV좌표: 화면 텍스트, 2D 이미지를 표시하기 위한 좌표계 (텍스쳐좌표계라고도 한다)
        // 왼쪽상단(0, 0), 우측하단(1, 1)

        Vector3 position = mainCamera.WorldToViewportPoint(transform.position);
        position.x = Mathf.Clamp(position.x, 0f + paddingX, 1f - paddingX);
        position.y = Mathf.Clamp(position.y, 0f + paddingY, 1f - paddingY);
        transform.position = mainCamera.ViewportToWorldPoint(position);

    }

}
