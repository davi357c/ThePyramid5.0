using UnityEngine;

public class LockTrigger : MonoBehaviour
{
    public enum LockColor { Red, Green, Blue }
    public LockColor lockColor;

    public GameObject doorToOpen;

    public void Interact()
    {
        bool hasKey = false;

        switch (lockColor)
        {
            case LockColor.Red:
                hasKey = KeyPickup.hasRedKey;
                break;
            case LockColor.Green:
                hasKey = KeyPickup.hasGreenKey;
                break;
            case LockColor.Blue:
                hasKey = KeyPickup.hasBlueKey;
                break;
        }

        if (hasKey)
        {
            Debug.Log($"Fechadura {lockColor} aberta com chave!");
            doorToOpen.transform.Translate(Vector3.up * 30f);
        }
        else
        {
            Debug.Log($"Você não tem a chave {lockColor}!");
        }
    }
}
