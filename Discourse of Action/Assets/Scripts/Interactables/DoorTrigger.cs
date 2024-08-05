using UnityEngine;

public class DoorTrigger : MonoBehaviour, Interactable
{
    public void Interact(Transform initiator)
    {
        gameObject.SetActive(false);
    }
}
