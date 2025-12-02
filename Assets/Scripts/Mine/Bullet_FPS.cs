using UnityEngine;

public class Bullet_FPS : MonoBehaviour
{
    public float lifeTime = 10f;

    [SerializeField] GameObject effect;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void OnCollisionEnter(Collision collision)
    {
        // 부딪친 지점 정보
        ContactPoint contact = collision.contacts[0];
        Vector3 hitPoint = contact.point;
        Vector3 hitNormal = contact.normal;  // 표면 법선 벡터

        // 이펙트 생성 (부딪친 지점에서, 표면 법선 방향으로)
        if (effect != null)
        {
            GameObject effects = Instantiate(
                effect,
                hitPoint,
                Quaternion.LookRotation(hitNormal)  // 법선 방향을 바라보게
            );
        }
            Destroy(gameObject);
    }
}
