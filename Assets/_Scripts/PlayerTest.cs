using UnityEngine;
using UnityEngine.Playables;        //타임라인 사용하기 위해서

public class PlayerTest : MonoBehaviour
{
    //다른 스크립트에서 호출은 어떻게?
    CameraShakeController shakeController;

    //타임라인 사용하기 위해서
    PlayableDirector pd;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //다른곳에 사용중인 스크립트 찾아서 가져올때
        //FindAnyObjectByType 사용하기 이놈아가 성능이 가장 좋아서 유니티에서 권장함
        shakeController = FindAnyObjectByType<CameraShakeController>(); 

        pd = GetComponent<PlayableDirector>();
        pd.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        //if(Input.GetKeyDown(KeyCode.Space))
        //{
        //    shakeController.ShakeCamera(2f, 2f);
        //}

        if(Input.anyKeyDown)
        {
            pd.enabled = true;
            pd.Play();
        }
    }


    public void ShakeTest()
    {
        shakeController.ShakeCamera(2f, 2f);
    }
}
