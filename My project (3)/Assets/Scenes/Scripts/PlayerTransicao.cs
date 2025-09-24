using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class PlayerTransicao : MonoBehaviour
{
    [Header("Vídeo de transição")]
    public GameObject videoCanvas;           // Canvas com RawImage e VideoPlayer
    public VideoPlayer videoPlayer;
    public string proximaCena = "SampleScene";

    private bool videoIniciado = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!videoIniciado && other.CompareTag("Porta"))
        {
            videoIniciado = true;

            // Ativa o Canvas e inicia o vídeo
            videoCanvas.SetActive(true);

            // Conecta o evento de fim do vídeo à troca de cena
            videoPlayer.loopPointReached += OnVideoFim;

            videoPlayer.Play();
        }
    }

    // Chamado automaticamente quando o vídeo termina
    private void OnVideoFim(VideoPlayer vp)
    {
        SceneManager.LoadScene(proximaCena);
    }
}
