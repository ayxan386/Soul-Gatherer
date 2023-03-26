using System.Collections;
using UnityEngine;

public class ChestController : ItemInteractionBehavior, ILoadableEntity
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

    public void LoadData(LoadableEntityData data)
    {
        lidTransform.rotation = data.rotation;
    }

    public LoadableEntityData GetData()
    {
        var entityData = new LoadableEntityData();
        entityData.rotation = lidTransform.rotation;
        entityData.instanceId = GetInstanceID();
        return entityData;
    }

    public int GetId()
    {
        return GetInstanceID();
    }
       public void Destroy()
        {
            Destroy(gameObject);
        }
}