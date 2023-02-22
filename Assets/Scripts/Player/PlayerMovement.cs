using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [Header("SFX")] [SerializeField] private AudioSource playerEffectSource;
    [SerializeField] private AudioClip footSteps;
    [SerializeField] [Range(0, 1f)] private float stepFrequency;

    private CharacterController cc;

    public static PlayerMovement Instance;

    void Awake()
    {
        cc = GetComponent<CharacterController>();
        Instance = this;
    }

    void Update()
    {
        if (Input.GetAxisRaw("Vertical") != 0)
        {
            cc.SimpleMove(transform.forward * (speed * Input.GetAxisRaw("Vertical")));
            if (Random.value >= stepFrequency) playerEffectSource.PlayOneShot(footSteps);
        }
    }

    public void MoveTo(Vector3 point)
    {
        cc.enabled = false;
        transform.position = point;
        cc.enabled = true;
    }
}