using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterRenderer : MonoBehaviour
{
    public Animator animator;

    public void RenderCharacter(CharacterData data)
    {
        animator.runtimeAnimatorController = data.characterAnimController;
    }

    public void SetFloat(string name, float data)
    {
        animator.SetFloat(name, data);
    }

    public void SetBool(string name, bool data)
    {
        animator.SetBool(name, data);
    }
}
