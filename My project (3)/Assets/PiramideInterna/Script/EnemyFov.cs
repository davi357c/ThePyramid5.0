using UnityEngine;
using UnityEngine.AI;

public class EnemyFov : MonoBehaviour
{
    public Transform jogador;
    public float raioVisao = 15f;
    public float anguloVisao = 360f;
    public float distanciaAtaque = 2.5f;
    public LayerMask obstaculos;

    private NavMeshAgent agente;
    private Animator animator;

    private bool playerAvistado = false;
    private bool estaAndando = false;
    private bool podeAtacar = true;
    public float tempoEntreAtaques = 1.5f;
    private float cronometroAtaque = 0f;

    void Start()
    {
        agente = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        if (jogador == null)
            Debug.LogWarning("Jogador não está atribuído no inspetor!");

        if (animator == null)
            Debug.LogError("Animator não encontrado no objeto!");
    }

    void Update()
    {
        // Se o player morreu, inimigos param de andar e atacar
        if (VidaPlayer.PlayerMorto)
        {
            agente.isStopped = true;
            animator.ResetTrigger("StartWalking");
            animator.SetTrigger("StopWalking");
            return;
        }

        if (jogador == null) return;

        Vector3 direcaoJogador = jogador.position - transform.position;
        direcaoJogador.y = 0;
        Vector3 forward = transform.forward;
        forward.y = 0;

        float distancia = direcaoJogador.magnitude;
        bool viuJogador = false;

        if (distancia < raioVisao)
        {
            float angulo = Vector3.Angle(forward, direcaoJogador);

            if (angulo < anguloVisao / 2f)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, direcaoJogador.normalized, out hit, distancia, ~obstaculos))
                {
                    if (hit.transform == jogador)
                    {
                        viuJogador = true;
                    }
                }
            }
        }

        if (viuJogador)
        {
            playerAvistado = true;

            if (distancia > distanciaAtaque)
            {
                agente.isStopped = false;
                agente.SetDestination(jogador.position);

                if (!estaAndando)
                {
                    animator.ResetTrigger("StopWalking");
                    animator.SetTrigger("StartWalking");
                    estaAndando = true;
                }
            }
            else
            {
                agente.isStopped = true;

                if (podeAtacar)
                {
                    animator.SetTrigger("Attack");
                    podeAtacar = false;
                    cronometroAtaque = tempoEntreAtaques;
                }

                if (estaAndando)
                {
                    animator.ResetTrigger("StartWalking");
                    animator.SetTrigger("StopWalking");
                    estaAndando = false;
                }
            }
        }
        else
        {
            if (playerAvistado)
            {
                playerAvistado = false;
                agente.isStopped = true;
                agente.ResetPath();

                MusicManager.Instance?.PlayInternoMusic();
            }

            if (estaAndando)
            {
                animator.ResetTrigger("StartWalking");
                animator.SetTrigger("StopWalking");
                estaAndando = false;
            }
        }

        if (!podeAtacar)
        {
            cronometroAtaque -= Time.deltaTime;
            if (cronometroAtaque <= 0f)
                podeAtacar = true;
        }
    }

    public void DarDano()
    {
        // Se o player morreu, não dá dano
        if (VidaPlayer.PlayerMorto) return;

        if (jogador == null) return;

        float distanciaAtual = Vector3.Distance(transform.position, jogador.position);

        if (distanciaAtual <= distanciaAtaque)
        {
            if (!Physics.Raycast(transform.position + Vector3.up,
                                 (jogador.position - transform.position).normalized,
                                 distanciaAtual, obstaculos))
            {
                Debug.Log("Inimigo causou dano!");
                VidaPlayer playerVida = jogador.GetComponent<VidaPlayer>();
                if (playerVida != null)
                {
                    playerVida.TomarDano(10);
                }
            }
            else
            {
                Debug.Log("Ataque bloqueado por obstáculo.");
            }
        }
        else
        {
            Debug.Log("Jogador saiu do alcance antes do dano.");
        }
    }
}
