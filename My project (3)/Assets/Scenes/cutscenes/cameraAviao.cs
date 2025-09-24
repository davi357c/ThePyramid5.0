using UnityEngine;

public class FollowXZ : MonoBehaviour
{
    public Transform target;  // Arraste o avi�o aqui
    private float fixedY = 79f;

    void LateUpdate()
    {
        if (target != null)
        {
            Vector3 newPos = target.position;
            newPos.y = fixedY;
            transform.position = newPos;
        }
    }
}
