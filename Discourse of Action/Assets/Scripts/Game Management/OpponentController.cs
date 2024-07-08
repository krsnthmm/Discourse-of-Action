using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpponentController : Character, Interactable
{
    [SerializeField] GameObject exclamation;
    [SerializeField] GameObject FOV;

    public DialogueTypes dialogueType;
    public Dialogue dialogue;

    public IEnumerator TriggerCombat(PlayerController player)
    {
        exclamation.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        exclamation.SetActive(false);

        var diff = 2 * (player.transform.position - transform.position);
        var moveVector = diff - diff.normalized;

        HandleMovement(moveVector.x, moveVector.y, ShowDialogue);
        _renderer.SetBool("isWalking", _movement.isMoving);
    }

    public void ShowDialogue()
    {
        _renderer.SetBool("isWalking", _movement.isMoving);

        DialogueManager.instance.dialogueType = dialogueType;
        DialogueManager.instance.StartDialogue(dialogue);

        GameManager.instance.enemyToBattle = (EnemyData)_data;
    }

    public void Interact(Transform initiator)
    {
        LookTowards(initiator.position);
        ShowDialogue();
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
        _renderer.SetFacingDirection(dir);
        SetFOVRotation(dir);
    }
}
