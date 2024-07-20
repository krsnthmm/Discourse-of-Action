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

    public CardTier cardTier;
    public ElementTypes cardType;

    public int baseDamage = 5;
}
