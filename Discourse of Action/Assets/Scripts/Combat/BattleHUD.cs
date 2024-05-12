using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleHUD : MonoBehaviour
{
    [Header("[UNIT DATA]")]
    public BattleUnit battleUnit;

    [Header("[UI COMPONENTS]")]
    public TMP_Text nameText;
    public TMP_Text hpText;
    public Slider hpSlider;

    public void SetHUD()
    {
        nameText.text = "" + battleUnit.characterData.characterName;
        hpText.text = "HP: " + battleUnit.characterData.currHealth + " / " + battleUnit.characterData.GetMaxHealth();
        hpSlider.value = battleUnit.characterData.currHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
