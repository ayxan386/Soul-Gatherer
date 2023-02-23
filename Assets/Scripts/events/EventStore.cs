using System;
using UnityEngine;

public class EventStore : MonoBehaviour
{
    public static EventStore Instance;

    public event EventHandler<ObtainedEntity> OnEntityObtainedDisplay;

    public event EventHandler<ObtainedEntity> OnEntityObtainedClick;

    private void Awake()
    {
        Instance = this;
    }

    public void PublishEntityObtainedDisplay(ObtainedEntity obtainedEntity)
    {
        OnEntityObtainedDisplay?.Invoke(this, obtainedEntity);
    }

    public void PublishEntityObtainedClick(ObtainedEntity obtainedEntity)
    {
        OnEntityObtainedClick?.Invoke(this, obtainedEntity);
    }
}