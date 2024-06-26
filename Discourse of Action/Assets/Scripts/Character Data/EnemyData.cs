using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EnemyData : CharacterData
{
    public enum EnemyType
    {
        ENEMY_NORMAL,
        ENEMY_BOSS_ASTER,
        ENEMY_BOSS_PRIMROSE
    }

    public EnemyType enemyType;
    public KeyPointDeck keyPointDeck;
}
