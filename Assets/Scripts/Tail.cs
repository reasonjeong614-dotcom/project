using UnityEngine;

/// <summary>
/// 1. 타겟 오브젝트 따라다니기
/// </summary>
public class Tail : MonoBehaviour
{
    public Transform target;                // 따라갈 타겟 오브젝트의 Transform
    public float speed = 3.0f;              // 따라가는 속도

    float stopDistance = 1f;                // 멈출 거리

    void Update()
    {
        // 타겟 방향 구하기 (벡터의 뺄셈)
        // 타겟 위치 - 내 위치 = 방향 벡터
        //Vector3 dir = target.position - transform.position;
        //dir.Normalize(); // 방향 벡터의 크기를 1로 만들기 (단위 벡터)
        //// 이동
        ////transform.position += dir * speed * Time.deltaTime;
        //transform.Translate(dir * speed * Time.deltaTime);


        // 타겟 따라다니기
        FollowTarget();
    }

    void FollowTarget()
    {
        // 타겟 방향 구하기 (벡터의 뺄셈)
        // 타겟 위치 - 내 위치 = 방향 벡터
        Vector3 dir = target.position - transform.position;
        
        // 현재 타겟과의 거리 계산
        //float distance = Vector3.Distance(target.position, transform.position);
        float distance = dir.magnitude;

        // 현재 거리가 멈출 거리보다 클 때만 이동
        if (distance > stopDistance)
        {
            dir.Normalize(); // 방향 벡터의 크기를 1로 만들기 (단위 벡터)
            transform.Translate(dir * speed * Time.deltaTime);
        }

    }
}
