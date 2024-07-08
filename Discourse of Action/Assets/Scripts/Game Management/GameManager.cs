using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    GAME_MENU,
    GAME_INTRO,
    GAME_OVERWORLD,
    GAME_DIALOGUE,
    GAME_BATTLE,
    GAME_RECALL,
    GAME_CUTSCENE,
    GAME_ENDING,
    GAME_WIN,
    GAME_LOSE
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("[GAME COMPONENTS]")]
    public GameObject menuCanvas;
    public PlayerController playerController;
    public InputController inputController;

    [Header("[CHARACTER DATA]")]
    public PlayerData currentPlayerData;
    public FinalBossData finalBossData;

    [Header("[GAME STATES]")]
    public GameState gameState;

    [Header("[SCENES]")]
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

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    // Update is called once per frame
    void Update()
    {
        if (gameState == GameState.GAME_OVERWORLD)
        {
            if (inputController.TryGetMovementAxisInput(out MovementAxisCommand movementAxisCommand))
                playerController.ReadMovementAxisCommand(movementAxisCommand);

            if (inputController.TryGetKeyboardInput(out KeyboardInputCommand keyboardInputCommand))
                playerController.ReadKeyboardInputCommand(keyboardInputCommand);

            playerController.UpdateTransform();

            if (Input.GetKeyDown(KeyCode.Tab))
            {
                gameState = GameState.GAME_RECALL;
                ChangeState(gameState);
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gameState = gameState == GameState.GAME_MENU ? GameState.GAME_OVERWORLD : GameState.GAME_MENU;
            ChangeState(gameState);
        }

        if (DialogueManager.instance != null && DialogueManager.instance.isInDialogue && Input.GetKeyDown(KeyCode.Space))
            DialogueManager.instance.OnContinueButtonClick();
    }

    #region GENERAL

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
                LoadScene(menuSceneName);
                menuCanvas.SetActive(true);
                AudioManager.instance.PlayClip(AudioManager.instance.BGMSource, AudioManager.instance.menuBGM);

                playerController.gameObject.SetActive(false);
                break;
            case GameState.GAME_INTRO:
                LoadScene(introSceneName);
                menuCanvas.SetActive(false);
                AudioManager.instance.PlayClip(AudioManager.instance.BGMSource, AudioManager.instance.introBGM);
                break;
            case GameState.GAME_OVERWORLD:
                LoadScene(overworldSceneName);
                menuCanvas.SetActive(false);
                AudioManager.instance.PlayClip(AudioManager.instance.BGMSource, AudioManager.instance.gameBGM);

                playerController.gameObject.SetActive(true);
                break;
            case GameState.GAME_BATTLE:
                LoadScene(battleSceneName);
                AudioManager.instance.PlayClip(AudioManager.instance.BGMSource, AudioManager.instance.combatBGM);
                currentPlayerData.SetAnimatorController();
                break;
            case GameState.GAME_RECALL:
                LoadScene(memoryRecallSceneName);
                AudioManager.instance.PlayClip(AudioManager.instance.BGMSource, AudioManager.instance.memoryRecallBGM);
                break;
            case GameState.GAME_DIALOGUE:
                // TODO: dialogue system implementation
                break;
            case GameState.GAME_CUTSCENE:
                // TODO: dialogue system implementation
                break;
        }
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
        ChangeState(GameState.GAME_INTRO);
    }

    public void OnControlsButtonClick()
    {
        Debug.Log("Controls");
    }

    public void OnSettingsButtonClick()
    {
        Debug.Log("Settings");
    }

    public void OnBackButtonClick()
    {
        Debug.Log("Back to Main");
    }

    public void OnQuitButtonClick()
    {
        Application.Quit();
    }

    #endregion

    #region INTRO

    public void SelectCharacter(int index) // 0 = masc, 1 = fem
    {
        currentPlayerData.selectedCharacter = (PlayerData.Character)index;

        currentPlayerData.SetAnimatorController();
        finalBossData.SetAnimatorController((int)currentPlayerData.selectedCharacter);
    }

    public void SetName(string name)
    {
        currentPlayerData.characterName = name;
        ChangeState(GameState.GAME_OVERWORLD);
    }

    #endregion
}
