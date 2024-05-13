using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum BattleState 
{ 
    START,
    PLAYER_TURN,
    ENEMY_TURN,
    WON,
    LOST,
    TOTAL
}

public class BattleSystem : MonoBehaviour
{
    [SerializeField] private BattleUnit _playerUnit;
    [SerializeField] private BattleUnit _enemyUnit;

    [SerializeField] private BattleHUD _playerHUD;
    [SerializeField] private BattleHUD _enemyHUD;

    [SerializeField] private TMP_Text _turnText;

    private BattleState _state;

    void Start()
    {
        _state = BattleState.START;
        _playerUnit.characterData = GameManager.instance.currentPlayerData;
        _enemyUnit.characterData = GameManager.instance.enemyToBattle;
        SetupBattle();
    }

    void SetupBattle()
    {
        // to make sure values from the previous battle don't carry over
        _playerUnit.characterData.currHealth = _playerUnit.characterData.maxHealth;
        _enemyUnit.characterData.currHealth = _enemyUnit.characterData.maxHealth;

        _playerUnit.characterRenderer.RenderCharacter(_playerUnit.characterData);
        _enemyUnit.characterRenderer.RenderCharacter(_enemyUnit.characterData);

        _playerHUD.SetHUD();
        _enemyHUD.SetHUD();

        _state = BattleState.PLAYER_TURN;
        StartCoroutine(PlayerTurn());
    }

    IEnumerator PlayerTurn()
    {
        if (_playerUnit.characterData.currHealth <= 0)
        {
            yield return new WaitForSeconds(2);

            _state = BattleState.LOST;
            StartCoroutine(EndBattle());
        }
        else
        {
            yield return new WaitForSeconds(2);

            _turnText.text = "YOUR TURN";
        }    
    }

    IEnumerator PlayerAttack(int damage)
    {
        _enemyUnit.characterData.TakeDamage(damage);
        _enemyHUD.UpdateHealthValue();
        _state = BattleState.ENEMY_TURN;

        yield return new WaitForSeconds(1);

        if (_enemyUnit.characterData.currHealth <= 0)
        {
            yield return new WaitForSeconds(1);

            _state = BattleState.WON;
            StartCoroutine(EndBattle());
        }
        else
        {
            yield return new WaitForSeconds(1);

            StartCoroutine(EnemyTurn());
        }
    }

    IEnumerator EnemyTurn()
    {
        _turnText.text = "ENEMY TURN";

        yield return new WaitForSeconds(1);

        _playerUnit.characterData.TakeDamage(5);
        _playerHUD.UpdateHealthValue();

        yield return new WaitForSeconds(1);

        _state = BattleState.PLAYER_TURN;
        StartCoroutine(PlayerTurn());
    }

    IEnumerator EndBattle()
    {
        if (_state == BattleState.WON)
        {
            _turnText.text = "THAT IS ALL!";

            yield return new WaitForSeconds(2);

            GameManager.instance.ChangeState(GameManager.GameState.GAME_OVERWORLD);
        }
        else if (_state == BattleState.LOST)
        {
            _turnText.text = "DEFEAT!";

            yield return new WaitForSeconds(2);

            GameManager.instance.ChangeState(GameManager.GameState.GAME_OVERWORLD);
        }
    }

    void OnKeyPointSelect()
    {
        if (_state == BattleState.PLAYER_TURN)
        {
            // TODO: key point selection logic
        }
        else
            return;
    }

    public void OnCardSelect()
    {
        if (_state == BattleState.PLAYER_TURN)
            StartCoroutine(PlayerAttack(10)); //placeholder value, change once card logic is done
        else
            return;
    }
}
