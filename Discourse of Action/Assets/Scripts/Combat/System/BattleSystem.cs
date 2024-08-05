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
    [SerializeField] private AudioSource _combatSFXSource;

    private Card _selectedCard;
    public KeyPoint _targetKeyPoint;
    private BattleState _state;

    private int _multiplier = 1;

    void Start()
    {
        // set up the battle scene
        _state = BattleState.START;

        _playerUnit.characterData = GameManager.instance.currentPlayerData;
        _enemyUnit.characterData = GameManager.instance.enemyToBattle;

        _keyPointManager.deck = GameManager.instance.enemyToBattle.keyPointDeck;

        SetupBattle();

        if (GameManager.instance.enemyToBattle.enemyType == EnemyData.EnemyType.ENEMY_BOSS_IRIS_PHASE2)
            _multiplier = 2;
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
            yield return new WaitForSeconds(0.5f);

            _state = BattleState.LOST;
            StartCoroutine(EndBattle());
        }
        else
        {
            yield return new WaitForSeconds(0.5f);

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

        switch(CheckTypeEffectiveness(_selectedCard.cardData.cardType, _targetKeyPoint.keyPointData.keyPointType))
        {
            case 1.5f:
                AudioManager.instance.PlayClip(_combatSFXSource, AudioManager.instance.superEffectiveSFX);
                break;
            case 1f:
                AudioManager.instance.PlayClip(_combatSFXSource, AudioManager.instance.normalEffectiveSFX);
                break;
            case 0.5f:
                AudioManager.instance.PlayClip(_combatSFXSource, AudioManager.instance.notEffectiveSFX);
                break;
        }

        _enemyUnit.characterData.TakeDamage((int)(damage * CheckTypeEffectiveness(_selectedCard.cardData.cardType, _targetKeyPoint.keyPointData.keyPointType) * _multiplier));
        _enemyHUD.UpdateHealthValue();

        _enemyUnit.characterRenderer.animator.SetInteger("Hurt", 1);
        _enemyHUD.UpdateDamageText("" + (int)(damage * CheckTypeEffectiveness(_selectedCard.cardData.cardType, _targetKeyPoint.keyPointData.keyPointType) * _multiplier));

        yield return new WaitForSeconds(0.1f);

        _enemyUnit.characterRenderer.animator.SetInteger("Hurt", 0);

        _state = BattleState.ENEMY_TURN;

        for (int i = 0; i < _cardManager.hand.Count; i++)
            _cardManager.hand[i].GetComponent<Button>().interactable = false;

        yield return new WaitForSeconds(0.4f);

        _enemyHUD.UpdateDamageText("");

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

        yield return new WaitForSeconds(0.5f);

        if (GameManager.instance.enemyToBattle.enemyType == EnemyData.EnemyType.ENEMY_BOSS_IRIS_PHASE1)
        {
            AudioManager.instance.PlayClip(_combatSFXSource, AudioManager.instance.superEffectiveSFX);

            _playerUnit.characterData.TakeDamage(999);

            _playerHUD.UpdateHealthValue();

            _playerUnit.characterRenderer.animator.SetInteger("Hurt", 1);
            _playerHUD.UpdateDamageText("!!!");
        }
        else
        {
            AudioManager.instance.PlayClip(_combatSFXSource, AudioManager.instance.normalEffectiveSFX);

            _playerUnit.characterData.TakeDamage(5);
            _playerHUD.UpdateHealthValue();

            _playerUnit.characterRenderer.animator.SetInteger("Hurt", 1);
            _playerHUD.UpdateDamageText("" + 5);
        }

        yield return new WaitForSeconds(0.1f);

        _playerUnit.characterRenderer.animator.SetInteger("Hurt", 0);

        yield return new WaitForSeconds(0.4f);

        _playerHUD.UpdateDamageText("");

        _state = BattleState.PLAYER_TURN;
        StartCoroutine(PlayerTurn());
    }

    IEnumerator EndBattle()
    {
        _keyPointManager.ClearDisplay();

        if (_state == BattleState.WON)
        {
            _turnText.text = "VICTORY!";
            AudioManager.instance.PlayClip(AudioManager.instance.BGMSource, AudioManager.instance.combatWonJingle);

            GameManager.instance.enemyToBattle.hasWonAgainst = true;

            yield return new WaitForSeconds(AudioManager.instance.combatWonJingle.length);

            AudioManager.instance.StopClip(AudioManager.instance.BGMSource);

            yield return new WaitForSeconds(1);

            switch (GameManager.instance.enemyToBattle.enemyType)
            {
                case EnemyData.EnemyType.ENEMY_BOSS_ASTER:
                    GameManager.instance.SetRecallData(GameManager.instance.act1RecallData);
                    GameManager.instance.ChangeState(GameState.GAME_RECALL);
                    break;
                case EnemyData.EnemyType.ENEMY_BOSS_PRIMROSE:
                    GameManager.instance.SetRecallData(GameManager.instance.act2RecallData);
                    GameManager.instance.ChangeState(GameState.GAME_RECALL);
                    break;
                case EnemyData.EnemyType.ENEMY_BOSS_IRIS_PHASE2:
                    GameManager.instance.ChangeState(GameState.GAME_ENDING);
                    break;
                default:
                    GameManager.instance.ChangeState(GameState.GAME_OVERWORLD);
                    break;
            }
        }
        else if (_state == BattleState.LOST)
        {
            _turnText.text = "DEFEAT!";
            AudioManager.instance.PlayClip(AudioManager.instance.BGMSource, AudioManager.instance.combatLostJingle);

            yield return new WaitForSeconds(AudioManager.instance.combatLostJingle.length);

            AudioManager.instance.StopClip(AudioManager.instance.BGMSource);

            yield return new WaitForSeconds(1);

            if (GameManager.instance.enemyToBattle.enemyType == EnemyData.EnemyType.ENEMY_BOSS_IRIS_PHASE1)
            {
                _turnText.text = "";

                yield return new WaitForSeconds(1);

                _turnText.text = "...Not yet!";

                yield return new WaitForSeconds(1);

                GameManager.instance.SetRecallData(GameManager.instance.act3RecallData);
                GameManager.instance.ChangeState(GameState.GAME_RECALL);
            }
            else
                GameManager.instance.ChangeState(GameState.GAME_ENDING);
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

        // if card is not very effective against key point, damage dealt is 0.5x normal damage
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
