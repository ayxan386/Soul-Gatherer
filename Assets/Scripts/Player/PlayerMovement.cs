using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour, IMoveableEntity
{
    [SerializeField] private float speed;
    [Header("SFX")] [SerializeField] private AudioSource playerEffectSource;
    [SerializeField] private AudioClip footSteps;
    [SerializeField] [Range(0, 1f)] private float stepFrequency;
    [SerializeField] private LayerMask solidLayers;
    [SerializeField] private float gravity;

    private CharacterController cc;

    public static PlayerMovement Instance;
    private Vector3 inputVector = Vector3.zero;

    void Awake()
    {
        cc = GetComponent<CharacterController>();
        Instance = this;
    }

    void Update()
    {
        if (GlobalStateManager.Instance.CurrentState != GameState.Running) return;
        if (inputVector.magnitude > 0)
        {
            cc.SimpleMove(transform.rotation * inputVector * speed);
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
}