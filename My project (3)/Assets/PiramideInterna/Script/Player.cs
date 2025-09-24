using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private CharacterController controller;
    private Transform cameraTransform;
    private Animator animator;

    public float velocidade = 5f;
    public float gravidade = -9.81f;
    public float forcaPulo = 5f;
    private Vector3 velocidadeVertical = Vector3.zero;
    private bool estaPulando;

    [Header("Detecção de chão")]
    public Transform checadorDeChao;
    public float raioChao = 0.3f;
    public LayerMask camadaDoChao;
    private bool estaNoChao;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        if (Camera.main != null)
        {
            cameraTransform = GameObject.Find("CameraRig").transform;
        }
        else
        {
            Debug.LogWarning("Nenhuma câmera com a tag 'MainCamera' foi encontrada.");
        }
    }

    void Update()
    {
        if (cameraTransform == null) return;

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

        Debug.DrawRay(transform.position, movimento * 2f, Color.red);

        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        bool isMoving = movimento.magnitude > 0.1f;
        float velocidadeAtual = isRunning ? velocidade * 4f : velocidade;

        // Pulo
        if (estaNoChao && Input.GetButtonDown("Jump"))
        {
            animator.SetBool("Jumping", true);
            
            velocidadeVertical.y = Mathf.Sqrt(forcaPulo * -2f * gravidade);
            estaPulando = true;
            
        }

        // Gravidade
        if (!estaNoChao)
        {
            velocidadeVertical.y += gravidade * Time.deltaTime;

            if (velocidadeVertical.y < 0)
            {
                animator.SetBool("Jumping", false); // Começou a cair
            }
        }
        else if (velocidadeVertical.y < 0)
        {
            velocidadeVertical.y = -0.1f;
        }

        // Combinar movimento horizontal e vertical
        Vector3 movimentoFinal = movimento * velocidadeAtual + velocidadeVertical;
        controller.Move(movimentoFinal * Time.deltaTime);

        if (isMoving)
        {
            Quaternion novaRotacao = Quaternion.LookRotation(movimento);
            transform.rotation = Quaternion.Slerp(transform.rotation, novaRotacao, Time.deltaTime * 10f);
        }

        // Resetar animação de pulo ao tocar o chão
    
        // Animações de movimento
        animator.SetBool("Running", isMoving && isRunning);
        animator.SetBool("moving", isMoving && !isRunning);
    }
}
