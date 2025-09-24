using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class PlayerTransicao : MonoBehaviour
{
    [Header("V�deo de transi��o")]
    public GameObject videoCanvas;           // Canvas com RawImage e VideoPlayer
    public VideoPlayer videoPlayer;
    public string proximaCena = "SampleScene";

    private bool videoIniciado = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!videoIniciado && other.CompareTag("Porta"))
        {
            videoIniciado = true;

            // Ativa o Canvas e inicia o v�deo
            videoCanvas.SetActive(true);

            // Conecta o evento de fim do v�deo � troca de cena
            videoPlayer.loopPointReached += OnVideoFim;

            videoPlayer.Play();
        }
    }

    // Chamado automaticamente quando o v�deo termina
    private void OnVideoFim(VideoPlayer vp)
    {
        SceneManager.LoadScene(proximaCena);
    }
}
