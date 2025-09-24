using UnityEngine;

public class VidaInimigo : MonoBehaviour
{
    public int vida = 30;
    private bool estaMorto = false;
    private Animator animator;
    private EnemyFov inimigoControle; // Seu script de controle de movimento/ataque do inimigo

    void Start()
    {
        animator = GetComponent<Animator>();
        inimigoControle = GetComponent<EnemyFov>(); // Ajuste para seu script de controle
        if (animator == null)
        {
            Debug.LogWarning("Animator não encontrado no inimigo.");
        }
    }

    public void TomarDano(int dano)
    {
        if (estaMorto) return;

        vida -= dano;
        Debug.Log("Inimigo tomou dano: " + dano);

        if (vida <= 0)
        {
            Morrer();
        }
        else
        {
            // Dispara animação de Hit se ainda não morreu
            if (animator != null)
            {
                animator.SetTrigger("Hit");
            }
        }
    }

    void Morrer()
    {
        estaMorto = true;

        if (animator != null)
        {
            animator.SetBool("Morto", true);
        }

        if (inimigoControle != null)
        {
            inimigoControle.enabled = false;
        }

        // Desativa o collider
        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            col.enabled = false;
        }
    }

}
