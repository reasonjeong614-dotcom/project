using UnityEngine;

/// <summary>
/// 1. 아이템 먹어서 보조비행기가 새롭게 생성되는 기능 (On/Off)
/// 2. 보조비행기는 일정시간마다 자동으로 총알발사 한다
/// </summary>
public class PlayerClone : MonoBehaviour
{
    public GameObject clone;                    //보조비행기 오브젝트
    public GameObject bulletFactory;            //총알 공장
    public float fireTime = 1f;                 //총알 발사 간격 (1초에 한번씩)
    float timer = 0f;                           //시간 누적 변수


    void Update()
    {
        //아이템먹었을 때 보조비행기 생성  
        CreateClone();

        //보조비행기가 자동으로 총알 발사
        AutoFire();
    }

    void CreateClone()
    {
        // 간단하게 ESC키를 누르면 보조비행기 생성 했지만
        // 나중에 아이템 먹었을 때 생성되도록 변경 필요
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //clone.SetActive(true);
            //if (clone.activeSelf)
            //    clone.SetActive(false);
            //else
            //    clone.SetActive(true);
            clone.SetActive(!clone.activeSelf); //토글형식으로 변경

            //bool isJump = !isJump; //토글형식
        }
    }

    void AutoFire()
    {
        // 보조비행기가 활성화 상태일 때만 총알 발사
        if (clone.activeSelf)
        {
            // 중요함
            // 몇초에 한번씩 이벤트 발동
            // timer 변수를 이용해서 시간 누적
            // 게임에서 정말 자주 사용함

            // 시간 누적
            timer += Time.deltaTime;
            if (timer >= fireTime)
            {
                timer = 0f; // 타이머 초기화

                // 총알 발사
                //GameObject[] bullet = new GameObject[2];
                GameObject[] bullet = new GameObject[clone.transform.childCount];
                for(int i = 0; i< clone.transform.childCount; i++)
                {
                    bullet[i] = Instantiate(bulletFactory);
                    bullet[i].transform.position = clone.transform.GetChild(i).position;
                }
                
            }
        }
    }
}
