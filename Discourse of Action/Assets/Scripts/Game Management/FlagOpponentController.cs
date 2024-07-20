using UnityEngine;

public class FlagOpponentController : Character, Interactable
{
    public GameFlags requiredFlag;
    public GameFlags flagToChange;

    public Dialogue wonDialogue;

    [Header("[REQUIRED FLAG ACTIVATED]")]
    public Dialogue trueDialogue;
    public DialogueTypes trueDialogueType;

    [Header("[REQUIRED FLAG NOT ACTIVATED]")]
    public Dialogue falseDialogue;
    public DialogueTypes falseDialogueType;

    public void Interact(Transform initiator)
    {
        LookTowards(initiator.position);

        EnemyData enemy = (EnemyData)_data;

        if (!enemy.hasWonAgainst)
        {
            if (GameManager.instance.IsFlagActivated(requiredFlag))
                ShowDialogue(trueDialogue, trueDialogueType);
            else
                ShowDialogue(falseDialogue, falseDialogueType);
        }
        else
            ShowWonDialogue();
    }

    void ShowDialogue(Dialogue selectedDialogue, DialogueTypes selectedDialogueType)
    {
        DialogueManager.instance.SetDialogueType(selectedDialogueType);
        DialogueManager.instance.StartDialogue(selectedDialogue);

        if (selectedDialogueType == DialogueTypes.DIALOGUE_COMBAT)
            GameManager.instance.enemyToBattle = (EnemyData)_data;

        if (flagToChange != GameFlags.Undefined)
            GameManager.instance.SetFlag(flagToChange);
    }

    public void ShowWonDialogue()
    {
        DialogueManager.instance.SetDialogueType(DialogueTypes.DIALOGUE_FLAVOR);
        DialogueManager.instance.StartDialogue(wonDialogue);
    }
}
