using UnityEngine;

/// <summary>
/// 1. 배경 스크롤
/// /// </summary>
public class Background : MonoBehaviour
{
    Material mat;                       // 배경 이미지의 머티리얼
    public float scrollSpeed = 0.1f;    // 배경 스크롤 속도

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // 배경 이미지의 머티리얼을 가져온다
        mat = GetComponent<MeshRenderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        // 백그라운드 스크롤
        ScrollBackground();
    }

    void ScrollBackground()
    {
        // 시간에 따른 오프셋 계산
        Vector2 offset = mat.mainTextureOffset;
        //offset.y 값만 보정해준다
        offset.Set(0, offset.y + scrollSpeed * Time.deltaTime);
        // 머티리얼의 오프셋 설정
        mat.mainTextureOffset = offset;
    }
}
