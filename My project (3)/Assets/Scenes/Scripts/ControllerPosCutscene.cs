using UnityEngine;
using System.Collections;

public class ControllerPosCutscene : MonoBehaviour
{
    public GameObject Player; // Prefab do player
    public Transform spawnPoint; // Ponto onde o player vai aparecer
    public float delay = 31f; // Tempo de espera em segundos

    void Start()
    {
        StartCoroutine(SpawnPlayerAfterDelay());
    }

    IEnumerator SpawnPlayerAfterDelay()
    {
        yield return new WaitForSeconds(delay);
        Instantiate(Player, spawnPoint.position, spawnPoint.rotation);
    }
}
