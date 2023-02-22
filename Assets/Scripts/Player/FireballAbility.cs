using System.Collections;
using UnityEngine;

public class FireballAbility : MonoBehaviour
{
    [SerializeField] private FireballController fireballPrefab;
    [SerializeField] private Transform fireballLocation;
    [SerializeField] private float fireRate;
    [SerializeField] private Transform aimPoint;

    private FireballController currentBall;

    void Update()
    {
        if (GlobalStateManager.Instance.CurrentState != GameState.Running) return;
        if (currentBall == null)
        {
            currentBall = Instantiate(fireballPrefab, fireballLocation.position, Quaternion.identity, fireballLocation);
        }

        if (Input.GetButtonDown("Fire1"))
        {
            currentBall.Launch(aimPoint.forward);
            StartCoroutine(WaitAndReset());
        }
    }

    private IEnumerator WaitAndReset()
    {
        yield return new WaitForSeconds(fireRate);
        currentBall = null;
    }
}