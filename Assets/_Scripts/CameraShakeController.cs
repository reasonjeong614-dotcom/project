using System.Collections;
using UnityEngine;
using Unity.Cinemachine;

public class CameraShakeController : MonoBehaviour
{
    public CinemachineCamera camera;        //쉐이크할 대상 카메라
    [Range(0f, 2f)]
    public float defaultAmplitude = 1f;     //흔들리는 강도
    [Range(0f, 2f)]
    public float defalutFrequency = 1f;     //흔들리는 주기(속도)

    CinemachineBasicMultiChannelPerlin noise;  //쉐이크 효과 주는 노이즈 컴포넌트


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //노이즈 컴포넌트 가져오기
        noise = camera.GetComponent<CinemachineBasicMultiChannelPerlin>();

        //예외처리
        if(noise == null)
        {
            Debug.LogError("CinemachineBasicMultiChannelPerlin 컴포넌트가 없음");
            return;
        }

        //쉐이크 초기값
        noise.AmplitudeGain = 0;
        noise.FrequencyGain = 0;
    }
    

    /// <summary>
    /// 일시적으로 카메라를 흔든다
    /// </summary>
    /// <param name="intensity">흔들리는 강도</param>
    /// <param name="duration">지속 시간</param>
    public void ShakeCamera(float intensity, float duration)
    {
        if (noise == null) return;
        StopAllCoroutines();        //기존 코루틴들은 중단
        StartCoroutine(ShakeCoroutine(intensity, duration));
    }
    //흔들리는 효과 코루틴
    IEnumerator ShakeCoroutine(float intensity, float duration)
    {
        //즉심 흔들림 시작
        noise.AmplitudeGain = intensity * defaultAmplitude;
        noise.FrequencyGain = intensity * defalutFrequency;

        //지속시간
        yield return new WaitForSeconds(duration);

        //부드럽게 감소시키기
        float fadeTime = 0.5f;
        float elapsed = 0f;
        float startAmp = noise.AmplitudeGain;
        float startFreq = noise.FrequencyGain;

        while(elapsed < fadeTime)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / fadeTime;

            noise.AmplitudeGain = Mathf.Lerp(startAmp, 0, t);
            noise.FrequencyGain = Mathf.Lerp(startFreq, 0, t);

            yield return null;
        }

        //완전히 정지하기
        noise.AmplitudeGain = 0f;
        noise.FrequencyGain = 0f;
    }

    /// <summary>
    /// 지속적인 흔들리는 효과 (걷기/달리기)
    /// </summary>
    /// <param name="intensity"></param>
    public void StartContinuousShake(float intensity)
    {
        if (noise == null) return;
        noise.AmplitudeGain = intensity * defaultAmplitude;
        noise.FrequencyGain = intensity * defalutFrequency;
    }

    /// <summary>
    /// 흔들리는 효과 즉시 정지
    /// </summary>
    public void StopShake()
    {
        if(noise ==  null) return;
        StopAllCoroutines();
        noise.AmplitudeGain = 0f;
        noise.FrequencyGain = 0f; 
    }
}
