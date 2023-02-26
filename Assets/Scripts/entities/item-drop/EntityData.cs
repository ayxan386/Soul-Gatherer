using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/EntityData", order = 2)]
public class EntityData : ScriptableObject
{
    public string id;
    public new string name;
    public Sprite smallDisplayIcon;
    public EntityType type;
}

public enum EntityType
{
    Exp,
    Gold,
    SoulShard
}