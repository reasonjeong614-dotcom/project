using UnityEngine;

/// <summary>
/// 1. 총알발사, 파편튀기 (레이로 충돌처리)
/// 2. 수류탄발사
/// </summary>
public class PlayerFire : MonoBehaviour
{
    public Transform firePoint;                 //총알 발사 위치
    public GameObject bulletImpactFactory;      //총알 파편 프리팹
    public GameObject bombFactory;              //폭탄 프리팹
    public float throwPower = 10f;              //던질 파워


    // Update is called once per frame
    void Update()
    {
        //총알 및 수류탄 발사
        Fire();
    }

    void Fire()
    {
        //마우스 왼쪽버튼일대 레이케스트로 총알 발사
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
            RaycastHit hit;

            //레이랑 충돌했냐?파
            if(Physics.Raycast(ray, out hit))
            {
                print("충돌오브젝트: " + hit.collider.name);

                //충돌지점에 총알 파편만 생성하면 된다
                GameObject bulletImpact = Instantiate(bulletImpactFactory);
                //부딪힌 지점
                bulletImpact.transform.position = hit.point;
                //파편이 부딪힌 지점이 향하는 방향으로 튀게 해줘야 한다
                //Hit 정보안에 노멀벡터의 값도 알수 있다
                //법선벡터 또는 노멀벡터는 평면에 수직인 벡터
                bulletImpact.transform.forward = hit.normal;

                //내 총알에 충돌했으니 몬스터 체력 깍기
                EnemyFSM enemy = hit.collider.GetComponent<EnemyFSM>();
                enemy.HitDamage(10);


            }

            // 레이어 마스크 사용 충돌처리 (최적화)
            // tag보다 약 20배 빠르다
            // 총 32비트를 사용하기때문에 32개까지 추가 가능

            //int layer = gameObject.layer;

            //layer = 1 << 6; //플레이어
            // 0000 0000 0000 0001 => 0000 0000 0010 0000

            // 0000 0000 1000 0000 => Enemy
            // 0000 0000 0000 1000 => Boss
            // 0000 1000 0000 0000 => Player
            // 0000 1000 1000 1000 => 모두다 충돌처리

            //layer = 1 << 8 | 1 << 4 | 1 << 12; //모두다 충돌처리
            //if (Physics.Raycast(ray, out hit, 100, layer)) //전부다 충돌
            //{ 
            //    //if(플레이어라면)
            //    //if(에너미라면)
            //    //if(보스라면)
            //}
            //if (Physics.Raycast(ray, out hit, 100, ~layer)) //레어어를 제외하고 충돌
            //{
            //}
        }

        //폭탄 던지기
        if(Input.GetMouseButtonDown(1))
        {
            //폭탄 생성
            GameObject bomb = Instantiate(bombFactory);
            bomb.transform.position = firePoint.position;

            //폭탄은 플레이어가 던지기 때문에
            //폭탄이 들고 있는 리지드바디를 이용하면 된다
            Rigidbody rb = bomb.GetComponent<Rigidbody>();
            //전방으로 물리적인 힘을 가한다
            //rb.AddForce(Camera.main.transform.forward * throwPower, ForceMode.Impulse);

            //ForceMode.Acceleration => 연속적인 힘을 가한다 (질량 영향 놉)
            //ForceMode.Force => 연속직인 힘을 가한다 (질량 영향을 받는다)

            //ForceMode.VelocityChange => 순간적인 힘을 가한다 (질량 영향 놉)
            //ForceMode.Impulse => 순간적인 힘을 가한다 (질량 영향을 받는다)

            //45도정도의 각도로 발사
            //벡터의 덧셈 (UP + Foward)
            //각도를 낮추고 싶다 => Foward의 길이를 늘려준다
            //각도를 높이고 싶다 => Up의 길이를 늘려준다
            //Vector3 dir = Camera.main.transform.forward + Camera.main.transform.up;
            Vector3 dir = Camera.main.transform.up + (Camera.main.transform.forward * 2f);
            dir.Normalize();
            rb.AddForce(dir * throwPower, ForceMode.Impulse);

        }


        // 스나이퍼 모드
        if (Input.GetKey(KeyCode.Escape))
        {
            Camera.main.fieldOfView = 20f;  //3배확대
        }
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Camera.main.fieldOfView = 60f;
        }
    }
}
