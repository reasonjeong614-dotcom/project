using UnityEngine;
public class CamRotate : MonoBehaviour
{
    [SerializeField]
    float mouseSensitivity = 5f;

    Camera cam;

    float angleX;  //회전 각도

    private void Start()
    {
        cam = GetComponentInChildren<Camera>();
    }

    private void Update()
    {
        CameraMove();
    }

    void CameraMove()
    {
        bool click = Input.GetMouseButton(0);

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        if (click)
        {
            transform.Rotate(Vector3.up * mouseX);
            angleX -= mouseY;
        }

        angleX = Mathf.Clamp(angleX, -90f, 90f);
        cam.transform.localRotation = Quaternion.Euler(angleX, 0f, 0f);
    }
}
