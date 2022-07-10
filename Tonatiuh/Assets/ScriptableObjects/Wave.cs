using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Wave", menuName = "Waves/wave")]
public class Wave : ScriptableObject
{
    [field: SerializeField]
    public List<Tuple<GameObject, int>> EnemyPrefabsInWave;

    [field: SerializeField]
    public float time;
}
