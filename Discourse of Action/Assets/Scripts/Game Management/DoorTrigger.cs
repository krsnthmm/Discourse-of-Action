using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour, Interactable
{
    public GameObject objectToTrigger;

    void Start()
    {
        objectToTrigger.SetActive(false);
    }

    public void Interact(Transform initiator)
    {
        objectToTrigger.SetActive(true);
        gameObject.SetActive(false);
    }
}
