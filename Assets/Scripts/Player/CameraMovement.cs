using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Vector2 verticalLimits;
    [SerializeField] private Vector2 mouseSensetivity;
    [SerializeField] private Vector2 gamePadSensitivity;
    [SerializeField] private Vector2 shakingFactor;

    private float verticalRotation;
    private int dir = 1;
    private float randomness;
    private Vector2 inputVector;

    private void Start()
    {
        GlobalStateManager.Instance.PausedGame();
        GlobalStateManager.Instance.RunningGame();
    }

    void Update()
    {
        if (GlobalStateManager.Instance.CurrentState != GameState.Running) return;
        var sensitivity = mouseSensetivity;
        if (GlobalStateManager.Instance.CurrentScheme == ControlSchemes.Gamepad)
        {
            sensitivity = gamePadSensitivity;
        }

        transform.parent.Rotate(0, sensitivity.x * inputVector.x * Time.deltaTime, 0);

        verticalRotation -= sensitivity.y * inputVector.y * Time.deltaTime;
        verticalRotation = Mathf.Clamp(verticalRotation, verticalLimits.x, verticalLimits.y);

        var eulerAngles = transform.localEulerAngles;
        eulerAngles.x = verticalRotation;
        transform.localEulerAngles = eulerAngles;
    }

    private void OnRotate(InputValue inputValue)
    {
        inputVector = inputValue.Get<Vector2>();
    }
}