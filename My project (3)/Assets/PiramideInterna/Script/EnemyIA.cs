using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyChaseSimple : MonoBehaviour
{
    [Header("Referências")]
    public Transform player;
    private NavMeshAgent agent;
    private Animator animator;

    [Header("Movimentos")]
    public float walkSpeed = 2f;
    public float runSpeed = 5f;
    public float runDistance = 10f;
    public float attackDistance = 2f;

    [Header("Ataque")]
    public int dano = 20;
    public float attackDelay = 0.5f;
    private bool isAttacking = false;

    private Vector3 spawnPosition; // posição inicial do inimigo

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        agent.stoppingDistance = attackDistance;

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
            rb.isKinematic = true;

        // Salva a posição inicial como spawn
        spawnPosition = transform.position;
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance > runDistance)
        {
            agent.speed = walkSpeed;
            animator.SetBool("isWalking", true);
            animator.SetBool("isRunning", false);
            agent.SetDestination(player.position);
            isAttacking = false;
        }
        else if (distance > attackDistance)
        {
            agent.speed = runSpeed;
            animator.SetBool("isWalking", false);
            animator.SetBool("isRunning", true);
            agent.SetDestination(player.position);
            isAttacking = false;
        }
        else
        {
            if (!isAttacking)
            {
                StartCoroutine(Ataque());
            }
        }
    }

    private IEnumerator Ataque()
    {
        isAttacking = true;

        // Para o NavMeshAgent
        agent.ResetPath();
        agent.velocity = Vector3.zero;

        // Para animações de movimento
        animator.SetBool("isWalking", false);
        animator.SetBool("isRunning", false);

        // Dispara trigger de ataque
        animator.SetTrigger("attackTrigger");

        // Espera até o momento do impacto do ataque
        yield return new WaitForSeconds(attackDelay);

        if (player != null && Vector3.Distance(transform.position, player.position) <= attackDistance)
        {
            VidaPlayer vida = player.GetComponent<VidaPlayer>();
            if (vida != null)
                vida.TomarDano(dano);
        }

        // Espera até a animação terminar
        // Agora usamos o tempo real do Animator em vez de uma variável fixa
        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);
        float remainingTime = state.length * (1 - state.normalizedTime % 1); // tempo restante
        yield return new WaitForSeconds(remainingTime);

        isAttacking = false;
    }


    // Função simples para resetar o inimigo quando o player respawnar
    public void ResetInimigo()
    {
        StopAllCoroutines();
        isAttacking = false;

        agent.enabled = false;
        transform.position = spawnPosition; // volta para a posição inicial
        agent.enabled = true;
        agent.ResetPath();

        animator.SetBool("isWalking", false);
        animator.SetBool("isRunning", false);
    }

    // Chame essa função quando o player respawnar
    public void OnPlayerRespawn()
    {
        ResetInimigo();
    }
}
