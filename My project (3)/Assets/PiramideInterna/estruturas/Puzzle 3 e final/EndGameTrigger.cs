using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using System.Collections;

public class EndGameTrigger : MonoBehaviour
{
    public GameObject endGameCanvas;
    public VideoPlayer videoPlayer;
    public KeyCode interactKey = KeyCode.E;

    public CanvasGroup buttonsGroup;
    public float buttonDelay = 3f;
    public float fadeDuration = 1.5f;

    private bool playerInRange = false;
    private bool triggered = false; // garante que só dispara uma vez

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInRange = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInRange = false;
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(interactKey) && !triggered)
        {
            triggered = true;
            endGameCanvas.SetActive(true);
            videoPlayer.Play();
            Time.timeScale = 0f;

            // Ativa o cursor
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            StartCoroutine(ShowButtonsWithFade());

        }
    }

    private IEnumerator ShowButtonsWithFade()
    {
        yield return new WaitForSecondsRealtime(buttonDelay);

        float t = 0f;
        buttonsGroup.gameObject.SetActive(true);
        buttonsGroup.alpha = 0f;

        while (t < fadeDuration)
        {
            t += Time.unscaledDeltaTime;
            buttonsGroup.alpha = Mathf.Lerp(0f, 1f, t / fadeDuration);
            yield return null;
        }

        buttonsGroup.alpha = 1f;
    }

    public void Retry()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
