using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum DialogueTypes
{
    DIALOGUE_FLAVOR,
    DIALOGUE_COMBAT,
    DIALOGUE_STORY,
    DIALOGUE_RECALL,
    DIALOGUE_TOTAL
}

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;


    public bool isInDialogue;

    public Animator animator;
    public Image characterSprite;
    public TMP_Text nameText;
    public TMP_Text dialogueText;

    //public GameObject guiGroup;

    private Queue<DialogueLine> _lines = new();
    private Coroutine _typeLineCoroutine = null;
    private DialogueLine _lastLine; // stores the last dequeued sentence; needed for sentence completion
    private DialogueTypes _dialogueType;

    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _buttonClip;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
            instance = this;
    }

    public void StartDialogue(Dialogue dialogue)
    {
        if (!isInDialogue)
        {
            isInDialogue = true;

            if (_dialogueType == DialogueTypes.DIALOGUE_STORY)
                characterSprite.gameObject.SetActive(true);
            else
                characterSprite.gameObject.SetActive(false);

            animator.SetBool("IsOpen", true);
            _lines.Clear();

            foreach (DialogueLine dialogueLine in dialogue.dialogueLines)
            {
                _lines.Enqueue(dialogueLine);
            }

            DisplayNextLine();
        }
    }

    public void OnContinueButtonClick()
    {
        AudioManager.instance.PlayClip(AudioManager.instance.SFXSource, AudioManager.instance.buttonSFX);

        // check if a sentence is currently being typed
        if (_typeLineCoroutine != null)
        {
            StopAllCoroutines();
            _typeLineCoroutine = null;
            dialogueText.text = _lastLine.line;
        }
        else
            DisplayNextLine();
    }

    void DisplayNextLine()
    {
        if (_lines.Count == 0)
        {
            EndDialogue();
            return;
        }

        DialogueLine currentLine = _lines.Dequeue();

        if (currentLine.character.icon != null && characterSprite.gameObject.activeSelf)
            characterSprite.sprite = currentLine.character.icon;

        nameText.text = currentLine.character.name;

        _lastLine = currentLine;
        _typeLineCoroutine = StartCoroutine(TypeSentence(currentLine));
    }

    IEnumerator TypeSentence(DialogueLine dialogueLine)
    {
        dialogueText.text = "";

        foreach (char letter in dialogueLine.line.ToCharArray())
        {
            AudioManager.instance.PlayClip(AudioManager.instance.SFXSource, AudioManager.instance.dialogueSFX);

            dialogueText.text += letter;
            yield return new WaitForSeconds(0.1f);
        }

        _typeLineCoroutine = null;
    }

    void EndDialogue()
    {
        Debug.Log("End of conversation.");

        isInDialogue = false;
        //guiGroup.SetActive(true);

        animator.SetBool("IsOpen", false);

        switch (_dialogueType)
        {
            case DialogueTypes.DIALOGUE_COMBAT:
                GameManager.instance.ChangeState(GameState.GAME_BATTLE);
                break;
        }
    }

    public void SetDialogueType(DialogueTypes selectedType)
    {
        _dialogueType = selectedType;
    }
}
