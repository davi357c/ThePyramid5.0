using UnityEngine;

public class MirrorObjectY : MonoBehaviour
{
    public Transform targetToMirror; // arraste o chão aqui
    public float offsetY = 5f;       // altura do teto em relação ao chão

    void Start()
    {
        if (targetToMirror == null)
        {
            Debug.LogError("Atribua o objeto que será espelhado.");
            return;
        }

        // Cria uma cópia
        GameObject mirrored = Instantiate(targetToMirror.gameObject);

        // Nome útil
        mirrored.name = targetToMirror.name + "_Mirrored";

        // Inverte no eixo Y com rotação
        mirrored.transform.rotation = targetToMirror.rotation * Quaternion.Euler(180, 0, 0);

        // Mantém escala original
        mirrored.transform.localScale = targetToMirror.localScale;

        // Coloca no alto, espelhado em relação à posição original
        Vector3 originalPos = targetToMirror.position;
        mirrored.transform.position = new Vector3(originalPos.x, originalPos.y + offsetY, originalPos.z);
    }
}
