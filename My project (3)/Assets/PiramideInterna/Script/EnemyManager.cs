using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance;

    private List<EnemyData> inimigos = new List<EnemyData>();

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void RegistrarInimigo(GameObject inimigo)
    {
        EnemyData dados = new EnemyData
        {
            inimigoObj = inimigo,
            posicaoInicial = inimigo.transform.position,
            rotacaoInicial = inimigo.transform.rotation
        };

        inimigos.Add(dados);
    }

    public void ResetarInimigos()
    {
        foreach (var inimigo in inimigos)
        {
            inimigo.inimigoObj.transform.position = inimigo.posicaoInicial;
            inimigo.inimigoObj.transform.rotation = inimigo.rotacaoInicial;

            // Resetar componentes
            var nav = inimigo.inimigoObj.GetComponent<UnityEngine.AI.NavMeshAgent>();
            if (nav != null)
            {
                nav.isStopped = false;
                nav.ResetPath();
            }

            var anim = inimigo.inimigoObj.GetComponent<Animator>();
            if (anim != null)
            {
                anim.Rebind(); // Reseta o Animator
                anim.Update(0f);
            }

            // Opcional: resetar scripts
            var fov = inimigo.inimigoObj.GetComponent<EnemyFov>();
            if (fov != null)
            {
                fov.enabled = true;
            }
        }
    }

    private class EnemyData
    {
        public GameObject inimigoObj;
        public Vector3 posicaoInicial;
        public Quaternion rotacaoInicial;
    }
}
