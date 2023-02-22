using UnityEngine;
using UnityEngine.VFX;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(VisualEffect))]
public class FireballController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private AudioClip collisionSound;
    [SerializeField] private AudioClip fireballLaunchSound;
    [SerializeField] private float lifespan;
    [SerializeField] private float forceFactor;
    [SerializeField] private float explosionRad;
    private Rigidbody rb;
    private VisualEffect fireballEffect;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        fireballEffect = GetComponent<VisualEffect>();
        fireballEffect.SetInt("collisionFactor", 1);
    }

    public void Launch(Vector3 dir)
    {
        transform.parent = null;
        rb.velocity = dir * speed;
        fireballEffect.SetInt("diskCount", 0);
        fireballEffect.Stop();
        Destroy(gameObject, lifespan);
        AudioSource.PlayClipAtPoint(fireballLaunchSound, Vector3.zero);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.TryGetComponent(out Rigidbody cr))
        {
            cr.AddExplosionForce(forceFactor, transform.position, explosionRad);
            AudioSource.PlayClipAtPoint(collisionSound, Vector3.zero);
            fireballEffect.SetInt("collisionFactor", -1);

            // if (collision.collider.TryGetComponent(out BlockController bc))
            // {
            //     bc.SetOnFire();
            // }
        }
    }
}