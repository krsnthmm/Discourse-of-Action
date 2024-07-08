using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : Character, Interactable
{
    public DialogueTypes dialogueType;
    public Dialogue dialogue;

    public void Interact(Transform initiator)
    {
        LookTowards(initiator.position);

        DialogueManager.instance.dialogueType = dialogueType;
        DialogueManager.instance.StartDialogue(dialogue);

        if (dialogueType == DialogueTypes.DIALOGUE_COMBAT)
            GameManager.instance.enemyToBattle = (EnemyData)_data;
    }
}
