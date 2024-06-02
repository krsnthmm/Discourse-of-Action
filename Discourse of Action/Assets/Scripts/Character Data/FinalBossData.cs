using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class FinalBossData : EnemyData
{
    public RuntimeAnimatorController[] irisAnimControllers;

    public void SetAnimatorController(int selectedCharacterIdx)
    {
        // we want this animator controller to correspond to the one the player has.
        // i.e., if the player is female, then iris will also be female.
        characterAnimController = irisAnimControllers[selectedCharacterIdx];
    }
}
