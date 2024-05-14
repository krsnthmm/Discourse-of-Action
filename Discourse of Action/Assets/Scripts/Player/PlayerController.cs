using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private CharacterMovement _characterMovement;
    [SerializeField] private PlayerData _playerData;
    [SerializeField] private CharacterRenderer _playerRenderer;

    private float _horizontalAxis, _verticalAxis;

    // Start is called before the first frame update
    void Start()
    {
        _playerRenderer.RenderCharacter(_playerData);
    }

    public void ReadMovementAxisCommand(MovementAxisCommand command)
    {
        _horizontalAxis = command.HorizontalAxis;
        _verticalAxis = command.VerticalAxis;
    }

    public void UpdateTransform()
    {
        bool isWalking = _horizontalAxis != 0 || _verticalAxis != 0;

        if (isWalking)
        {
            _characterMovement.Move(_horizontalAxis, _verticalAxis);
            _playerRenderer.SetFloat("x", _horizontalAxis);
            _playerRenderer.SetFloat("y", _verticalAxis);
        }
        
        _playerRenderer.SetBool("isWalking", isWalking);
    }
}
