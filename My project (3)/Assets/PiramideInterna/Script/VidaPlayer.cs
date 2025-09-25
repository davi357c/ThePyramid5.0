using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class VidaPlayer : MonoBehaviour
{
    public static bool PlayerMorto = false;

    [Header("Vida")]
    public int vidaMaxima = 100;
    public int vidaAtual;

    [Header("UI")]
    public Image sangueNaTela;
    private Color corOriginal;

    public GameObject painelMorte; // Painel com os botões
    public Button botaoRespawn;
    public Button botaoMenu;

    [Header("Respawn")]
    public float tempoParaRespawn = 3f;

    private Animator animator;

    void Start()
    {
        vidaAtual = vidaMaxima;
        PlayerMorto = false;

        animator = GetComponent<Animator>();

        if (sangueNaTela != null)
        {
            corOriginal = sangueNaTela.color;
            sangueNaTela.color = new Color(corOriginal.r, corOriginal.g, corOriginal.b, 0f);
        }

        if (painelMorte != null)
            painelMorte.SetActive(false); // desativa painel no começo

        // Configura botões
        if (botaoRespawn != null)
            botaoRespawn.onClick.AddListener(RespawnCena);

        if (botaoMenu != null)
            botaoMenu.onClick.AddListener(VoltarMenu);
    }

    public void TomarDano(int quantidade)
    {
        if (PlayerMorto) return;

        vidaAtual -= quantidade;

        if (sangueNaTela != null)
        {
            sangueNaTela.color = new Color(corOriginal.r, corOriginal.g, corOriginal.b, 0.7f);
            StartCoroutine(FadeSangue());
        }

        if (animator != null)
        {
            animator.SetBool("TomandoDano", true);
            StartCoroutine(ResetarTomarDano());
        }

        if (vidaAtual <= 0)
            Morrer();
    }

    private IEnumerator ResetarTomarDano()
    {
        yield return new WaitForSeconds(0.5f);
        animator.SetBool("TomandoDano", false);
    }

    private IEnumerator FadeSangue()
    {
        float duracao = 1.5f;
        float tempo = 0f;

        while (tempo < duracao)
        {
            float alpha = Mathf.Lerp(0.7f, 0f, tempo / duracao);
            sangueNaTela.color = new Color(corOriginal.r, corOriginal.g, corOriginal.b, alpha);
            tempo += Time.deltaTime;
            yield return null;
        }

        sangueNaTela.color = new Color(corOriginal.r, corOriginal.g, corOriginal.b, 0f);
    }

    void Morrer()
    {
        PlayerMorto = true;
        animator.SetTrigger("Morrer");

        // Desativa controle do player
        var movimento = GetComponent<Player>();
        if (movimento != null) movimento.enabled = false;

        var cc = GetComponent<CharacterController>();
        if (cc != null) cc.enabled = false;

        var rb = GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = true;

        // Começa a coroutine para mostrar painel após 3 segundos
        StartCoroutine(MostrarPainelDepois());
    }

    private IEnumerator MostrarPainelDepois()
    {
        yield return new WaitForSeconds(3f); // espera 3 segundos

        // Destrava o mouse para clicar nos botões
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Mostra painel de morte
        if (painelMorte != null)
            painelMorte.SetActive(true);
    }



    // Função do botão de respawn
    public void RespawnCena()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Função do botão de voltar para o menu
    public void VoltarMenu()
    {
        SceneManager.LoadScene("Menu"); // coloque o nome da sua cena de menu
    }
}
