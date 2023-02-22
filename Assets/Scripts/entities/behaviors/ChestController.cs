using System.Collections;
using UnityEngine;

public class ChestController : ItemInteractionBehavior
{
    [SerializeField] private Transform lidTransform;
    [SerializeField] private float speed;
    [SerializeField] private float maxAngle;

    public override void Interact(InteractionPassData data)
    {
        if (data.WasInteractedBefore) return;
        StartCoroutine(OpenLid());
    }

    private IEnumerator OpenLid()
    {
        float currentAngle = lidTransform.eulerAngles.x;
        while (Mathf.Abs(currentAngle) < maxAngle)
        {
            currentAngle += speed * Time.deltaTime;
            lidTransform.Rotate(speed * Time.deltaTime, 0, 0);
            yield return new WaitForEndOfFrame();
        }

        Complete = true;
    }
}