using System.Collections;
using UnityEngine;
using TMPro;

public class EndingManager : MonoBehaviour
{
    [SerializeField] private TMP_Text _endingHeader;
    [SerializeField] private TMP_Text _endingSubtitle;

    // Start is called before the first frame update
    void Start()
    {
        SetText();
        StartCoroutine(RedirectToMain());
    }

    void SetText()
    {
        _endingHeader.text = GameManager.instance.hasWon ? "FIN" : "THE END";
        _endingSubtitle.text = GameManager.instance.hasWon ? "Here's to new memories and a better you." : "Try to do your best to remember.";
    }

    IEnumerator RedirectToMain()
    {
        yield return new WaitForSeconds(7f);

        GameManager.instance.ChangeState(GameState.GAME_MENU);
    }
}
