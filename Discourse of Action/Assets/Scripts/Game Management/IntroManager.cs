using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class IntroManager : MonoBehaviour
{
    [Header("UI ELEMENTS")]
    public TMP_InputField nameInput;

    private void Start()
    {
        nameInput.text = "Indigo";
    }

    public void OnCharacterSelect(int index)
    {
        GameManager.instance.SelectCharacter(index);
        Debug.Log("Selected Character: " + index);
    }

    public void OnNameConfirm()
    {
        GameManager.instance.SetName(nameInput.text);
    }
}
