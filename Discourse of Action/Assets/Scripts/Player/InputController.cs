using UnityEngine;

public abstract class Command
{
    public float Time { get; private set; }
    public Command(float time) => Time = time;
}

public class MovementAxisCommand : Command
{
    public float HorizontalAxis { get; private set; }
    public float VerticalAxis { get; private set; }

    public MovementAxisCommand(float time, float horizontalAxis, float verticalAxis) : base(time)
    {
        HorizontalAxis = horizontalAxis;
        VerticalAxis = verticalAxis;
    }
}

public class KeyboardInputCommand : Command
{
    public string Action { get; private set; }

    public KeyboardInputCommand(float time, string action) : base(time)
    {
        Action = action;
    }
}

public class InputController : MonoBehaviour
{
    private MovementAxisCommand _lastMovementAxisCommand = new MovementAxisCommand(0, 0, 0);
    [SerializeField] private float _replayButtonToggledTime;

    public bool TryGetMovementAxisInput(out MovementAxisCommand movementAxisCommand)
    {
        float horizontalAxis = Input.GetAxisRaw("Horizontal");
        float verticalAxis = Input.GetAxisRaw("Vertical");
        bool hasAxisInputChanged = _lastMovementAxisCommand.HorizontalAxis != horizontalAxis || _lastMovementAxisCommand.VerticalAxis != verticalAxis;

        if (hasAxisInputChanged)
            _lastMovementAxisCommand = new MovementAxisCommand(Time.time - _replayButtonToggledTime, horizontalAxis, verticalAxis);

        movementAxisCommand = _lastMovementAxisCommand;

        return hasAxisInputChanged;
    }

    public bool TryGetKeyboardInput(out KeyboardInputCommand keyboardInputCommand)
    {
        keyboardInputCommand = null;
        if (Input.GetKeyDown(KeyCode.F))
            keyboardInputCommand = new KeyboardInputCommand(Time.time - _replayButtonToggledTime, "Interact");
        else if (Input.GetKeyDown(KeyCode.LeftControl))
            keyboardInputCommand = new KeyboardInputCommand(Time.time - _replayButtonToggledTime, "Crouch");
        else if (Input.GetKeyDown(KeyCode.Space))
            keyboardInputCommand = new KeyboardInputCommand(Time.time - _replayButtonToggledTime, "Jump");
        else if (Input.GetKeyDown(KeyCode.Alpha1))
            keyboardInputCommand = new KeyboardInputCommand(Time.time - _replayButtonToggledTime, "Switch_0");
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            keyboardInputCommand = new KeyboardInputCommand(Time.time - _replayButtonToggledTime, "Switch_M");
        else if (Input.GetKeyDown(KeyCode.Alpha3))
            keyboardInputCommand = new KeyboardInputCommand(Time.time - _replayButtonToggledTime, "Switch_R");
        return keyboardInputCommand != null;
    }
}
