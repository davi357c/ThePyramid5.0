using UnityEngine;

public class CameraOrbit : MonoBehaviour
{
    public Transform target;
    public float sensitivity = 5f;
    public float distance = 6f;
    public float height = 3f;

    private float currentX = 0f;
    private float currentY = 0f;

    void Update()
    {
        currentX += Input.GetAxis("Mouse X") * sensitivity;
        currentY -= Input.GetAxis("Mouse Y") * sensitivity;
        currentY = Mathf.Clamp(currentY, -20, 60);
    }

    void LateUpdate()
    {
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
        Vector3 direction = new Vector3(0, height, -distance);
        transform.position = target.position + rotation * direction;
        transform.LookAt(target);
    }
}
