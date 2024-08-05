using UnityEngine;

public class FlavorTextTrigger : MonoBehaviour, Interactable
{
    public Dialogue dialogue;

    public void Interact(Transform initiator)
    {
        DialogueManager.instance.SetDialogueType(DialogueTypes.DIALOGUE_FLAVOR);
        DialogueManager.instance.StartDialogue(dialogue);
    }
}
