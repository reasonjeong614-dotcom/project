using UnityEngine;
using Unity.Cinemachine;

/// <summary>
/// 키입력 받아서 카메라 전환
/// 우선순위(Priority)의 값을 변경해서 처리한다
/// </summary>
public class CameraSwicher : MonoBehaviour
{
    public CinemachineCamera[] cameras;     //전환시킬 카메라 목록
    int activePriority = 10;                //활성화된 카메라 우선순위 값
    int inactivePriority = 0;               //비활성회된 카메라 우선순위 값

    // Update is called once per frame
    void Update()
    {
        //숫자 키로 카메라 전환
        for (int i = 0; i < cameras.Length; i++)
        {
            if(Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                ActiveCamera(i);
            }
        }
    }

    public void ActiveCamera(int index)
    {
        //유효한 인덱스인지 확인
        if (index < 0 || index >= cameras.Length) return;

        //모든 카메라를 비활성화
        for(int i = 0; i < cameras.Length; i++)
        {
            if (cameras[i] != null)
            {
                cameras[i].Priority = inactivePriority;
            }
        }

        //선택한 카메라만 활성화
        cameras[index].Priority = activePriority;

        print($"카메라 전환: {cameras[index].name}");
    }
}
