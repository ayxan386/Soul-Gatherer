﻿using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/EntityData", order = 2)]
[Serializable]
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
    SoulShard,
    Relic_Common,
    Relic_Rare,
    Relic_Epic,
}