using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : ScriptableObject
{
    private int _currHealth;
    private int _maxHealth = 100;
    private string _name;
    public enum Character
    {
        CHARACTER_MASC,
        CHARACTER_FEM
    }

    private Character _selectedCharacter;

    // Start is called before the first frame update
    void Start()
    {
        _currHealth = _maxHealth;
    }

    public int GetCurrHealth()
    {
        return _currHealth;
    }

    public int GetMaxHealth()
    {
        return _maxHealth;
    }

    public void TakeDamage(int value)
    {
        _currHealth -= value;

        if (_currHealth < 0)
            _currHealth = 0;
    }

    public void RestoreHealth(int value)
    {
        _currHealth += value;

        if (_currHealth > _maxHealth)
            _currHealth = _maxHealth;
    }

    public void SetMaxHealth(int value)
    {
        _maxHealth = value;
    }

    public Character GetCharacter()
    {
        return _selectedCharacter;
    }

    public void SetCharacter(Character character)
    {
        _selectedCharacter = character;
    }
}
