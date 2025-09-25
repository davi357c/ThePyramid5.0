using UnityEngine;

public class LadderPickup : MonoBehaviour
{
    public KeyCode pickupKey = KeyCode.E;
    private bool playerInRange = false;

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
        if (playerInRange && Input.GetKeyDown(pickupKey))
        {
            PlayerInventory.hasLadder = true;
            gameObject.SetActive(false);      
        }
    }
}
