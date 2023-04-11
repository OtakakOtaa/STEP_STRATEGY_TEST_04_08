using UnityEngine;

namespace CodeBase.GameCore.Map.Data
{
    public sealed class GameMap
    {
        private readonly MapCell[,] _mapCells;
        private readonly Transform _playerSpawn;
        private readonly Transform _enemySpawn;

        public GameMap(MapCell[,] mapCells, Transform playerSpawn, Transform enemySpawn)
        {
            _mapCells = mapCells;
            _playerSpawn = playerSpawn;
            _enemySpawn = enemySpawn;
        }

        public MapCell GetCellByIndex(Vector2 index)
            => _mapCells[(int)index.y, (int)index.x]; 

        public Vector2? TryGetIndexByPosition(Vector3 position)
        {
            for (var i = 0; i < _mapCells.GetLength(0); i++)
            for (var j = 0; j < _mapCells.GetLength(1); j++)
                if (_mapCells[i, j].Position.position.x == position.x
                    && _mapCells[i, j].Position.position.z == position.z)
                    return new Vector2(j, i);
            return default;
        }

        public Transform PlayerSpawn => _playerSpawn;
        public Transform EnemySpawn => _enemySpawn;
        public Vector2 MapScale => new Vector2(_mapCells.GetLength(1), _mapCells.GetLength(0));
    }
}