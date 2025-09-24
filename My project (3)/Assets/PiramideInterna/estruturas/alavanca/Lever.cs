using UnityEngine;
using UnityEngine.UI;

public class Lever : MonoBehaviour
{
    public bool isOn = false;
    public LeverPuzzle puzzleManager;
    public Transform player;
    public float interactionDistance = 3f;
    public GameObject interactionUI; // Texto "Pressione E"

    private Vector3 originalRotation;
    private Vector3 downRotation;
    private bool playerNear = false;

    void Start()
    {
        originalRotation = transform.eulerAngles;
        downRotation = originalRotation + new Vector3(-45f, 0f, 0f);

        if (interactionUI != null)
            interactionUI.SetActive(false); // Esconde o texto no início
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position + Vector3.up, player.position + Vector3.up);
        playerNear = distance <= interactionDistance;

        if (interactionUI != null)
            interactionUI.SetActive(playerNear);

        if (playerNear && Input.GetKeyDown(KeyCode.E))
        {
            Toggle();
        }
    }

    public void Toggle()
    {
        isOn = !isOn;
        transform.eulerAngles = isOn ? downRotation : originalRotation;
        puzzleManager.CheckSolution();
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position + Vector3.up, interactionDistance);
    }
}
