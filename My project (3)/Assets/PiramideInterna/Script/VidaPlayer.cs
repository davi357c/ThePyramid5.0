using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class VidaPlayer : MonoBehaviour
{
    public static bool PlayerMorto = false;  // Vari�vel est�tica para informar se o player morreu

    public int vidaMaxima = 100;
    public int vidaAtual;

    public Image sangueNaTela; // Refer�ncia � imagem de sangue na tela
    private Color corOriginal;

    private Animator animator;

    void Start()
    {
        vidaAtual = vidaMaxima;
        PlayerMorto = false;  // Reinicia a vari�vel quando o jogo come�a

        animator = GetComponent<Animator>();

        if (animator == null)
        {
            Debug.LogWarning("Animator n�o encontrado no VidaPlayer.");
        }

        if (sangueNaTela != null)
        {
            corOriginal = sangueNaTela.color;
            sangueNaTela.color = new Color(corOriginal.r, corOriginal.g, corOriginal.b, 0f); // come�a invis�vel
        }
    }

    public void TomarDano(int quantidade)
    {
        if (PlayerMorto) return; // Se j� morreu, n�o toma mais dano

        vidaAtual -= quantidade;
        Debug.Log($"Vida atual: {vidaAtual}");

        if (sangueNaTela != null)
        {
            sangueNaTela.color = new Color(corOriginal.r, corOriginal.g, corOriginal.b, 0.7f); // mostra sangue
            StartCoroutine(FadeSangue());
        }

        if (animator != null)
        {
            animator.SetBool("TomandoDano", true);
            StartCoroutine(ResetarTomarDano());
        }

        if (vidaAtual <= 0)
        {
            Morrer();
        }
    }

    private IEnumerator ResetarTomarDano()
    {
        yield return new WaitForSeconds(0.5f);
        animator.SetBool("TomandoDano", false);
    }

    void Morrer()
    {
        PlayerMorto = true; // Marca que o player morreu
        Debug.Log("Personagem morreu!");
        animator.SetTrigger("Morrer");

        // Desativa scripts que provavelmente controlam o movimento
        var movimento = GetComponent<MonoBehaviour>(); // Para voc� substituir pelo seu script real

        // Exemplo de desativar um script espec�fico, mude o nome para seu script de movimento
        var scriptMovimento = GetComponent<PlayerInterno>(); // Substitua PlayerMovement pelo nome correto
        if (scriptMovimento != null)
        {
            scriptMovimento.enabled = false;
        }

        // Se usa CharacterController sem script de movimento, talvez queira desativar componente:
        var cc = GetComponent<CharacterController>();
        if (cc != null)
        {
            cc.enabled = false;
        }

        // Se usa Rigidbody com controle manual:
        var rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true; // Para travar a f�sica e impedir movimento
        }
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
}
    