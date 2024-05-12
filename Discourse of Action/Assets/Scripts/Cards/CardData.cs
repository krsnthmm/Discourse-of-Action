using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CardData : ScriptableObject
{
    public enum CardTier
    {
        TIER_NORMAL,
        TIER_RARE,
        TIER_SUPERRARE
    }
    public enum CardType
    {
        TYPE_REASONING,
        TYPE_INSTINCT,
        TYPE_EMOTION
    }

    public CardTier cardTier;
    public CardType cardType;

    public int baseDamage = 5;
}
