using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CardManager : MonoBehaviour
{
    public List<Card> deck = new();
    public List<Card> discardPile = new();
    public List<Card> hand = new();

    public Transform[] handSlots;
    public bool[] availableHandSlots;

    public TMP_Text deckSizeText;
    public Card selectedCard;

    public void DrawCard()
    {
        if (deck.Count > 0)
        {
            Card randCard = deck[Random.Range(0, deck.Count)];

            for (int i = 0; i < availableHandSlots.Length; i++)
            {
                if (availableHandSlots[i] == true)
                {
                    randCard.gameObject.SetActive(true);
                    randCard.handIndex = i;
                    randCard.transform.position = handSlots[i].position;
                    randCard.hasBeenPlayed = false;

                    availableHandSlots[i] = false;
                    deck.Remove(randCard);
                    hand.Add(randCard);

                    break;
                }
            }
        }
        else if (deck.Count <= 0 && hand.Count <= 0)
            TransferAllToDeck();

        UpdateDeck();
    }

    public void TransferAllToDeck()
    {
        foreach (Card card in discardPile)
            deck.Add(card);
        
        discardPile.Clear();
    }

    void UpdateDeck()
    {
        deckSizeText.text = (deck.Count + hand.Count) + " CARDS LEFT";
    }
}
