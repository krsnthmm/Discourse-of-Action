using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PlayerData : CharacterData
{
    public enum Character
    {
        CHARACTER_MASC = 0,
        CHARACTER_FEM = 1
    }

    public Character selectedCharacter;
    public RuntimeAnimatorController[] overworldAnimControllers;
    public RuntimeAnimatorController[] dialogueAnimControllers;

    public void SetAnimatorController()
    {
        if (GameManager.instance.gameState == GameState.GAME_CUTSCENE || GameManager.instance.gameState == GameState.GAME_BATTLE)
            characterAnimController = dialogueAnimControllers[(int)selectedCharacter];
        else
            characterAnimController = overworldAnimControllers[(int)selectedCharacter];
    }
}
