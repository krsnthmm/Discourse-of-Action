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
    public int currentLevel;
    public bool isPaused;
    public Transform playerSpawn;
    public PlayerController playerController;
    public InputController inputController;

    [Header("[PAUSE MENU]")]
    [SerializeField] private Canvas pauseMenuBackground;
    [SerializeField] private Canvas pauseMenuGroup;
    [SerializeField] private Canvas pauseMenuBackButton;
    [SerializeField] private Canvas[] pauseMenuPages;

    [Header("[CHARACTER DATA]")]
    public PlayerData currentPlayerData;
    public FinalBossData finalBossData;

    [Header("[MEMORY RECALL DATA]")]
    public MemoryRecallData act1RecallData;
    public MemoryRecallData act2RecallData;
    public MemoryRecallData act3RecallData;
    public MemoryRecallData currentRecallData;

    [Header("[GAME DATABASE]")]
    public GameState gameState;
    public GameFlags activatedFlags;

    [Header("[SCENES]")]
    public Animator transitionAnimator;
    public string menuSceneName;
    public string introSceneName;
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

    public IEnumerator LoadScene(string sceneName)
    {
        transitionAnimator.SetTrigger("Start");

        yield return new WaitForSeconds(1);

        SceneManager.LoadScene(sceneName);

        transitionAnimator.SetTrigger("End");
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPaused)
        {
            if (gameState == GameState.GAME_OVERWORLD && !DialogueManager.instance.isInDialogue)
            {
                if (inputController.TryGetMovementAxisInput(out MovementAxisCommand movementAxisCommand))
                    playerController.ReadMovementAxisCommand(movementAxisCommand);

                playerController.UpdateTransform();
            }

            if (DialogueManager.instance != null && DialogueManager.instance.isInDialogue && Input.GetKeyDown(KeyCode.Space))
                DialogueManager.instance.OnContinueButtonClick();
        }

        if (inputController.TryGetKeyboardInput(out KeyboardInputCommand keyboardInputCommand))
            playerController.ReadKeyboardInputCommand(keyboardInputCommand);
    }

    #region GENERAL

    void ResetStates()
    {
        foreach (EnemyData enemy in enemies)
            enemy.hasWonAgainst = false;

        SetFlag(GameFlags.Undefined);
    }

    public void HandlePauseKeyPress()
    {
        if (gameState > GameState.GAME_INTRO && gameState < GameState.GAME_ENDING)
        {
            isPaused = !isPaused;
            OnTogglePause(isPaused);
        }
    }

    void OnTogglePause(bool isPaused)
    {
        AudioManager.instance.PlayClip(AudioManager.instance.SFXSource, AudioManager.instance.buttonSFX);

        if (isPaused)
            pauseMenuGroup.enabled = isPaused;
        else
        {
            foreach (var page in pauseMenuPages) // coming back to menu from pausing the game
                page.enabled = isPaused;

            pauseMenuBackButton.enabled = isPaused;
        }

        pauseMenuBackground.enabled = isPaused;
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
                StartCoroutine(LoadScene(menuSceneName));
                
                playerController.gameObject.SetActive(false);

                DialogueManager.instance.animator.SetBool("IsOpen", false);
                DialogueManager.instance.isInDialogue = false;

                ResetFlags();
                break;
            case GameState.GAME_INTRO:
                currentLevel = 1;
                playerController.transform.position = playerSpawn.position;

                finalBossData.enemyType = EnemyData.EnemyType.ENEMY_BOSS_IRIS_PHASE1;

                StartCoroutine(LoadScene(introSceneName));
                AudioManager.instance.PlayClip(AudioManager.instance.BGMSource, AudioManager.instance.introBGM);

                ResetStates();
                break;
            case GameState.GAME_OVERWORLD:
                StartCoroutine(LoadScene("Level" + currentLevel + "Scene"));
                AudioManager.instance.PlayClip(AudioManager.instance.BGMSource, AudioManager.instance.gameBGM);
                currentPlayerData.SetAnimatorController();

                playerController.gameObject.SetActive(true);
                break;
            case GameState.GAME_BATTLE:
                StartCoroutine(LoadScene(battleSceneName));
                AudioManager.instance.PlayClip(AudioManager.instance.BGMSource, AudioManager.instance.combatBGM);
                currentPlayerData.SetAnimatorController();

                playerController.gameObject.SetActive(false);
                break;
            case GameState.GAME_RECALL:
                StartCoroutine(LoadScene(memoryRecallSceneName));
                AudioManager.instance.PlayClip(AudioManager.instance.BGMSource, AudioManager.instance.memoryRecallBGM);
                break;
            case GameState.GAME_STORY:
                // TODO: dialogue stuff i guess
                break;
            case GameState.GAME_ENDING:
                AudioManager.instance.PlayClip(AudioManager.instance.BGMSource, AudioManager.instance.menuBGM);
                StartCoroutine(LoadScene(endSceneName));
                break;
        }
    }

    public void OnQuitButtonClick()
    {
        Application.Quit();
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
            pauseMenuBackButton.enabled = false;
        else
            pauseMenuBackButton.enabled = true;

        for (int i = 0; i < pauseMenuPages.Length; i++)
        {
            if (i == selectedIndex)
                pauseMenuPages[i].enabled = true;
            else
                pauseMenuPages[i].enabled = false;
        }
    }

    #endregion

    #region FLAGS

    public bool IsFlagActivated(GameFlags flag)
    {
        return activatedFlags != GameFlags.Undefined && instance.activatedFlags.HasFlag(flag);
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
        currentRecallData = data;
    }

    #endregion
}
