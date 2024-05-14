using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("[GAME COMPONENTS]")]
    public PlayerController playerController;
    public InputController inputController;

    [Header("[CHARACTER DATA]")]
    public PlayerData currentPlayerData;
    public FinalBossData finalBossData;

    [Header("[GAME STATES]")]
    public GameState gameState;
    public enum GameState
    {
        GAME_OVERWORLD,
        GAME_DIALOGUE,
        GAME_BATTLE,
        GAME_CUTSCENE
    }

    [Header("[SCENES]")]
    public string menuSceneName;
    public string introSceneName;
    public string overworldSceneName;
    public string battleSceneName;
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

        currentPlayerData.SetAnimatorController();
        finalBossData.SetAnimatorController((int)currentPlayerData.selectedCharacter);
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

            playerController.UpdateTransform();
        }

        // testing purposes
        if (Input.GetKeyDown(KeyCode.Space))
        {
            gameState = gameState == GameState.GAME_OVERWORLD ? GameState.GAME_BATTLE : GameState.GAME_OVERWORLD;
            ChangeState(gameState);
        }
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
            case GameState.GAME_OVERWORLD:
                LoadScene(overworldSceneName);
                currentPlayerData.SetAnimatorController();
                break;
            case GameState.GAME_BATTLE:
                LoadScene(battleSceneName);
                currentPlayerData.SetAnimatorController();
                break;
            case GameState.GAME_DIALOGUE:
                break;
        }
    }

    public void DestroyInstance()
    {
        if (instance != null)
            Destroy(this);
    }
}
