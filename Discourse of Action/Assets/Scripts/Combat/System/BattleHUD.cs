using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleHUD : MonoBehaviour
{
    [Header("[UNIT DATA]")]
    [SerializeField] private BattleUnit _battleUnit;

    [Header("[UI COMPONENTS]")]
    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private TMP_Text _hpText;
    [SerializeField] private Slider _hpSlider;

    public void SetHUD()
    {
        _nameText.text = "" + _battleUnit.characterData.characterName;
        _hpText.text = "HP: " + _battleUnit.characterData.currHealth + " / " + _battleUnit.characterData.maxHealth;
        _hpSlider.maxValue = _battleUnit.characterData.maxHealth;
        _hpSlider.value = _battleUnit.characterData.currHealth;
    }

    public void UpdateHealthValue()
    {
        _hpText.text = "HP: " + _battleUnit.characterData.currHealth + " / " + _battleUnit.characterData.maxHealth;
        _hpSlider.value = _battleUnit.characterData.currHealth;
    }
}
