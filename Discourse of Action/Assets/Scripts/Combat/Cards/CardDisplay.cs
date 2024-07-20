using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardDisplay : MonoBehaviour
{
    private Card _card;
    [SerializeField] private Animator _animator;

    [Header("[CARD ELEMENTS]")]
    [SerializeField] private TMP_Text _tierText;
    [SerializeField] private Image _typeImage;

    [Header("[TEXT COLORS]")]
    [SerializeField] private Color _normalColor;
    [SerializeField] private Color _rareColor;
    [SerializeField] private Color _superRareColor;

    [Header("[SPRITE ASSETS]")]
    [SerializeField] private Sprite _emotionTypeIcon;
    [SerializeField] private Sprite _reasoningTypeIcon;
    [SerializeField] private Sprite _instinctTypeIcon;

    void Awake()
    {
        _card = GetComponent<Card>();
        SetCardDisplay();
    }

    private void SetCardDisplay()
    {
        if (_card != null && _card.cardData != null)
        {
            SetTierText();
            SetTypeIcon();
        }
    }

    private void SetTierText()
    {
        switch (_card.cardData.cardTier)
        {
            case CardData.CardTier.TIER_NORMAL:
                _tierText.text = "N";
                _tierText.color = _normalColor;
                break;
            case CardData.CardTier.TIER_RARE:
                _tierText.text = "R";
                _tierText.color = _rareColor;
                break;
            case CardData.CardTier.TIER_SUPERRARE:
                _tierText.text = "SR";
                _tierText.color = _superRareColor;
                break;
        }
    }

    private void SetTypeIcon()
    {
        switch (_card.cardData.cardType)
        {
            case ElementTypes.TYPE_REASONING:
                _typeImage.sprite = _reasoningTypeIcon;
                break;
            case ElementTypes.TYPE_EMOTION:
                _typeImage.sprite = _emotionTypeIcon;
                break;
            case ElementTypes.TYPE_INSTINCT:
                _typeImage.sprite = _instinctTypeIcon;
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
