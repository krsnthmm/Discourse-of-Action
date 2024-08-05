using System.Collections;
using UnityEngine;

public class OpponentController : Character, Interactable
{
    [SerializeField] GameObject exclamation;
    [SerializeField] GameObject FOV;

    public DialogueTypes dialogueType;
    public Dialogue dialogue;

    // no need for wonDialogueType as this is default flavor
    public Dialogue wonDialogue;

    public IEnumerator TriggerCombat(PlayerController player)
    {
        AudioManager.instance.PlayClip(AudioManager.instance.SFXSource, AudioManager.instance.opponentTriggerSFX);

        exclamation.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        exclamation.SetActive(false);

        var diff = 2 * (player.transform.position - transform.position);
        var moveVector = diff - diff.normalized;

        if (moveVector.x > 0.1f || moveVector.y > 0.1f)
        {
            HandleMovement(moveVector.x, moveVector.y, ShowDialogue);
            _renderer.SetBool("isWalking", _movement.isMoving);
        }
        else
            ShowDialogue();
    }

    public void ShowDialogue()
    {
        _renderer.SetBool("isWalking", _movement.isMoving);

        DialogueManager.instance.SetDialogueType(dialogueType);
        DialogueManager.instance.StartDialogue(dialogue);

        GameManager.instance.enemyToBattle = (EnemyData)_data;
    }

    public void ShowWonDialogue()
    {
        DialogueManager.instance.SetDialogueType(DialogueTypes.DIALOGUE_FLAVOR);
        DialogueManager.instance.StartDialogue(wonDialogue);
    }

    public void Interact(Transform initiator)
    {
        LookTowards(initiator.position);

        EnemyData enemy = (EnemyData)_data;

        if (!enemy.hasWonAgainst)
            ShowDialogue();
        else
            ShowWonDialogue();
    }

    public void SetFOVRotation(FacingDirection dir)
    {
        float angle = 0f;

        switch (dir)
        {
            case FacingDirection.DIR_RIGHT:
                angle = 90f;
                break;
            case FacingDirection.DIR_UP:
                angle = 180f;
                break;
            case FacingDirection.DIR_LEFT:
                angle = 270f;
                break;
        }

        FOV.transform.eulerAngles = new Vector3(0f, 0f, angle);
    }

    public override void HandleFacingDirection(FacingDirection dir)
    {
        var enemyData = (EnemyData)_data;

        _renderer.SetFacingDirection(dir);

        if (enemyData.hasWonAgainst)
            FOV.SetActive(false);
        else
            SetFOVRotation(dir);
    }
}
