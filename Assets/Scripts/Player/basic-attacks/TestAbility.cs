using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class TestAbility : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float damage;
    [SerializeField] private float radius;
    [SerializeField] private LayerMask collisionMask;
    [SerializeField] private float lifespan;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        rb.velocity = transform.forward * speed;
        Destroy(gameObject, lifespan);
    }

    private void FixedUpdate()
    {
        var colliders = Physics.OverlapSphere(transform.position, radius, collisionMask);
        if (colliders.Length > 0)
        {
            print("Collided with stuff");
            Destroy(gameObject);
        }
    }
}