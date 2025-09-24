using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    public Receptacle[] receptacles;
    public string[] correctIDs; // Ex: ["Ra", "Anubis", "Isis"]
    public GameObject doorToOpen;

    public void CheckPuzzle()
    {
        int correctCount = 0;

        foreach (string id in correctIDs)
        {
            foreach (var receptacle in receptacles)
            {
                if (receptacle.GetSphereID() == id)
                {
                    correctCount++;
                    break;
                }
            }
        }

        if (correctCount == correctIDs.Length)
        {
            OpenDoor();
        }
    }

    void OpenDoor()
    {
        // Pode ser uma animação, desativar colisor, mover porta, etc
        Debug.Log("PUZZLE RESOLVIDO!");
        if (doorToOpen != null)
            doorToOpen.SetActive(false); // Exemplo simples: desativa a porta
    }
}
