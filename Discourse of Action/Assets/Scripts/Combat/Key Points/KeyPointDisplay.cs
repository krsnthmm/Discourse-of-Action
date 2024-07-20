using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KeyPointDisplay : MonoBehaviour
{
    private KeyPoint _keyPoint;
    [SerializeField] private Animator _animator;

    [Header("[PROPERTIES]")]
    [SerializeField] private TMP_Text _keyPointText;
    [SerializeField] private Image _typeImage;

    [Header("[SPRITE ASSETS]")]
    [SerializeField] private Sprite _neutralTypeBubble;
    [SerializeField] private Sprite _reasoningTypeBubble;
    [SerializeField] private Sprite _emotionTypeBubble;
    [SerializeField] private Sprite _instinctTypeBubble;

    public void OnEnable()
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

    public void OnHover()
    {
        _animator.SetBool("Hover", true);
    }

    public void OnLeave()
    {
        _animator.SetBool("Hover", false);
    }
}
