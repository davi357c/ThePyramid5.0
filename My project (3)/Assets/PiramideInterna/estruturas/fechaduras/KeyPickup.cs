using UnityEngine;

public class KeyPickup : MonoBehaviour
{
    public enum KeyColor { Red, Green, Blue }
    public KeyColor keyColor;

    public static bool hasRedKey = false;
    public static bool hasGreenKey = false;
    public static bool hasBlueKey = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            switch (keyColor)
            {
                case KeyColor.Red:
                    hasRedKey = true;
                    Debug.Log("Pegou a chave vermelha!");
                    break;
                case KeyColor.Green:
                    hasGreenKey = true;
                    Debug.Log("Pegou a chave verde!");
                    break;
                case KeyColor.Blue:
                    hasBlueKey = true;
                    Debug.Log("Pegou a chave azul!");
                    break;
            }
            Destroy(gameObject);
        }
    }
}
