using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour, IMoveableEntity
{
    [SerializeField] private float startSpeed;
    [Header("SFX")] [SerializeField] private AudioSource playerEffectSource;
    [SerializeField] private AudioClip footSteps;
    [SerializeField] [Range(0, 1f)] private float stepFrequency;
    [SerializeField] private LayerMask solidLayers;
    [SerializeField] private float gravity;

    private CharacterController cc;
    private float currentSpeed;

    public static PlayerMovement Instance { get; private set; }
    private Vector3 inputVector = Vector3.zero;

    public Vector3 MovementDir => inputVector;
    public float CurrentSpeed => currentSpeed;

    void Awake()
    {
        cc = GetComponent<CharacterController>();
        Instance = this;
        currentSpeed = startSpeed;
    }

    void Update()
    {
        if (GlobalStateManager.Instance.CurrentState != GameState.Running) return;
        if (inputVector.magnitude > 0)
        {
            cc.SimpleMove(transform.rotation * inputVector * currentSpeed);
        }

        if (!cc.isGrounded)
        {
            cc.Move(new Vector3(0, -gravity * Time.deltaTime, 0));
        }
    }

    public void MoveTo(Vector3 point)
    {
        var dir = point - transform.position;
        var dist = dir.magnitude;
        if (Physics.Raycast(transform.position, dir, out RaycastHit hit, dist, solidLayers))
        {
            point = hit.point;
        }

        cc.enabled = false;
        transform.position = point;
        cc.enabled = true;
    }

    public void MoveBy(Vector3 force)
    {
        var dir = force;
        var dist = dir.magnitude;
        if (Physics.Raycast(transform.position, dir, out RaycastHit hit, dist, solidLayers))
        {
            force = hit.point - transform.position;
        }

        cc.Move(force);
    }

    private void OnMove(InputValue val)
    {
        var temp = val.Get<Vector2>();
        inputVector.x = temp.x;
        inputVector.z = temp.y;
        inputVector.y = 0;
    }

    private void Start()
    {
        EventStore.Instance.OnPlayerMaxSpeedChange += OnPlayerMaxSpeedChange;
    }

    private void OnPlayerMaxSpeedChange(float changePercentage)
    {
        currentSpeed *= (1 + changePercentage / 100);
    }
}