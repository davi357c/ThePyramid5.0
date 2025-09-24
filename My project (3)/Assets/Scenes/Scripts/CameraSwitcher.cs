using UnityEngine;


public class CameraSwitcher : MonoBehaviour

{
    public GameObject cutsceneCamera;
    public GameObject playerCamera;

    void Start()
    {
        Invoke("AtivarCameraDoPlayer", 30f); // Espera 30 segundos
    }

    void AtivarCameraDoPlayer()
    {
        cutsceneCamera.SetActive(false);
        playerCamera.SetActive(true);
    }
}
