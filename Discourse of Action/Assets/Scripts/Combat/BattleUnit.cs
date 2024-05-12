using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleUnit : MonoBehaviour
{
    public CharacterData characterData;
    public CharacterRenderer characterRenderer;

    void Start()
    {
        if (characterData == null)
            characterData = ScriptableObject.CreateInstance<CharacterData>();

        characterRenderer.RenderCharacter(characterData);
    }
}
