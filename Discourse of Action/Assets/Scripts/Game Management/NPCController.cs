using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : Character, Interactable
{
    public Dialogue dialogue;

    public void Interact(Transform initiator)
    {
        LookTowards(initiator.position);
        DialogueManager.instance.StartDialogue(dialogue);
    }
}
