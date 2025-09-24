using UnityEngine;

public class Altar : MonoBehaviour
{
    public Sphere currentSphere;

    public void Interact(PlayerInterno player)
    {
        Debug.Log("Intera��o com altar iniciada");


        if (currentSphere != null && player.carriedSphere == null)
        {
            Debug.Log("Pegando esfera do altar: " + currentSphere.name);
            player.carriedSphere = currentSphere;
            currentSphere.gameObject.SetActive(false);
            currentSphere.isHeld = true;
            currentSphere = null;
        }
        else if (currentSphere == null && player.carriedSphere != null)
        {
            Debug.Log("Devolvendo esfera ao altar");
            player.carriedSphere.transform.position = transform.position + Vector3.up * 0.88f;
            player.carriedSphere.gameObject.SetActive(true);
            player.carriedSphere.isHeld = false;
            currentSphere = player.carriedSphere;
            player.carriedSphere = null;
        }
        else
        {
            Debug.Log("Condi��es para intera��o n�o atendidas.");
        }
    }

}

