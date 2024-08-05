using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [Header("[COMPONENTS]")]
    [SerializeField] private Canvas mainMenuGroup;
    [SerializeField] private Canvas mainMenuBackButton;
    [SerializeField] private Canvas[] mainMenuPages;

    private void Start()
    {
        AudioManager.instance.PlayClip(AudioManager.instance.BGMSource, AudioManager.instance.menuBGM);
    }

    public void OnPlayButtonClick()
    {
        GameManager.instance.transitionAnimator.gameObject.SetActive(true);
        AudioManager.instance.PlayClip(AudioManager.instance.SFXSource, AudioManager.instance.buttonSFX);
        GameManager.instance.ChangeState(GameState.GAME_INTRO);
    }

    public void ToggleMenuPages(int selectedIndex)
    {
        // toggle between pages on the main menu
        AudioManager.instance.PlayClip(AudioManager.instance.SFXSource, AudioManager.instance.buttonSFX);

        if (selectedIndex == 0) // if the destination is the main menu, hide the back button
            mainMenuBackButton.enabled = false;
        else
            mainMenuBackButton.enabled = true;

        for (int i = 0; i < mainMenuPages.Length; i++)
        {
            if (i == selectedIndex)
                mainMenuPages[i].enabled = true;
            else
                mainMenuPages[i].enabled = false;
        }
    }
}
