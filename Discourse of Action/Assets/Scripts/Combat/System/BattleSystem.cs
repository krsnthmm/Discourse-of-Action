using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
    [Header("[UI ELEMENTS]")]
    [SerializeField] private BattleUnit _playerUnit;
    [SerializeField] private BattleUnit _enemyUnit;

    [SerializeField] private BattleHUD _playerHUD;
    [SerializeField] private BattleHUD _enemyHUD;

    [SerializeField] private TMP_Text _turnText;

    [Header("[BATTLE SYSTEM]")]
    [SerializeField] private CardManager _cardManager;
    [SerializeField] private KeyPointManager _keyPointManager;

    private Card _selectedCard;
    public KeyPoint _targetKeyPoint;
    private BattleState _state;

    void Start()
    {
        // set up the battle scene
        _state = BattleState.START;

        _playerUnit.characterData = GameManager.instance.currentPlayerData;
        _enemyUnit.characterData = GameManager.instance.enemyToBattle;

        _keyPointManager.deck = GameManager.instance.enemyToBattle.keyPointDeck;

        SetupBattle();
    }

    void SetupBattle()
    {
        // to make sure values from the previous battle don't carry over
        _playerUnit.characterData.currHealth = _playerUnit.characterData.maxHealth;
        _enemyUnit.characterData.currHealth = _enemyUnit.characterData.maxHealth;

        // render character sprites
        _playerUnit.characterRenderer.RenderCharacter(_playerUnit.characterData);
        _enemyUnit.characterRenderer.RenderCharacter(_enemyUnit.characterData);

        // display name and health
        _playerHUD.SetHUD();
        _enemyHUD.SetHUD();

        _keyPointManager.SetTarget();

        for (int i = 0; i < _cardManager.availableHandSlots.Length; i++)
            _cardManager.DrawCard();

        for (int i = 0; i < _keyPointManager.availableDisplaySlots.Length; i++)
            _keyPointManager.PutOnDisplay();

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

            for (int i = 0; i < _cardManager.availableHandSlots.Length; i++)
                _cardManager.DrawCard();

            for (int i = 0; i < _cardManager.hand.Count; i++)
                _cardManager.hand[i].GetComponent<Button>().interactable = true;
            
            _turnText.text = "YOUR TURN";
        }    
    }

    IEnumerator PlayerAttack(int damage)
    {
        _targetKeyPoint = _keyPointManager.selectedKeyPoint;
        _keyPointManager.RemoveFromDisplay(_targetKeyPoint);

        _enemyUnit.characterData.TakeDamage((int)(damage * CheckTypeEffectiveness(_selectedCard.cardData.cardType, _targetKeyPoint.keyPointData.keyPointType)));
        _enemyHUD.UpdateHealthValue();

        _state = BattleState.ENEMY_TURN;

        for (int i = 0; i < _cardManager.hand.Count; i++)
            _cardManager.hand[i].GetComponent<Button>().interactable = false;

        yield return new WaitForSeconds(1);

        if (_enemyUnit.characterData.currHealth <= 0)
        {
            _state = BattleState.WON;
            StartCoroutine(EndBattle());
        }
        else
            StartCoroutine(EnemyTurn());
    }

    IEnumerator EnemyTurn()
    {
        _turnText.text = "ENEMY TURN";
        _keyPointManager.PutOnDisplay();

        yield return new WaitForSeconds(1);

        _playerUnit.characterData.TakeDamage(5);
        _playerHUD.UpdateHealthValue();

        yield return new WaitForSeconds(1);

        _state = BattleState.PLAYER_TURN;
        StartCoroutine(PlayerTurn());
    }

    IEnumerator EndBattle()
    {
        _keyPointManager.ClearDisplay();

        if (_state == BattleState.WON)
        {
            _turnText.text = "VICTORY!";

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

    public void OnCardSelect()
    {
        if (_state == BattleState.PLAYER_TURN)
        {
            _selectedCard = _cardManager.selectedCard;
            StartCoroutine(PlayerAttack(_selectedCard.damage));
        }
        else
            return;
    }

    float CheckTypeEffectiveness(ElementTypes card, ElementTypes keyPoint)
    {
        // define the types of "statements" present in combat
        Dictionary<ElementTypes, Dictionary<ElementTypes, float>> multiplier = new()
        {
            { ElementTypes.TYPE_NEUTRAL, new Dictionary<ElementTypes, float>() },
            { ElementTypes.TYPE_REASONING, new Dictionary<ElementTypes, float>() },
            { ElementTypes.TYPE_EMOTION, new Dictionary<ElementTypes, float>() },
            { ElementTypes.TYPE_INSTINCT, new Dictionary<ElementTypes, float>() },
        };

        // if card trumps key point, damage dealt is 1.5x normal damage
        multiplier[ElementTypes.TYPE_REASONING][ElementTypes.TYPE_EMOTION] = 1.5f;
        multiplier[ElementTypes.TYPE_EMOTION][ElementTypes.TYPE_INSTINCT] = 1.5f;
        multiplier[ElementTypes.TYPE_INSTINCT][ElementTypes.TYPE_REASONING] = 1.5f;

        // if card isn't very effective against key point, damage dealt is 0.5x normal damage
        multiplier[ElementTypes.TYPE_REASONING][ElementTypes.TYPE_INSTINCT] = 0.5f;
        multiplier[ElementTypes.TYPE_EMOTION][ElementTypes.TYPE_REASONING] = 0.5f;
        multiplier[ElementTypes.TYPE_INSTINCT][ElementTypes.TYPE_EMOTION] = 0.5f;

        foreach (ElementTypes offensive in multiplier.Keys)
        {
            foreach (ElementTypes defensive in multiplier.Keys)
            {
                // normal effectiveness
                if (!multiplier[offensive].ContainsKey(defensive))
                    multiplier[offensive][defensive] = 1;
            }
        }

        return multiplier[card][keyPoint];
    }
}
