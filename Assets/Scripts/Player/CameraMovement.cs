using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Vector2 verticalLimits;
    [SerializeField] private Vector2 mouseSensetivity;
    [SerializeField] private Vector2 gamePadSensitivity;
    [SerializeField] private Vector2 shakingFactor;
    [SerializeField] private float shakingIncrement;

    private float verticalRotation;
    private float randomness;
    private Vector2 inputVector;
    private bool isMoving;
    private Vector3 initialPos;
    private float offset;

    public float GamepadSensitivityMult { get; set; }

    private void Start()
    {
        GlobalStateManager.Instance.PausedGame();
        GlobalStateManager.Instance.RunningGame();
        initialPos = transform.localPosition;
    }

    void Update()
    {
        if (GlobalStateManager.Instance.CurrentState != GameState.Running) return;
        var sensitivity = mouseSensetivity;
        if (GlobalStateManager.Instance.CurrentScheme == ControlSchemes.Gamepad)
        {
            sensitivity = gamePadSensitivity * GamepadSensitivityMult;
        }

        transform.parent.Rotate(0, sensitivity.x * inputVector.x * Time.deltaTime, 0);

        verticalRotation -= sensitivity.y * inputVector.y * Time.deltaTime;
        verticalRotation = Mathf.Clamp(verticalRotation, verticalLimits.x, verticalLimits.y);

        var eulerAngles = transform.localEulerAngles;
        eulerAngles.x = verticalRotation;
        transform.localEulerAngles = eulerAngles;

        if (isMoving)
        {
            offset += Time.deltaTime * shakingIncrement;
            transform.localPosition = new Vector3(initialPos.x + Mathf.Cos(offset) * shakingFactor.x,
                initialPos.y + Mathf.Sin(offset) * shakingFactor.y,
                initialPos.z);
        }
        else
        {
            transform.localPosition = initialPos;
        }
    }

    private void OnRotate(InputValue inputValue)
    {
        inputVector = inputValue.Get<Vector2>();
    }

    private void OnMove(InputValue val)
    {
        isMoving = val.Get<Vector2>().magnitude > 0;
    }
}