using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class MemoryRecallData : ScriptableObject
{
    public RecallRevelationData[] revelationDatas;
    public RecallConclusionData conclusionData;
}
