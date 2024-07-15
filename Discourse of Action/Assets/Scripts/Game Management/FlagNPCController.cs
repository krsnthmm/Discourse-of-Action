using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagNPCController : Character, Interactable
{
    public GameFlags requiredFlag;

    [Header("[REQUIRED FLAG ACTIVATED]")]
    public Dialogue trueDialogue;
    public DialogueTypes trueDialogueType;

    [Header("[REQUIRED FLAG NOT ACTIVATED]")]
    public Dialogue falseDialogue;
    public DialogueTypes falseDialogueType;

    public void Interact(Transform initiator)
    {
        LookTowards(initiator.position);

        if (GameManager.instance.IsFlagActivated(requiredFlag))
            ShowDialogue(trueDialogue, trueDialogueType);
        else
            ShowDialogue(falseDialogue, falseDialogueType);
    }

    void ShowDialogue(Dialogue selectedDialogue, DialogueTypes selectedDialogueType)
    {
        DialogueManager.instance.SetDialogueType(selectedDialogueType);
        DialogueManager.instance.StartDialogue(selectedDialogue);

        if (selectedDialogueType == DialogueTypes.DIALOGUE_COMBAT)
            GameManager.instance.enemyToBattle = (EnemyData)_data;
    }
}
