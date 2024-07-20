using UnityEngine;

[CreateAssetMenu]
public class RecallRevelationData : ScriptableObject
{
    public string memoryPieceText;
    public string[] inferenceTexts;

    public int correctInference;
}
