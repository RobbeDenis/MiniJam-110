using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyTypes
{
    Basic,
    Kamikaze,
    Bull,
    Wisp
}

[CreateAssetMenu(fileName = "SpawnData", menuName = "Waves/Spawndata")]
public class SpawnData : ScriptableObject
{
    [field: SerializeField]
    public List<EnemyTypes> Enemies { get; private set; }

    [field: SerializeField]
    public Vector3 SpawnerLocation { get; private set; }
}
