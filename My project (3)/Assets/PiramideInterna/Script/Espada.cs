using UnityEngine;

public class Espada : MonoBehaviour
{
    public Transform handTransform;
    public Transform cinturaTransform;
    public GameObject espadaNaMaoPrefab;
    public float distanciaColeta = 2f;

    [Header("Ajustes de posição e rotação")]
    public Vector3 posicaoOffsetMao = Vector3.zero;
    public Vector3 rotacaoOffsetMao = new Vector3(100f, 180f, 0f);

    public Vector3 posicaoOffsetCintura = Vector3.zero;
    public Vector3 rotacaoOffsetCintura = Vector3.zero;
    public Vector3 escalaEspada = new Vector3(0.05f, 0.05f, 0.05f);

    [Header("Configuração de Dano")]
    public int dano = 10; // <-- variável pública para o dano da espada

    private GameObject espadaEquipada;
    private bool pegouEspada = false;
    private bool espadaEquipadaNaMao = false;

    void Update()
    {
        if (!pegouEspada && Input.GetKeyDown(KeyCode.E))
        {
            TentarPegarEspada();
        }

        if (pegouEspada && Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (!espadaEquipadaNaMao)
                EquiparEspada();
            else
                DesequiparEspada();
        }
    }

    void TentarPegarEspada()
    {
        Collider[] objetosProximos = Physics.OverlapSphere(transform.position, distanciaColeta);

        foreach (Collider objeto in objetosProximos)
        {
            if (objeto.CompareTag("Espada"))
            {
                Destroy(objeto.gameObject);
                pegouEspada = true;

                espadaEquipada = Instantiate(espadaNaMaoPrefab, cinturaTransform);
                AjustarTransform(espadaEquipada.transform, posicaoOffsetCintura, rotacaoOffsetCintura, escalaEspada);
                Debug.Log("Espada coletada e guardada na cintura.");
                return;
            }
        }

        Debug.Log("Nenhuma espada próxima.");
    }

    void EquiparEspada()
    {
        if (espadaEquipada == null) return;

        espadaEquipada.transform.SetParent(handTransform);
        AjustarTransform(espadaEquipada.transform, posicaoOffsetMao, rotacaoOffsetMao, escalaEspada);

        espadaEquipadaNaMao = true;
        Debug.Log("Espada equipada na mão.");
    }

    void DesequiparEspada()
    {
        if (espadaEquipada == null) return;

        espadaEquipada.transform.SetParent(cinturaTransform);
        AjustarTransform(espadaEquipada.transform, posicaoOffsetCintura, rotacaoOffsetCintura, escalaEspada);

        espadaEquipadaNaMao = false;
        Debug.Log("Espada guardada na cintura.");
    }

    void AjustarTransform(Transform t, Vector3 posOffset, Vector3 rotOffset, Vector3 escala)
    {
        t.localPosition = posOffset;
        t.localRotation = Quaternion.Euler(rotOffset);
        t.localScale = escala;
    }

    public bool EstaComEspadaNaMao()
    {
        return pegouEspada && espadaEquipadaNaMao;
    }

    // Getter para dano (opcional, mas útil se quiser controlar acesso)
    public int GetDano()
    {
        return dano;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, distanciaColeta);
    }
}
