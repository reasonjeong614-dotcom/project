using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] Transform firePoint;

    [Header("총")]
    [SerializeField] GameObject gun;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] float bulletSpeed = 40f;

    [Header("수류탄")]
    [SerializeField] GameObject grenadePrefab;
    [SerializeField] float grenadeSpeed = 10f;

    bool isGrenadeMode = false;


    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Z))
        {
            isGrenadeMode = !isGrenadeMode;    //수류탄 모드 토글
            if(isGrenadeMode)
            {
                gun.SetActive(false);
            }
            else
            {
                gun.SetActive(true);
            }
        }

        if(Input.GetMouseButtonDown(0))
        {
            if (isGrenadeMode)
            {
                Throw();
            }
            else
                Shoot();
        }
    }

    void Shoot()
    {
        // 마우스 위치 기준 Ray 생성
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Vector3 targetPoint;

        // Ray가 무언가에 맞으면 그 지점을 목표로
        if (Physics.Raycast(ray, out hit, 200f))
        {
            targetPoint = hit.point;
        }
        else
        {
            //안 맞으면 그냥 앞으로
            targetPoint = ray.GetPoint(200f);
        }

        //발사 방향 계산 (목표 지점 - 총구 위치)
        Vector3 dir = (targetPoint - firePoint.position).normalized;

        //총알 생성
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.LookRotation(dir));

        //총알 속도 반영
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.linearVelocity = dir * bulletSpeed;
    }

    void Throw()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Vector3 targetPoint;

        if (Physics.Raycast(ray, out hit, 200f))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.GetPoint(200f);
        }

        //발사 방향 (조금 위쪽으로)
        Vector3 dir = (targetPoint - firePoint.position).normalized;
        dir = (dir + Vector3.up * 0.3f).normalized;

        //수류탄 생성
        GameObject grenade = Instantiate(grenadePrefab, firePoint.position, Quaternion.LookRotation(dir));
        Rigidbody rb = grenade.GetComponent<Rigidbody>();
        rb.linearVelocity = dir * grenadeSpeed;

    }

}
