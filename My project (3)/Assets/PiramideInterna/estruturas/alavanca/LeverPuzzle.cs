using UnityEngine;

public class LeverPuzzle : MonoBehaviour
{
    public Lever[] levers;
    public int targetNumber = 13;
    public GameObject door;

    private string targetBinary;
    private bool puzzleSolved = false;

    void Start()
    {
        targetBinary = System.Convert.ToString(targetNumber, 2).PadLeft(levers.Length, '0');
    }

    public void CheckSolution()
    {
        if (puzzleSolved) return;

        string currentState = "";

        foreach (Lever lever in levers)
        {
            currentState += lever.isOn ? "1" : "0";
        }

        if (currentState == targetBinary)
        {
            puzzleSolved = true;
            OpenDoor();
        }
    }

    void OpenDoor()
    {
        door.transform.Translate(Vector3.up * 9f);
    }
}