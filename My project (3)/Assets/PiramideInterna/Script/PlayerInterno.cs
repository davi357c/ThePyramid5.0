using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInterno : MonoBehaviour
{
    private CharacterController controller;
    private Animator animator;
    private Transform cameraTransform;
    private Espada espadaScript;

    public float velocidade = 5f;
    public float gravidade = -9.81f;
    public float forcaPulo = 5f;
    private Vector3 velocidadeVertical = Vector3.zero;
    private bool estaPulando;
    private bool estaAtacando = false;

    [Header("Detecção de chão")]
    public Transform checadorDeChao;
    public float raioChao = 0.3f;
    public LayerMask camadaDoChao;
    private bool estaNoChao;

    [Header("Interação")]
    public LayerMask camadaInterativa;
    public float alcanceInteracao = 3f;
    public Sphere carriedSphere;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        espadaScript = GetComponent<Espada>();

        if (espadaScript == null)
        {
            Debug.LogWarning("Script Espada não encontrado no Player.");
        }

        if (Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
        }
        else
        {
            Debug.LogWarning("Nenhuma câmera com a tag 'MainCamera' foi encontrada.");
        }
    }

    void Update()
    {
        if (cameraTransform == null) return;

        AnimatorStateInfo estado = animator.GetCurrentAnimatorStateInfo(0);
        if (estaAtacando)
        {
            if (estado.IsName("Attack") || estado.IsTag("Attack"))
            {
                if (estado.normalizedTime >= 0.95f)
                {
                    estaAtacando = false;
                }
            }
            else
            {
                estaAtacando = false;
            }
        }

        estaNoChao = Physics.CheckSphere(checadorDeChao.position, raioChao, camadaDoChao);

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 frenteCamera = cameraTransform.forward;
        Vector3 direitaCamera = cameraTransform.right;
        frenteCamera.y = 0f;
        direitaCamera.y = 0f;
        frenteCamera.Normalize();
        direitaCamera.Normalize();

        Vector3 movimento = (direitaCamera * horizontal + frenteCamera * vertical).normalized;

        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        bool isMoving = movimento.magnitude > 0.1f;
        float velocidadeAtual = isRunning ? velocidade * 4f : velocidade;

        if (estaNoChao && Input.GetButtonDown("Jump"))
        {
            velocidadeVertical.y = Mathf.Sqrt(forcaPulo * -2f * gravidade);
            estaPulando = true;
            animator.SetBool("Jumping", true);
        }

        if (!estaNoChao)
        {
            velocidadeVertical.y += gravidade * Time.deltaTime;

            if (velocidadeVertical.y < 0)
                animator.SetBool("Jumping", false);
        }
        else if (velocidadeVertical.y < 0)
        {
            velocidadeVertical.y = -0.1f;
        }

        Vector3 movimentoFinal = movimento * velocidadeAtual + velocidadeVertical;
        controller.Move(movimentoFinal * Time.deltaTime);

        if (isMoving)
        {
            Quaternion novaRotacao = Quaternion.LookRotation(movimento);
            transform.rotation = Quaternion.Slerp(transform.rotation, novaRotacao, Time.deltaTime * 10f);
        }

        animator.SetBool("Running", isMoving && isRunning);
        animator.SetBool("moving", isMoving && !isRunning);

        // Interação
        if (Input.GetKeyDown(KeyCode.E))
        {
            TentarInteragir();
        }

        // Ataque
        if (Input.GetMouseButtonDown(0) && espadaScript != null && espadaScript.EstaComEspadaNaMao() && !estaAtacando)
        {
            Atacar();
        }
    }

    void Atacar()
    {
        estaAtacando = true;
        animator.ResetTrigger("Attack");
        animator.SetTrigger("Attack");
        Debug.Log("Player atacou com a espada.");
    }

    // Função chamada no Animation Event para aplicar dano na hora certa
    public void AplicarDanoNoInimigo()
    {
        Debug.Log("AplicarDanoNoInimigo chamado");

        if (espadaScript == null)
        {
            Debug.LogWarning("EspadaScript é null");
            return;
        }

        int dano = espadaScript.dano;
        Debug.Log("Dano da espada: " + dano);

        Ray ray = new Ray(transform.position + Vector3.up * 0.5f, transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * 2f, Color.red, 1f);

        if (Physics.Raycast(ray, out RaycastHit hit, 2f))
        {
            Debug.Log("Raycast acertou: " + hit.collider.name);

            if (hit.collider.TryGetComponent(out VidaInimigo inimigo))
            {
                inimigo.TomarDano(dano);
                Debug.Log("Dano aplicado ao inimigo: " + dano);
            }
            else
            {
                Debug.Log("Objeto acertado não tem VidaInimigo");
            }
        }
        else
        {
            Debug.Log("Raycast não acertou nada");
        }
    }


    void TentarInteragir()
    {
        Ray ray = new Ray(transform.position + Vector3.up * 0.5f, transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * alcanceInteracao, Color.red, 2f);

        if (Physics.Raycast(ray, out RaycastHit hit, alcanceInteracao, camadaInterativa))
        {
            if (hit.collider.TryGetComponent(out Altar altar))
            {
                altar.Interact(this);
            }
            else if (hit.collider.TryGetComponent(out Receptacle receptacle))
            {
                receptacle.Interact(this);
            }
            else if (hit.collider.TryGetComponent(out LockTrigger fechadura))
            {
                fechadura.Interact();
            }
        }
    }
}
