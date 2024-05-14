using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CardDisplay))]
public class Card : MonoBehaviour
{
    public CardData cardData;
    public bool hasBeenPlayed;
    public int handIndex;
    public int damage;

    private CardManager _cardManager;

    private void Start()
    {
        _cardManager = FindObjectOfType<CardManager>();
        damage = ((int)cardData.cardTier + 1) * cardData.baseDamage;
    }

    public void PlayCard()
    {
        if (!hasBeenPlayed)
        {
            _cardManager.selectedCard = this;
            _cardManager.hand.Remove(this);
            hasBeenPlayed = true;

            _cardManager.availableHandSlots[handIndex] = true;
            MoveToDiscardPile();
        }
    }

    void MoveToDiscardPile()
    {
        _cardManager.discardPile.Add(this);
        gameObject.SetActive(false);
    }
}
