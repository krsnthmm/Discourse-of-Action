using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterRenderer : MonoBehaviour
{
    [SerializeField] private RuntimeAnimatorController[] _animatorControllers;
    [SerializeField] private Animator _animator;

    public void RenderCharacter(CharacterData data)
    {
        if (data is PlayerData)
        {
            PlayerData playerData = data as PlayerData;
            _animator.runtimeAnimatorController = _animatorControllers[(int)playerData.GetCharacter()];
        }
    }

    public void SetFloat(string name, float data)
    {
        _animator.SetFloat(name, data);
    }

    public void SetBool(string name, bool data)
    {
        _animator.SetBool(name, data);
    }
}
