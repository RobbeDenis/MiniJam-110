using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Wave", menuName = "Waves/Wave")]
public class Wave : ScriptableObject
{
    [field: SerializeField]
    public List<SpawnData> Spawners { get; private set; }
}
