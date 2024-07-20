using UnityEngine;

[CreateAssetMenu]
public class RecallConclusionData : ScriptableObject
{
    [TextArea] public string[] conclusionStartOptions;
    [TextArea] public string[] conclusionMiddleOptions;
    [TextArea] public string[] conclusionEndOptions;

    [TextArea] public string correctConclusion;
}
