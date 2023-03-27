using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class ChestController : ItemInteractionBehavior, ILoadableEntity
{
    [SerializeField] private Transform lidTransform;
    [SerializeField] private float speed;
    [SerializeField] private float maxAngle;
    [FormerlySerializedAs("id")] [SerializeField] private int assignedId;

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
        entityData.instanceId = GetId();
        return entityData;
    }
    
    public void SetId(int id)
    {
        this.assignedId = id;
    }

    public int GetId()
    {
        return assignedId;
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}