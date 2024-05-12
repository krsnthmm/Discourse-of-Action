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
        if (_playerData == null)
            _playerData = ScriptableObject.CreateInstance<PlayerData>();

        _playerRenderer.RenderCharacter(_playerData);
    }

    public void ReadMovementAxisCommand(MovementAxisCommand command)
    {
        _horizontalAxis = command.HorizontalAxis;
        _verticalAxis = command.VerticalAxis;
    }

    // Update is called once per frame
    void Update()
    {
        // testing purposes
        if (Input.GetKeyDown(KeyCode.Tab))
            SwitchCharacter();
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

    void SwitchCharacter()
    {
        if ((int)_playerData.GetCharacter() == 1)
            _playerData.SetCharacter(PlayerData.Character.CHARACTER_MASC);
        else
            _playerData.SetCharacter(PlayerData.Character.CHARACTER_FEM);

        _playerRenderer.RenderCharacter(_playerData);
    }
}
