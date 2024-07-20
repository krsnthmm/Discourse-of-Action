using UnityEngine;

public class IntroAnimations : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    public void OnHover()
    {
        _animator.SetBool("Hover", true);
    }

    public void OnLeave()
    {
        _animator.SetBool("Hover", false);
    }
}
