using UnityEngine;

public class AirplaneMovement : MonoBehaviour
{
    public float speed = 10f;
    public float waveAmplitude = 0.5f;
    public float waveFrequency = 1f;
    public float startDelay = 3f;

    public bool useAppearTiming = false; // Ativa timing de aparecer?
    public float appearTime = 0f; // Tempo para aparecer (segundos)
    public bool useDisappearTiming = false; // Ativa timing de sumir?
    public float disappearTime = 0f; // Tempo para sumir (segundos)

    private Vector3 basePosition;
    private float elapsed = 0f;
    private float lifeElapsed = 0f;
    private bool startedMoving = false;
    private bool isVisible = true;

    private Renderer[] renderers; // Para sumir/aparecer visualmente

    void Start()
    {
        basePosition = transform.position;
        renderers = GetComponentsInChildren<Renderer>();
    }

    void Update()
    {
        elapsed += Time.deltaTime;
        lifeElapsed += Time.deltaTime;

        // --- VISIBILIDADE CORRIGIDA ---
        bool shouldBeVisible = true;

        if (useAppearTiming && lifeElapsed < appearTime)
        {
            shouldBeVisible = false;
        }
        if (useDisappearTiming && disappearTime > 0f && lifeElapsed > disappearTime)
        {
            shouldBeVisible = false;
        }

        SetVisible(shouldBeVisible);

        if (!shouldBeVisible)
            return;

        // Delay inicial para come�ar a andar
        if (!startedMoving && elapsed >= startDelay)
        {
            startedMoving = true;
            elapsed = 0f;
            basePosition = transform.position; // guarda o ponto de partida pra oscila��o
        }

        if (!startedMoving)
            return;

        // Movimento pra frente no Z local
        basePosition += transform.forward * speed * Time.deltaTime;

        // Oscila��o no eixo local Y (cima do avi�o)
        float verticalOffset = Mathf.Sin(elapsed * waveFrequency) * waveAmplitude;

        // Define a posi��o atual somando offset � base (no eixo up do avi�o)
        transform.position = basePosition + transform.up * verticalOffset;
    }


    // Fun��o para controlar renderiza��o (aparecer/sumir)
    void SetVisible(bool visible)
    {
        if (isVisible == visible) return; // J� est� como quer

        foreach (var rend in renderers)
        {
            rend.enabled = visible;
        }
        isVisible = visible;
    }

    // M�todos p�blicos para outros scripts controlarem
    public void ResetLife()
    {
        lifeElapsed = 0f;
    }
    public void ForceAppear()
    {
        SetVisible(true);
        useAppearTiming = false;
    }
    public void ForceDisappear()
    {
        SetVisible(false);
        useDisappearTiming = false;
    }
}
