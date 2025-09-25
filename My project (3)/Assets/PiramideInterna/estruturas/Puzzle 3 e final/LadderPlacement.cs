using UnityEngine;

public class LadderPlacement : MonoBehaviour
{
    [Header("Referências")]
    public GameObject ghostLadder;  // filho visível desde o início
    public GameObject realLadder;   // filho desativado
    public KeyCode placeKey = KeyCode.E;

    private bool playerInRange = false;
    private bool ladderPlaced = false; // garante que a escada só é colocada uma vez

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInRange = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInRange = false;
    }

    private void Update()
    {
        if (!playerInRange || ladderPlaced)
            return; // nunca mais reage a input após colocar

        if (Input.GetKeyDown(placeKey) && PlayerInventory.hasLadder)
        {
            PlaceLadder();
        }
    }


    private void PlaceLadder()
    {
        ghostLadder.SetActive(false);   // some a ghost
        realLadder.SetActive(true);     // ativa a escada real
        PlayerInventory.hasLadder = false; // consome a escada do inventário
        ladderPlaced = true;            // marca como colocada
    }
}
