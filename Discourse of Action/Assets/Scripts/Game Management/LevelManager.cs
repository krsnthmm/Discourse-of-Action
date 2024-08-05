using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private LevelEnemyCounter _levelEnemyCounter;

    private void OnEnable()
    {
        if (_levelEnemyCounter.CheckForCondition())
            GameManager.instance.SetFlag(_levelEnemyCounter.requiredFlag);
    }
}
