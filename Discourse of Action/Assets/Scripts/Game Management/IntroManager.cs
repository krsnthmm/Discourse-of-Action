using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class IntroManager : MonoBehaviour
{
    [Header("UI ELEMENTS")]
    [SerializeField] private TMP_InputField nameInput;
    [SerializeField] private Button confirmButton;

    private void Start()
    {
        nameInput.text = "Indigo";
        confirmButton.interactable = false;
    }

    public void OnCharacterSelect(int index)
    {
        GameManager.instance.SelectCharacter(index);
        confirmButton.interactable = true;
    }

    public void OnNameConfirm()
    {
        GameManager.instance.SetName(nameInput.text);
    }
}
