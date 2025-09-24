using UnityEngine;

public class EngineFailure : MonoBehaviour
{
    public float failureTime = 15f;
    public float transitionDuration = 2f;
    public float failureRoll = -30f; // Inclina��o direita
    public GameObject fireObject;
    private float elapsed = 0f;
    private float timeSinceFailure = 0f;
    private bool engineFailure = false;

    private Quaternion startRotation;
    private AirplaneMovement airplaneMovement;

    void Start()
    {
        startRotation = transform.rotation;
        airplaneMovement = GetComponent<AirplaneMovement>();
        if (fireObject != null)
            fireObject.SetActive(false);
    }

    void Update()
    {
        elapsed += Time.deltaTime;

        if (!engineFailure && elapsed >= failureTime)
        {
            engineFailure = true;
            timeSinceFailure = 0f;
            if (fireObject != null)
                fireObject.SetActive(true);
        }

        if (!engineFailure)
            return;

        timeSinceFailure += Time.deltaTime;

        // Transi��o suave do roll
        float t = Mathf.Clamp01(timeSinceFailure / transitionDuration);
        float roll = Mathf.Lerp(0f, failureRoll, t);

        // Mant�m pitch e yaw originais, s� mexe o roll
        Vector3 euler = startRotation.eulerAngles;
        transform.rotation = Quaternion.Euler(euler.x, euler.y, roll);

        // Se quiser ajustar a oscila��o vertical durante a falha:
        // airplaneMovement.customAmplitude = 0.8f;
        // airplaneMovement.customFrequency = 1.5f;
        // Se n�o, s� inclina mesmo.
    }
}
