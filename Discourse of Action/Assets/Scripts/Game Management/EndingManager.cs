using System.Collections;
using UnityEngine;

public class EndingManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(RedirectToMain());
    }

    public IEnumerator RedirectToMain()
    {
        yield return new WaitForSeconds(7f);

        GameManager.instance.ChangeState(GameState.GAME_MENU);
    }
}
