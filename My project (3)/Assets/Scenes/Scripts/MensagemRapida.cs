using TMPro;
using UnityEngine;

public class MensagemRapida : MonoBehaviour
{
    public GameObject mensagemTexto; // Agora � o GameObject, n�o o componente de texto

    void Start()
    {
        Invoke(nameof(AtivarTexto), 45f);
    }

    void AtivarTexto()
    {
        mensagemTexto.SetActive(true);
        Invoke(nameof(DesativarTexto), 3f);
    }

    void DesativarTexto()
    {
        mensagemTexto.SetActive(false);
    }
}
