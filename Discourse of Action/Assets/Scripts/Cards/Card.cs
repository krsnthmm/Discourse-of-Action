using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CardDisplay))]
public class Card : MonoBehaviour
{
    public CardData cardData;
    public int damage;

    private void Start()
    {
        damage = ((int)cardData.cardTier + 1) * cardData.baseDamage;
    }
}
