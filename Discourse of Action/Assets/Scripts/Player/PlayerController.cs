using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Character
{
    private float _horizontalAxis, _verticalAxis;

    void OnEnable ()
    {
        _renderer.RenderCharacter(_data);
    }

    public void ReadMovementAxisCommand(MovementAxisCommand command)
    {
        _horizontalAxis = command.HorizontalAxis;
        _verticalAxis = command.VerticalAxis;
    }

    public void ReadKeyboardInputCommand(KeyboardInputCommand command)
    {
        switch (command.Action)
        {
            case "Interact":
                HandleInteraction();
                break;
        }
    }

    public void UpdateTransform()
    {
        if (!_movement.isMoving)
        {
            // remove diagonal movement
            if (_horizontalAxis != 0)
                _verticalAxis = 0;

            if (_horizontalAxis != 0 || _verticalAxis != 0)
                HandleMovement(_horizontalAxis, _verticalAxis, CheckIfInOpponentView);
        }

        _renderer.SetBool("isWalking", _movement.isMoving);
    }

    void HandleInteraction()
    {
        Debug.Log("Button pressed");

        // check where the player is facing
        var facingDir = new Vector3(_renderer.GetFloat("x"), _renderer.GetFloat("y"));
        var interactPos = transform.position + facingDir / 2;

        var collider = Physics2D.OverlapCircle(interactPos, 0.1f, GameLayers.Instance.InteractableLayer);

        if (collider != null)
            collider.GetComponent<Interactable>()?.Interact(transform);
    }

    void CheckIfInOpponentView()
    {
        Collider2D collider = Physics2D.OverlapCircle(transform.position, 0.1f, GameLayers.Instance.FOVLayer);

        if (collider != null)
        {
            var opponent = collider.GetComponentInParent<OpponentController>();
            StartCoroutine(opponent.TriggerCombat(this));

            _renderer.SetBool("isWalking", _movement.isMoving);
            GameManager.instance.ChangeState(GameState.GAME_CUTSCENE);
        }
    }
}
