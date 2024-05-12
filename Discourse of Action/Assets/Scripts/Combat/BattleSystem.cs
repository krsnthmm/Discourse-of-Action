using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public BattleUnit playerUnit;
    public BattleUnit enemyUnit;

    public BattleHUD playerHUD;
    public BattleHUD enemyHUD;

    public BattleState state;

    // Start is called before the first frame update
    void Start()
    {
        state = BattleState.START;
        SetupBattle();
    }

    void SetupBattle()
    {
        playerHUD.SetHUD();

        state = BattleState.PLAYER_TURN;
        PlayerTurn();
    }

    void PlayerTurn()
    {

    }

    public void OnCardSelected()
    {

    }
}
