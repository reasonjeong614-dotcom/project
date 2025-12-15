using UnityEngine;
using Unity.Cinemachine;
using System.Collections;

public class CameraSwicher_My : MonoBehaviour
{
    public CinemachineCamera[] cameras;  //전환시킬 카메라 목록
    int activePriority = 10;             //활성화된 카메라 우선순위 값
    int inactivePriority = 0;            //비활성화된 카메라 우선순위 값
    int currentCameraIndex = 0;          //현재 활성화된 카메라 인덱스

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < cameras.Length; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                ActiveCamera(i);
            }
        }
    }

    public void ActiveCamera (int index)
    {
        //인덱스 예외처리
        if(index < 0 || index >= cameras.Length) return;

        //카메라 초기화
        for (int i = 0; i < cameras.Length; i++)
        {
            if (cameras[i] != null)
            {
                cameras[i].Priority = inactivePriority;
            }
        }

        //선택한 카메라 활성화
        cameras[index].Priority = activePriority;   
        currentCameraIndex = index;


    }
}

public class CameraShakeController_My : MonoBehaviour
{
    public CinemachineCamera camera;    //세상을 흔들 카메라
    [Range(0f, 2f)]
    public float defaultAmplitude = 1f; //강도
    [Range(0f, 2f)]
    public float defaultFrequency = 1f; //속도

    CinemachineBasicMultiChannelPerlin noise; //노이즈 컴포넌트


    private void Start()
    {
        noise = camera.GetComponent<CinemachineBasicMultiChannelPerlin>();

        if (noise == null )
        {
            Debug.LogError("없다");
            return;
        }

        //초기화
        noise.AmplitudeGain = 0;
        noise.FrequencyGain = 0;
    }

    public void ShakeCamera(float intensity,  float duration)
    {
        if (noise == null) return;
        StopAllCoroutines();
        StartCoroutine(ShakeCoroutine(intensity, duration));
    }

    IEnumerator ShakeCoroutine(float intensity, float duration)
    {
        //흔들림 시작
        noise.AmplitudeGain = intensity * defaultAmplitude;
        noise.FrequencyGain = intensity * defaultFrequency;

        //지속시간
        yield return new WaitForSeconds(duration);

        //부드럽게 감소시키기
        float fadeTime = 0.5f;
        float elapsed = 0f;
        float startAmp = noise.AmplitudeGain;
        float startFreq = noise.FrequencyGain;

        while (elapsed < fadeTime)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / fadeTime;

            noise.AmplitudeGain = Mathf.Lerp(startAmp, 0 , t);
            noise.FrequencyGain = Mathf.Lerp(startFreq, 0, t);

            yield return null;
        }

        //정지
        noise.AmplitudeGain = 0f;
        noise.FrequencyGain = 0f;
    }

    public void KeepShaking(float intensity)
    {
        if (noise == null) return;
        noise.AmplitudeGain = (intensity * defaultAmplitude);
        noise.FrequencyGain = (intensity * defaultFrequency);
    }

    public void StopShaking()
    {
        if(noise == null) return;
        StopAllCoroutines();
        noise.AmplitudeGain = 0f;
        noise.FrequencyGain = 0f;
    }
}
