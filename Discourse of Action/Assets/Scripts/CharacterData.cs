using UnityEngine;

public abstract class CharacterData : ScriptableObject
{
    public string characterName;
    public int currHealth;
    private int _maxHealth = 100;

    void Awake()
    {
        currHealth = _maxHealth;
    }

    public int GetMaxHealth()
    {
        return _maxHealth;
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

        if (currHealth > _maxHealth)
            currHealth = _maxHealth;
    }

    public void SetMaxHealth(int value)
    {
        _maxHealth = value;
    }
}
