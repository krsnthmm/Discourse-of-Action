using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private CharacterMovement _characterMovement;
    [SerializeField] private Animator[] _animators;
    [SerializeField] private PlayerData _playerData;

    private float _horizontalAxis, _verticalAxis;
    private Animator _selectedAnimator;

    // Start is called before the first frame update
    void Start()
    {
        if (_playerData == null)
            _playerData = ScriptableObject.CreateInstance<PlayerData>();

        RenderCharacter();
    }

    void RenderCharacter()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (i == (int)_playerData.GetCharacter())
                transform.GetChild(i).gameObject.SetActive(true);
            else
                transform.GetChild(i).gameObject.SetActive(false);
        }
        _selectedAnimator = _animators[(int)_playerData.GetCharacter()];
    }

    public void ReadMovementAxisCommand(MovementAxisCommand command)
    {
        _horizontalAxis = command.HorizontalAxis;
        _verticalAxis = command.VerticalAxis;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTransform(); // testing purposes
    }

    public void UpdateTransform()
    {
        // change the following 2 lines accordingly once gamecontroller is done
        _horizontalAxis = Input.GetAxisRaw("Horizontal");
        _verticalAxis = Input.GetAxisRaw("Vertical");

        bool isWalking = _horizontalAxis != 0 || _verticalAxis != 0;

        if (isWalking)
        {
            _characterMovement.Move(_horizontalAxis, _verticalAxis);
            _selectedAnimator.SetFloat("x", _horizontalAxis);
            _selectedAnimator.SetFloat("y", _verticalAxis);
        }
        
        _selectedAnimator.SetBool("isWalking", isWalking);

        // testing purposes
        if (Input.GetKeyDown(KeyCode.Tab))
            SwitchCharacter();
    }

    void SwitchCharacter()
    {
        if ((int)_playerData.GetCharacter() == 1)
            _playerData.SetCharacter(PlayerData.Character.CHARACTER_MASC);
        else
            _playerData.SetCharacter(PlayerData.Character.CHARACTER_FEM);

        RenderCharacter();
    }
}
