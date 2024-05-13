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
        if (GameManager.instance.gameState == GameManager.GameState.GAME_OVERWORLD)
            characterAnimController = overworldAnimControllers[(int)selectedCharacter];
        else
            characterAnimController = dialogueAnimControllers[(int)selectedCharacter];
    }
}
