using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI; // Arraste aqui o painel do menu no Inspector
    public ThirdPersonCamera cameraScript; // Arraste aqui seu script de c�mera
    private bool isPaused = false;

    void Start()
    {
        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(false);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                Resume();
            else
                Pause();
        }
    }

    public void Resume()
    {
        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(false);

        Time.timeScale = 1f;
        isPaused = false;

        // Habilita cursor e c�mera
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        if (cameraScript != null)
            cameraScript.enabled = true; // Volta a mover a c�mera
    }

    void Pause()
    {
        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(true);

        Time.timeScale = 0f;
        isPaused = true;

        // Mostra cursor e trava c�mera
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        if (cameraScript != null)
            cameraScript.enabled = false; // Trava a c�mera
    }

    public void BackToMenu()
    {
        Debug.Log("Bot�o de voltar ao menu pressionado!");
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }
}
