using System;
using UnityEngine;

namespace CodeBase.GameCore.Map.Factory
{
    [CreateAssetMenu(menuName = nameof(MapConfiguration), order = default)]
    public sealed class MapConfiguration : ScriptableObject
    {
        [Header("General")]
        [SerializeField] private GameObject _primaryCell;
        [SerializeField] private Vector3 _mapOrigin;
        [SerializeField] private Vector2 _mapScale;
            
        [Header("Obstacle")]
        [SerializeField] private GameObject[] _obstacles;
        [SerializeField, Range(0,1)] private float _obstacleDensity;
        [SerializeField, Range(1, 5)] private int _obstacleMaxSize;

        [Header("SpawnPoints")] 
        [SerializeField] private SpawnPoint _playerSpawnCell;
        [SerializeField] private SpawnPoint _enemySpawnCell;

        public GameObject PrimaryCell => _primaryCell;
        public Vector3 MapOrigin => _mapOrigin;
        public Vector2 MapScale => _mapScale;
        public int CellCount => (int)(_mapScale.x * _mapScale.y);

        public GameObject[] Obstacles => _obstacles;
        public int ObstacleCount =>  (int)(CellCount * _obstacleDensity);
        public int ObstacleMaxSize => _obstacleMaxSize;
            
        public Vector2 PlayerSpawnCell => TranslateSpawnPoint(_playerSpawnCell);
        public Vector2 EnemySpawnCell => TranslateSpawnPoint(_enemySpawnCell);

        private Vector2 TranslateSpawnPoint(SpawnPoint spawnPoint)
            => spawnPoint switch
            {
                SpawnPoint.UpperLeftCorner => new (0, _mapScale.y - 1),
                SpawnPoint.UpperRightCorner => new (_mapScale.x - 1,_mapScale.y - 1),
                SpawnPoint.DownLeftCorner => new (0,0),
                SpawnPoint.DownRightCorner => new (_mapScale.x - 1,0),
                _ => throw new ArgumentOutOfRangeException(nameof(spawnPoint), spawnPoint, null)
            };

        private enum SpawnPoint
        {
            UpperLeftCorner,
            UpperRightCorner,
            DownLeftCorner,
            DownRightCorner,
        }
    }
}