using UnityEngine;

/// <summary>
/// 1. 충돌 감지 후 오브젝트 삭제 또는 비활성화
/// </summary>
public class DestroyZone : MonoBehaviour
{
    public GameObject Player;
    PlayerFire pf;

    void Start()
    {
        pf = Player.GetComponent<PlayerFire>();
    }

    private void OnTriggerEnter(Collider other)
    {
        //충돌한 오브젝트가 "Bullet" 태그를 가지고 있는지 확인
        //if (other.CompareTag("Bullet"))
        //{
        //    //오브젝트 비활성화
        //    other.gameObject.SetActive(false);
        //    //또는 오브젝트 삭제
        //    //Destroy(other.gameObject);
        //}

        // 충돌한 오브젝트의 이름이 "Bullet"인 경우
        //if (other.name.Contains("Bullet"))
        //{
        //    //오브젝트 비활성화
        //    other.gameObject.SetActive(false);
        //    //또는 오브젝트 삭제
        //    //Destroy(other.gameObject);
        //}

        // 충돌한 오브젝트의 레이어가 "Bullet"인 경우
        if (other.gameObject.layer == LayerMask.NameToLayer("Bullet"))
        {
            //오브젝트 비활성화
            //other.gameObject.SetActive(false);
            //또는 오브젝트 삭제
            //Destroy(other.gameObject);

            //오브젝트 풀에서 재사용할 수 있도록 처리
            //Player.GetComponent<PlayerFire>().bulletPool.Add(other.gameObject);
            //pf.bulletPool.Add(other.gameObject);

            //PlayerFire pf = GameObject.Find("Player").GetComponent<PlayerFire>();
            //pf.bulletPool.Add(other.gameObject);

            //이놈아가 제일 최적화 잘된 코드임
            pf.ReloadPool(other.gameObject);
        }

        // 충돌한 오브젝트의 레이어가 "Enemy"인 경우
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            Destroy(other.gameObject);
        }
    }

}
