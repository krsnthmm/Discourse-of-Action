using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    GAME_MENU,
    GAME_INTRO,
    GAME_OVERWORLD,
    GAME_BATTLE,
    GAME_RECALL,
    GAME_CUTSCENE,
    GAME_STORY,
    GAME_ENDING,
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("[GAME COMPONENTS]")]
    public bool isPaused;
    public Transform playerSpawn;
    public PlayerController playerController;
    public InputController inputController;

    [Header("[MENU MANAGEMENT]")]
    [SerializeField] private GameObject mainMenuBackground;
    [SerializeField] private GameObject mainMenuGroup;
    [SerializeField] private GameObject mainMenuBackButton;
    [SerializeField] private GameObject[] mainMenuPages;
    [SerializeField] private GameObject pauseMenuBackground;
    [SerializeField] private GameObject pauseMenuGroup;
    [SerializeField] private GameObject pauseMenuBackButton;
    [SerializeField] private GameObject[] pauseMenuPages;

    [Header("[CHARACTER DATA]")]
    public PlayerData currentPlayerData;
    public FinalBossData finalBossData;

    [Header("[MEMORY RECALL DATA]")]
    public MemoryRecallData act1RecallData;
    public MemoryRecallData act2RecallData;
    public MemoryRecallData act3RecallData;
    public RecallRevelationData currentRevelationData;
    public RecallConclusionData currentConclusionData;

    [Header("[GAME DATABASE]")]
    public GameState gameState;
    public GameFlags activatedFlags;

    [Header("[SCENES]")]
    public Animator transitionAnimator;
    public string menuSceneName;
    public string introSceneName;
    public string overworldSceneName;
    public string battleSceneName;
    public string memoryRecallSceneName;
    public string endSceneName;

    [Header("[ENEMY DATABASE]")]
    public EnemyData[] enemies;
    public EnemyData enemyToBattle;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    public IEnumerator LoadScene(string sceneName, bool isMenuActive)
    {
        transitionAnimator.SetTrigger("Start");

        yield return new WaitForSeconds(1);

        SceneManager.LoadScene(sceneName);
        mainMenuGroup.SetActive(isMenuActive);
        mainMenuBackground.SetActive(isMenuActive);

        transitionAnimator.SetTrigger("End");
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPaused)
        {
            if (gameState == GameState.GAME_OVERWORLD)
            {
                if (inputController.TryGetMovementAxisInput(out MovementAxisCommand movementAxisCommand))
                    playerController.ReadMovementAxisCommand(movementAxisCommand);

                if (inputController.TryGetKeyboardInput(out KeyboardInputCommand keyboardInputCommand))
                    playerController.ReadKeyboardInputCommand(keyboardInputCommand);

                playerController.UpdateTransform();
            }

            if (DialogueManager.instance != null && DialogueManager.instance.isInDialogue && Input.GetKeyDown(KeyCode.Space))
                DialogueManager.instance.OnContinueButtonClick();
        }

        if (Input.GetKeyDown(KeyCode.Escape) && gameState > GameState.GAME_MENU && gameState < GameState.GAME_ENDING)
        {
            isPaused = !isPaused;
            OnTogglePause(isPaused);
        }
    }

    #region GENERAL

    void ResetStates()
    {
        foreach (EnemyData enemy in enemies)
            enemy.hasWonAgainst = false;

        SetFlag(GameFlags.Undefined);
    }

    void OnTogglePause(bool isPaused)
    {
        AudioManager.instance.PlayClip(AudioManager.instance.SFXSource, AudioManager.instance.buttonSFX);

        if (isPaused) // if true
            pauseMenuGroup.SetActive(isPaused);
        else // if false
        {
            foreach (var page in pauseMenuPages) // coming back to menu from pausing the game
                page.SetActive(isPaused);

            pauseMenuBackButton.SetActive(isPaused);
        }

        pauseMenuBackground.SetActive(isPaused);
    }

    public void ChangeState(GameState state)
    {
        gameState = state;
        OnStateChange();
    }

    void OnStateChange()
    {
        switch (gameState)
        {
            case GameState.GAME_MENU:
                StartCoroutine(LoadScene(menuSceneName, true));
                AudioManager.instance.PlayClip(AudioManager.instance.BGMSource, AudioManager.instance.menuBGM);
                
                playerController.gameObject.SetActive(false);

                DialogueManager.instance.animator.SetBool("IsOpen", false);
                DialogueManager.instance.isInDialogue = false;

                ResetFlags();
                break;
            case GameState.GAME_INTRO:
                StartCoroutine(LoadScene(introSceneName, false));
                AudioManager.instance.PlayClip(AudioManager.instance.BGMSource, AudioManager.instance.introBGM);

                ResetStates();
                break;
            case GameState.GAME_OVERWORLD:
                StartCoroutine(LoadScene(overworldSceneName, false));
                AudioManager.instance.PlayClip(AudioManager.instance.BGMSource, AudioManager.instance.gameBGM);

                playerController.gameObject.SetActive(true);
                break;
            case GameState.GAME_BATTLE:
                StartCoroutine(LoadScene(battleSceneName, false));
                AudioManager.instance.PlayClip(AudioManager.instance.BGMSource, AudioManager.instance.combatBGM);
                currentPlayerData.SetAnimatorController();
                break;
            case GameState.GAME_RECALL:
                StartCoroutine(LoadScene(memoryRecallSceneName, false));
                AudioManager.instance.PlayClip(AudioManager.instance.BGMSource, AudioManager.instance.memoryRecallBGM);
                break;
            case GameState.GAME_STORY:
                // TODO: dialogue stuff i guess
                break;
            case GameState.GAME_ENDING:
                AudioManager.instance.PlayClip(AudioManager.instance.BGMSource, AudioManager.instance.menuBGM);
                StartCoroutine(LoadScene(endSceneName, false));
                break;
        }
    }

    public void OnQuitButtonClick()
    {
        Application.Quit();
    }

    public void DestroyInstance()
    {
        if (instance != null)
            Destroy(this);
    }

    #endregion

    #region MAIN MENU

    public void OnPlayButtonClick()
    {
        transitionAnimator.gameObject.SetActive(true);
        AudioManager.instance.PlayClip(AudioManager.instance.SFXSource, AudioManager.instance.buttonSFX);
        ChangeState(GameState.GAME_INTRO);
    }

    public void ToggleMenuPages(int selectedIndex)
    {
        // toggle between pages on the main menu
        AudioManager.instance.PlayClip(AudioManager.instance.SFXSource, AudioManager.instance.buttonSFX);

        if (selectedIndex == 0) // if the destination is the main menu, hide the back button
            mainMenuBackButton.SetActive(false);
        else
            mainMenuBackButton.SetActive(true);

        for (int i = 0; i < mainMenuPages.Length; i++)
        {
            if (i == selectedIndex)
                mainMenuPages[i].SetActive(true);
            else
                mainMenuPages[i].SetActive(false);
        }
    }

    #endregion

    #region PAUSE MENU

    public void OnUnpauseClick()
    {
        AudioManager.instance.PlayClip(AudioManager.instance.SFXSource, AudioManager.instance.buttonSFX);

        isPaused = false;
        OnTogglePause(isPaused);
    }

    public void OnMenuButtonClick()
    {
        ChangeState(GameState.GAME_MENU);

        AudioManager.instance.PlayClip(AudioManager.instance.SFXSource, AudioManager.instance.buttonSFX);

        isPaused = false;
        OnTogglePause(isPaused);
    }

    public void TogglePausePages(int selectedIndex)
    {
        // toggle between pages on the pause menu
        AudioManager.instance.PlayClip(AudioManager.instance.SFXSource, AudioManager.instance.buttonSFX);

        if (selectedIndex == 0 || selectedIndex == pauseMenuPages.Length - 1) // if the destination is the pause menu or the confirm page, hide the back button
            pauseMenuBackButton.SetActive(false);
        else
            pauseMenuBackButton.SetActive(true);

        for (int i = 0; i < pauseMenuPages.Length; i++)
        {
            if (i == selectedIndex)
                pauseMenuPages[i].SetActive(true);
            else
                pauseMenuPages[i].SetActive(false);
        }
    }

    #endregion

    #region INTRO

    public void SelectCharacter(int index) // 0 = masc, 1 = fem
    {
        AudioManager.instance.PlayClip(AudioManager.instance.SFXSource, AudioManager.instance.buttonSFX);

        currentPlayerData.selectedCharacter = (PlayerData.Character)index;

        currentPlayerData.SetAnimatorController();
        finalBossData.SetAnimatorController((int)currentPlayerData.selectedCharacter);
    }

    public void SetName(string name)
    {
        AudioManager.instance.PlayClip(AudioManager.instance.SFXSource, AudioManager.instance.buttonSFX);

        currentPlayerData.characterName = name;

        playerController.transform.position = playerSpawn.position;
        ChangeState(GameState.GAME_OVERWORLD);
    }

    #endregion

    #region FLAGS

    public bool IsFlagActivated(GameFlags flag)
    {
        return activatedFlags != GameFlags.Undefined && GameManager.instance.activatedFlags.HasFlag(flag);
    }

    public void SetFlag(GameFlags flag)
    {
        activatedFlags |= flag;
    }

    public void ResetFlags()
    {
        activatedFlags = GameFlags.Undefined;
    }

    #endregion

    #region RECALL

    public void SetRecallData(MemoryRecallData data)
    {
        currentRevelationData = data.revelationDatas[0];
        currentConclusionData = data.conclusionData;
    }

    #endregion
}
