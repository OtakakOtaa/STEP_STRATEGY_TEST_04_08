using CodeBase.GameCore.Map.Data;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = System.Random;

namespace CodeBase.GameCore.Map.Factory
{
    public sealed class MapCreator
    {
        private readonly MapConfiguration _settings;
        private readonly Transform _root;

        public MapCreator(MapConfiguration settings, Transform root)
        {
            _settings = settings;
            _root = root;
        }

        public GameMap Create()
        {
            InstantiateCells(out var cells);
            ArrangeCells(cells);
            MapCell[,] mapCells = ConvertToCells(cells);
            InitCellTriggers(mapCells);
            FillWithObstacles(mapCells);

            return new GameMap(mapCells, 
                GetCellByIndex(_settings.PlayerSpawnCell, mapCells).Position,
                GetCellByIndex(_settings.EnemySpawnCell, mapCells).Position);
        }

        private void ArrangeCells(Transform[,] cells)
        {
            cells[0, 0].position = _settings.MapOrigin;
            for (var i = 0; i < cells.GetLength(0); i++)
            {
                for (var j = 1; j < cells.GetLength(1); j++)
                {
                    if (j is 1 && i is not 0)
                        cells[i, 0].position = cells[i - 1, 0].position + new Vector3(0, 0, GetOffset().z);
                    cells[i, j].position = cells[i, j - 1].position + new Vector3(GetOffset().x, 0, 0);
                }
            }
        }

        private void InstantiateCells(out Transform[,] cells)
        {
            cells = new Transform[(int)_settings.MapScale.y, (int)_settings.MapScale.x];
            for (var i = 0; i < cells.GetLength(0); i++)
            for (var j = 0; j < cells.GetLength(1); j++)
                cells[i, j] = Object.Instantiate(_settings.PrimaryCell, _root).transform;
        }

        private MapCell[,] ConvertToCells(Transform[,] originCells)
        {
            var cells = new MapCell[originCells.GetLength(0), originCells.GetLength(1)];
            for (var i = 0; i < cells.GetLength(0); i++)
            for (var j = 0; j < cells.GetLength(1); j++)
                cells[i, j] = new MapCell(originCells[i, j]);
            return cells;
        }

        private void InitCellTriggers(MapCell[,] cells)
        {
            for (var i = 0; i < cells.GetLength(0); i++)
            for (var j = 0; j < cells.GetLength(1); j++)
                cells[i, j].Position.GetComponent<MapCellTrigger>().Init(cells[i, j]); 
        }

        private void FillWithObstacles(MapCell[,] cells)
        {
            int placedObstacleCount = 0;
            while (placedObstacleCount < _settings.ObstacleCount)
            {
                MapCell mapCell = GetCellByIndex(GetRandomFreeCell(cells), cells);
                mapCell.MakeAnObstacle();
                placedObstacleCount++;
                SpawnObstacleObject(mapCell);
            }
        }

        private Vector3 GetOffset()
            => new()
            {
                x = _settings.PrimaryCell.transform.localScale.x,
                y = 0,
                z = _settings.PrimaryCell.transform.localScale.z
            };

        private Vector2 GetRandomFreeCell(MapCell[,] cells)
        {
            while (true)
            {
                Vector2 index = new()
                {
                    x = new Random().Next(0, cells.GetLength(0)),
                    y = new Random().Next(0, cells.GetLength(1))
                };
                
                if(IsObstacle(index, cells)) continue;
                if(IsSpawnPoint(index)) continue;

                return index;
            }
        }

        private void SpawnObstacleObject(MapCell mapCell)
        {
            var obstacle = Object.Instantiate(GetRandomObstacle(), mapCell.Position);
            obstacle.transform.position = mapCell.Position.position + Vector3.up / 2;
                
            GameObject GetRandomObstacle()
                => _settings.Obstacles[new Random().Next(0, _settings.Obstacles.Length)];
        }
        
        private bool IsObstacle(Vector2 index, MapCell[,] cells) 
            => GetCellByIndex(index, cells).IsObstacle;
        
        private bool IsSpawnPoint(Vector2 index)
            => (int)index.x == (int)_settings.PlayerSpawnCell.x
               && (int)index.y == (int)_settings.PlayerSpawnCell.y
               || (int)index.x == (int)_settings.EnemySpawnCell.x
               && (int)index.y == (int)_settings.EnemySpawnCell.y;

        private MapCell GetCellByIndex(Vector2 index, MapCell[,] cells)
            => cells[(int)index.x, (int)index.y];
    }
}