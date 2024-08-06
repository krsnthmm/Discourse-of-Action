using UnityEngine;

public class LevelEnemyCounter : MonoBehaviour
{
    [SerializeField] private Character[] _enemies;
    public GameFlags requiredFlag;

    public bool enemiesDefeated = false;

    public bool CheckForCondition()
    {
        for (int i = 0; i < _enemies.Length; i++)
        {
            EnemyData enemyData = (EnemyData)_enemies[i].GetData();

            if (!enemyData.hasWonAgainst)
            {
                enemiesDefeated = false; // if any enemy hasn't been defeated, condition = false
                break;
            }
            else
            {
                enemiesDefeated = true;
                continue;
            }
        }

        return enemiesDefeated;
    }
}
