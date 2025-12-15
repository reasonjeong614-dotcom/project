using UnityEngine;
public class CamRotate_FPS : MonoBehaviour
{
    [SerializeField]
    float mouseSensitivity = 10f;

    Camera cam;

    float angleX;  //회전 각도

    private void Start()
    {
        cam = GetComponentInChildren<Camera>();
        // 마우스를 화면 중앙에 고정하고 보이지 않게
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        CameraMove();
    }

    void CameraMove()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        transform.Rotate(Vector3.up * mouseX);  //좌우 회전 (플레이어 몸통 회전)
       
        angleX -= mouseY;                       //상하 회전 (카메라만 위,아래로)
        angleX = Mathf.Clamp(angleX, -90f, 90f);
        cam.transform.localRotation = Quaternion.Euler(angleX, 0f, 0f);
    }
}
