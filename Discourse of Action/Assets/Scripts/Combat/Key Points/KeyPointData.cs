using UnityEngine;

[CreateAssetMenu]
public class KeyPointData : ScriptableObject
{
    public ElementTypes keyPointType;
    [TextArea] public string keyPointText;
}
