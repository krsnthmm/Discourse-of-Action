using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardDisplay : MonoBehaviour
{
    private Card _card;

    [Header("[CARD ELEMENTS]")]
    [SerializeField] private TMP_Text _tierText;
    [SerializeField] private Image _typeImage;

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
                break;
            case CardData.CardTier.TIER_RARE:
                _tierText.text = "R";
                break;
            case CardData.CardTier.TIER_SUPERRARE:
                _tierText.text = "SR";
                break;
        }
    }

    private void SetTypeIcon()
    {
        switch (_card.cardData.cardType)
        {
            case CardData.CardType.TYPE_REASONING:
                _typeImage.sprite = _reasoningTypeIcon;
                break;
            case CardData.CardType.TYPE_EMOTION:
                _typeImage.sprite = _emotionTypeIcon;
                break;
            case CardData.CardType.TYPE_INSTINCT:
                _typeImage.sprite = _instinctTypeIcon;
                break;
        }
    }
}
