using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class KeyPointData : ScriptableObject
{
    public ElementTypes keyPointType;
    [TextArea] public string keyPointText;
}
