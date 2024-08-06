using UnityEngine;

public class PlayerController : Character
{
    [SerializeField] private Canvas _promptCanvas;
    private float _horizontalAxis, _verticalAxis;

    void OnEnable ()
    {
        _renderer.RenderCharacter(_data);
        HandleFacingDirection(_renderer.defaultDirection);

        CheckIfInteractableInRange();
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
                if (!GameManager.instance.isPaused)
                    HandleInteractKeyPress();
                break;
            case "Pause":
                GameManager.instance.HandlePauseKeyPress();
                break;
            case "Advance Dialogue":
                GameManager.instance.HandleDialogueKeyPress();
                break;
            // CHEATS: start at a specific level
            case "Level 1":
                GameManager.instance.HandleLevelKeyPress(1);
                break;
            case "Level 2":
                GameManager.instance.HandleLevelKeyPress(2);
                break;
            case "Level 3":
                GameManager.instance.HandleLevelKeyPress(3);
                break;
            // CHEATS: advance battle
            case "Advance Battle":
                GameManager.instance.HandleBattleKeyPress();
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

        CheckIfInteractableInRange();
        _renderer.SetBool("isWalking", _movement.isMoving);
    }

    void CheckIfInteractableInRange()
    {        
        // check where the player is facing
        var facingDir = new Vector3(_renderer.GetFloat("x"), _renderer.GetFloat("y"));
        var interactPos = transform.position + facingDir / 2;

        var collider = Physics2D.OverlapCircle(interactPos, 0.1f, GameLayers.Instance.InteractableLayer);

        if (collider != null)
            _promptCanvas.enabled = true;
        else
            _promptCanvas.enabled = false;
    }

    void HandleInteractKeyPress()
    {
        // check where the player is facing
        var facingDir = new Vector3(_renderer.GetFloat("x"), _renderer.GetFloat("y"));
        var interactPos = transform.position + facingDir / 2;

        var collider = Physics2D.OverlapCircle(interactPos, 0.1f, GameLayers.Instance.InteractableLayer);

        if (collider != null)
        {
            collider.GetComponent<Interactable>()?.Interact(transform);
            _promptCanvas.enabled = false;
        }
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
