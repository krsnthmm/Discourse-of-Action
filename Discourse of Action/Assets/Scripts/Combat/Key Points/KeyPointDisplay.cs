using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KeyPointDisplay : MonoBehaviour
{
    private KeyPoint _keyPoint;

    [Header("[PROPERTIES]")]
    [SerializeField] private TMP_Text _keyPointText;
    [SerializeField] private Image _typeImage;
    [SerializeField] private Image _targetImage;

    [Header("[SPRITE ASSETS]")]
    [SerializeField] private Sprite _neutralTypeBubble;
    [SerializeField] private Sprite _reasoningTypeBubble;
    [SerializeField] private Sprite _emotionTypeBubble;
    [SerializeField] private Sprite _instinctTypeBubble;

    private void Awake()
    {
        _keyPoint = GetComponent<KeyPoint>();
        SetKeyPointDisplay();
    }

    private void SetKeyPointDisplay()
    {
        SetKeyPointText();
        SetTypeIcon();
    }


    private void SetKeyPointText()
    {
        _keyPointText.text = _keyPoint.keyPointData.keyPointText;
    }

    private void SetTypeIcon()
    {
        switch (_keyPoint.keyPointData.keyPointType)
        {
            case ElementTypes.TYPE_NEUTRAL:
                _typeImage.sprite = _neutralTypeBubble;
                break;
            case ElementTypes.TYPE_REASONING:
                _typeImage.sprite = _reasoningTypeBubble;
                break;
            case ElementTypes.TYPE_EMOTION:
                _typeImage.sprite = _emotionTypeBubble;
                break;
            case ElementTypes.TYPE_INSTINCT:
                _typeImage.sprite = _instinctTypeBubble;
                break;
        }
    }
}
