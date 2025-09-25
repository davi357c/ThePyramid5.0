using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private CharacterController controller;
    private Transform cameraTransform;
    private Animator animator;

    [Header("Movimento")]
    public float velocidadeBase = 4f;          // velocidade de caminhada
    public float multiplicadorCorrida = 1.3f; // corrida levemente mais rápida
    public float gravidade = -9.81f;
    public float forcaPulo = 0f;              // ajuste a força do pulo
    private Vector3 velocidadeVertical = Vector3.zero;

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
            cameraTransform = GameObject.Find("CameraRig").transform;
        else
            Debug.LogWarning("Nenhuma câmera com a tag 'MainCamera' foi encontrada.");
    }

    void Update()
    {
        if (cameraTransform == null) return;

        // Checagem do chão
        estaNoChao = Physics.CheckSphere(checadorDeChao.position, raioChao, camadaDoChao);

        // Entrada do jogador
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Direções da câmera
        Vector3 frenteCamera = cameraTransform.forward;
        Vector3 direitaCamera = cameraTransform.right;
        frenteCamera.y = 0f;
        direitaCamera.y = 0f;
        frenteCamera.Normalize();
        direitaCamera.Normalize();

        // Vetor de movimento
        Vector3 movimento = (direitaCamera * horizontal + frenteCamera * vertical);
        if (movimento.magnitude > 1f) movimento.Normalize(); // evita velocidade maior na diagonal

        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        bool isMoving = movimento.magnitude > 0.1f;

        // Velocidade horizontal
        float velocidadeAtual = isRunning ? velocidadeBase * multiplicadorCorrida : velocidadeBase;
        Vector3 movimentoHorizontal = movimento * velocidadeAtual;

        // Gravidade e pulo
        if (estaNoChao)
        {
            if (velocidadeVertical.y < 0)
                velocidadeVertical.y = -0.1f;

            if (Input.GetButtonDown("Jump"))
                velocidadeVertical.y = Mathf.Sqrt(forcaPulo * -2f * gravidade);
        }
        else
        {
            velocidadeVertical.y += gravidade * Time.deltaTime;
        }

        // Combina movimento horizontal e vertical
        Vector3 movimentoFinal = movimentoHorizontal + velocidadeVertical;
        controller.Move(movimentoFinal * Time.deltaTime);

        // Rotação do player
        if (isMoving)
        {
            Quaternion novaRotacao = Quaternion.LookRotation(movimento);
            transform.rotation = Quaternion.Slerp(transform.rotation, novaRotacao, Time.deltaTime * 10f);
        }

        // Animações
        animator.SetBool("Running", isMoving && isRunning);
        animator.SetBool("moving", isMoving && !isRunning);
        animator.SetBool("Jumping", !estaNoChao);
    }
}
