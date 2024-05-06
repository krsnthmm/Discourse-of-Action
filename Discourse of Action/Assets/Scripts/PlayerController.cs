using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private CharacterMovement _characterMovement;
    [SerializeField] private Animator _mascAnimator, _femAnimator;
    [SerializeField] private PlayerData _playerData;

    private float _horizontalAxis, _verticalAxis;
    private Animator _selectedAnimator;

    // Start is called before the first frame update
    void Start()
    {
        _selectedAnimator = _femAnimator;
    }

    //public void ReadMovementAxisCommand(MovementAxisCommand command)
    //{
    //    _horizontalAxis = command.HorizontalAxis;
    //    _verticalAxis = command.VerticalAxis;
    //}

    // Update is called once per frame
    void Update()
    {
        bool isWalking = _horizontalAxis != 0 || _verticalAxis != 0;

        _horizontalAxis = Input.GetAxisRaw("Horizontal");
        _verticalAxis = Input.GetAxisRaw("Vertical");

        if (isWalking)
        {
            _characterMovement.Move(_horizontalAxis, _verticalAxis);
            _selectedAnimator.SetBool("isWalking", isWalking);
            _selectedAnimator.SetFloat("x", _horizontalAxis);
            _selectedAnimator.SetFloat("y", _verticalAxis);
        }
    }
}
