using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FacingDirection
{
    DIR_UP,
    DIR_DOWN,
    DIR_LEFT,
    DIR_RIGHT
}

public class CharacterRenderer : MonoBehaviour
{
    public Animator animator;
    public FacingDirection defaultDirection;

    public void RenderCharacter(CharacterData data)
    {
        animator.runtimeAnimatorController = data.characterAnimController;
    }

    public void SetFloat(string name, float data)
    {
        animator.SetFloat(name, data);
    }

    public float GetFloat(string name)
    {
        return animator.GetFloat(name);
    }

    public void SetBool(string name, bool data)
    {
        animator.SetBool(name, data);
    }

    public void SetFacingDirection(FacingDirection dir)
    {
        switch (dir)
        {
            case FacingDirection.DIR_UP:
                animator.SetFloat("x", 0);
                animator.SetFloat("y", 1);
                break;
            case FacingDirection.DIR_DOWN:
                animator.SetFloat("x", 0);
                animator.SetFloat("y", -1);
                break;
            case FacingDirection.DIR_LEFT:
                animator.SetFloat("x", -1);
                animator.SetFloat("y", 0);
                break;
            case FacingDirection.DIR_RIGHT:
                animator.SetFloat("x", 1);
                animator.SetFloat("y", 0);
                break;
        }
    }
}
