using UnityEngine;

public class NPCController : Character, Interactable
{
    public DialogueTypes dialogueType;
    public Dialogue dialogue;

    public void Interact(Transform initiator)
    {
        LookTowards(initiator.position);
        DialogueManager.instance.SetDialogueType(dialogueType);
        DialogueManager.instance.StartDialogue(dialogue);
    }
}
