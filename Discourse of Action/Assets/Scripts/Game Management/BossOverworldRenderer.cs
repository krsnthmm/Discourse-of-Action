using UnityEngine;

public class BossOverworldRenderer : CharacterRenderer
{
    [SerializeField] private PlayerData _playerData;

    private void Start()
    {
        RenderCharacter(_playerData);
    }
}
