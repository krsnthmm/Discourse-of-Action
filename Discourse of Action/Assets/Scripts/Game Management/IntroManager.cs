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
        SelectCharacter(index);
        confirmButton.interactable = true;
    }

    public void OnNameConfirm()
    {
        SetName(nameInput.text);
    }

    void SelectCharacter(int index) // 0 = masc, 1 = fem
    {
        AudioManager.instance.PlayClip(AudioManager.instance.SFXSource, AudioManager.instance.buttonSFX);

        GameManager.instance.currentPlayerData.selectedCharacter = (PlayerData.Character)index;
        GameManager.instance.currentPlayerData.SetAnimatorController();
        GameManager.instance.finalBossData.SetAnimatorController((int)GameManager.instance.currentPlayerData.selectedCharacter);
    }

    void SetName(string name)
    {
        AudioManager.instance.PlayClip(AudioManager.instance.SFXSource, AudioManager.instance.buttonSFX);

        GameManager.instance.currentPlayerData.characterName = name;
        GameManager.instance.ChangeState(GameState.GAME_OVERWORLD);
    }
}
