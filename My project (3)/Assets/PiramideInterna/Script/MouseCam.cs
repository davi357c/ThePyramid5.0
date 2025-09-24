using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform target;             
    public Vector3 offset = new Vector3(0, 1.8f, -3.5f); 
    public float sensitivity = 3f;
    public bool lockCursor = true;

    private float currentX = 0f;
    private float currentY = 0f;
    public float yMin = -20f;
    public float yMax = 60f;

    void Start()
    {
        if (lockCursor)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    void Update()
    {
        currentX += Input.GetAxis("Mouse X") * sensitivity;
        currentY -= Input.GetAxis("Mouse Y") * sensitivity;
        currentY = Mathf.Clamp(currentY, yMin, yMax);
    }

    void LateUpdate()
    {
        if (target == null) return;

        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
        Vector3 desiredPosition = target.position + rotation * offset;

        transform.position = desiredPosition;
        transform.LookAt(target);
    }
}
