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

    public Character _selectedCharacter;

    public Character GetCharacter()
    {
        return _selectedCharacter;
    }

    public void SetCharacter(Character character)
    {
        _selectedCharacter = character;
    }
}
