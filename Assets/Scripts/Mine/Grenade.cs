using UnityEngine;

public class Grenade : MonoBehaviour
{
    public float lifeTime = 10f;

    [SerializeField] GameObject effect;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void OnCollisionEnter(Collision collision)
    {
        // 부딪치면 바로 삭제 ( TODO: 폭발 이펙트, 데미지 처리 추가)
        Instantiate(effect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
