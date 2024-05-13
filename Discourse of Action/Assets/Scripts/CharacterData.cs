using UnityEngine;

public abstract class CharacterData : ScriptableObject
{
    public string characterName;
    public RuntimeAnimatorController characterAnimController;
    public int currHealth;
    public int maxHealth = 100;

    void Awake()
    {
        currHealth = maxHealth;
    }

    public void TakeDamage(int value)
    {
        currHealth -= value;

        if (currHealth < 0)
            currHealth = 0;
    }

    public void RestoreHealth(int value)
    {
        currHealth += value;

        if (currHealth > maxHealth)
            currHealth = maxHealth;
    }
}
