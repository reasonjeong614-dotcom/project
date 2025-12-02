using UnityEngine;

/// <summary>
/// 1. 총알 이동 (방향은 설정해줘야 한다)
/// 2. 화면 밖으로 나가면 삭제 (충돌, 거리)
/// </summary>
public class Bullet : MonoBehaviour
{
    public float speed = 10f;           //초당 10유닛 이동
    public float destroyDistance = 20f; //발사 위치로부터 20유닛 떨어지면 삭제
    Vector3 startPosition;              //발사 위치

    void Start()
    {
        // 발사 위치 저장
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // 1. 총알 이동
        //transform.position += transform.up * speed * Time.deltaTime;
        transform.Translate(Vector3.up * speed * Time.deltaTime);

        // 2. 발사 위치로부터 일정 거리 이상 떨어지면 삭제
        // 2-1. 거리 계산
        //if (startPosition.y > destroyDistance)
        //{
        //    Destroy(gameObject);
        //}
        // 2-2. Vector3.Distance() 함수 사용
        // Distance(a, b) : a와 b 사이의 거리 반환
        // 피타고라스의 정리 응용 => 두 지점간의 차이의 제곱근
        //float distance = Vector3.Distance(startPosition, transform.position);
        //if (distance > destroyDistance)
        //{
        //    Destroy(gameObject);
        //}
        // 정교한 거리가 필요 없고 단순 비교만 할거면 sqrMagnitude를 쓰는게 낫다.
        // Distance (비교적 느림) 함수 내부적으로 제곱근 계산을 하기 때문에 성능에 약간 부담이 될 수 있다.
        // mgnitude (Distance함수보다 아주 조금 빠름) 속성도 마찬가지로 제곱근 계산을 한다.
        // sqrMagnitude (가장 빠름) 속성은 제곱근 계산을 하지 않기 때문에 성능에 유리하다.
        // 2-3. 제곱 거리 비교 (성능 향상)
        //float sqrDistance = (startPosition - transform.position).sqrMagnitude;
        //if (sqrDistance > destroyDistance * destroyDistance)
        //{
        //    Destroy(gameObject);
        //}
    }
}
