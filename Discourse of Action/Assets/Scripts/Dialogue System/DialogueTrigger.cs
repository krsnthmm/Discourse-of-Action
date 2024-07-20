using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;

    public DialogueTypes dialogueType;
    public Dialogue dialogue;

    public void TriggerDialogue()
    {
        DialogueManager.instance.SetDialogueType(dialogueType);
        DialogueManager.instance.StartDialogue(dialogue);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // TODO: UI Prompt
            Debug.Log("!!");
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                TriggerDialogue();
                Debug.Log("Button Pressed");
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // TODO: Remove UI Prompt
        }
    }
}

