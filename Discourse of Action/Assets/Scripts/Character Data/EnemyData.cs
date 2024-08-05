using UnityEngine;

[CreateAssetMenu]
public class EnemyData : CharacterData
{
    public enum EnemyType
    {
        ENEMY_NORMAL = 0,
        ENEMY_BOSS_ASTER,
        ENEMY_BOSS_PRIMROSE,
        ENEMY_BOSS_IRIS_PHASE1,
        ENEMY_BOSS_IRIS_PHASE2
    }

    public EnemyType enemyType;
    public KeyPointDeck keyPointDeck;

    public bool hasWonAgainst;
}
