using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class VidaPlayer : MonoBehaviour
{
    public static bool PlayerMorto = false;  // Variável estática para informar se o player morreu

    public int vidaMaxima = 100;
    public int vidaAtual;

    public Image sangueNaTela; // Referência à imagem de sangue na tela
    private Color corOriginal;

    private Animator animator;

    public Transform pontoDeRespawn; // Atribuir no Inspector
    public float tempoParaRespawn = 3f; // Quantos segundos depois de morrer
    private Vector3 posicaoInicial;
    private Quaternion rotacaoInicial;


    void Start()
    {
        vidaAtual = vidaMaxima;
        PlayerMorto = false;  // Reinicia a variável quando o jogo começa
        posicaoInicial = transform.position;
        rotacaoInicial = transform.rotation;


        animator = GetComponent<Animator>();

        if (animator == null)
        {
            Debug.LogWarning("Animator não encontrado no VidaPlayer.");
        }

        if (sangueNaTela != null)
        {
            corOriginal = sangueNaTela.color;
            sangueNaTela.color = new Color(corOriginal.r, corOriginal.g, corOriginal.b, 0f); // começa invisível
        }
    }

    public void TomarDano(int quantidade)
    {
        if (PlayerMorto) return; // Se já morreu, não toma mais dano

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
        PlayerMorto = true;
        Debug.Log("Personagem morreu!");
        animator.SetTrigger("Morrer");

        // Desativa controle
        var movimento = GetComponent<Player>();
        if (movimento != null) movimento.enabled = false;

        var cc = GetComponent<CharacterController>();
        if (cc != null) cc.enabled = false;

        var rb = GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = true;

        // Resetar inimigos ao morrer
        EnemyManager.Instance?.ResetarInimigos();

        // Começa o respawn
        StartCoroutine(Respawn());
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

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(tempoParaRespawn);

        // Reposicionar o player
        if (pontoDeRespawn != null)
        {
            transform.position = pontoDeRespawn.position;
            transform.rotation = pontoDeRespawn.rotation;
        }
        else
        {
            transform.position = posicaoInicial;
            transform.rotation = rotacaoInicial;
        }

        // Restaurar vida
        vidaAtual = vidaMaxima;
        PlayerMorto = false;

        // Reativar controle
        var movimento = GetComponent<Player>();
        if (movimento != null) movimento.enabled = true;

        var cc = GetComponent<CharacterController>();
        if (cc != null) cc.enabled = true;

        var rb = GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = false;

        animator.Rebind(); // Resetar animações
        animator.Update(0f);
    }

}
