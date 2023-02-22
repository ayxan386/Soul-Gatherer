using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.VFX;

public class TeleportationAbility : MonoBehaviour
{
    [SerializeField] private KeyCode keyName;

    [FormerlySerializedAs("effectsAroundLayer")] [FormerlySerializedAs("playerEffect")] [Header("FX")] [SerializeField]
    private VisualEffect effectsAroundPlayer;

    [SerializeField] private AudioSource playerEffectSource;
    [SerializeField] private AudioClip swooshSound;

    [Header("Ray-casting")] [SerializeField]
    private Transform shootingPoint;

    [SerializeField] private LayerMask effectedLayers;
    [SerializeField] private float maxDistance;
    [SerializeField] private VisualEffect indicator;
    [SerializeField] private float verticalOffset;

    private RaycastHit lastHitPoint;
    private bool isActive;

    private void Start()
    {
        effectsAroundPlayer.Stop();
        indicator.Stop();
    }

    void Update()
    {
        if (GlobalStateManager.Instance.CurrentState != GameState.Running) return;
        
        if (Input.GetKey(keyName))
        {
            isActive = Physics.Raycast(shootingPoint.position, shootingPoint.forward, out lastHitPoint, maxDistance,
                effectedLayers);
        }
        else if (Input.GetKeyUp(keyName) && isActive)
        {
            playerEffectSource.PlayOneShot(swooshSound);
            PlayerMovement.Instance.MoveTo(lastHitPoint.point + lastHitPoint.normal * verticalOffset);
        }
        else
        {
            isActive = false;
        }

        if (isActive && !effectsAroundPlayer.pause)
        {
            indicator.transform.position = lastHitPoint.point;
            indicator.Play();
            effectsAroundPlayer.Play();
        }
        else
        {
            indicator.Stop();
            effectsAroundPlayer.Stop();
        }
    }
}