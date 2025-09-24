using UnityEngine;

public class Receptacle : MonoBehaviour
{
    public Sphere currentSphere;
    public PuzzleManager puzzleManager;

    public void Interact(PlayerInterno player)
    {
        if (currentSphere == null && player.carriedSphere != null)
        {
            // Coloca esfera no recept�culo
            currentSphere = player.carriedSphere;
            currentSphere.transform.position = transform.position + Vector3.up * 0.1f;
            currentSphere.gameObject.SetActive(true);
            currentSphere.isHeld = false;
            player.carriedSphere = null;

            puzzleManager.CheckPuzzle();
        }
        else if (currentSphere != null && player.carriedSphere == null)
        {
            // Pega esfera de volta
            player.carriedSphere = currentSphere;
            currentSphere.gameObject.SetActive(false);
            currentSphere.isHeld = true;
            currentSphere = null;

            puzzleManager.CheckPuzzle();
        }
    }

    public string GetSphereID()
    {
        return currentSphere != null ? currentSphere.sphereID : null;
    }
}
