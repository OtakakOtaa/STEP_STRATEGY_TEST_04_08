using System;
using CodeBase.GameCore.Map;
using CodeBase.GameCore.Map.Data;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CodeBase.GameCore.Spawner
{
    public sealed class PlayersSpawner
    {
        private const float SpawnUpFactor = 1.5f;
        private readonly Settings _settings;
        
        public PlayersSpawner(Settings settings)
            => _settings = settings;

        public void Spawn(GameMap gameMap, out Player player, out Player enemy)
        {
            var playerRoot = Object.Instantiate(_settings.Player).transform;
            playerRoot.position = gameMap.PlayerSpawn.position + Vector3.up * SpawnUpFactor;
            
            var enemyRoot = Object.Instantiate(_settings.Enemy).transform;
            enemyRoot.position = gameMap.EnemySpawn.position + Vector3.up * SpawnUpFactor;

            player = new Player(playerRoot);
            enemy = new Player(enemyRoot);
        }
        
        [Serializable] public sealed class Settings
        {
            [SerializeField] private GameObject _playerPref;
            [SerializeField] private GameObject _enemyPref;

            public GameObject Player => _playerPref;
            public GameObject Enemy => _enemyPref;
        }
    }
}