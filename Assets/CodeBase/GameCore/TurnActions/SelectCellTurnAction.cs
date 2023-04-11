using System;
using CodeBase.GameCore._CellSelector;
using CodeBase.GameCore.Map._PathFinder;
using CodeBase.GameCore.Map.Data;
using CodeBase.GameCore.Mover;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CodeBase.GameCore.TurnActions
{
    public sealed class SelectCellTurnAction
    {
        private readonly Settings _settings;

        private readonly CellSelector _cellSelector;
        private readonly PlayerMover _playerMover;

        private GameMap _gameMap;
        private Player _player;
        private MapCell _selectedCell;

        public SelectCellTurnAction(Settings settings, CellSelector cellSelector, PlayerMover playerMover)
        {
            _settings = settings;
            _playerMover = playerMover;
            _cellSelector = cellSelector;
            _cellSelector.hasSelected += OnSelected;
        }

        public void InjectGameMap(GameMap gameMap)
        {
            _gameMap = gameMap;
            _playerMover.Init(new PathFinder(gameMap), gameMap);
        }

        public void InjectPlayer(Player player)
            => _player = player;

        public async UniTask EnterAction()
        {
            _cellSelector.ObserveFlag = true;
            await UniTask.WaitUntil(() => _selectedCell is not null);
            await _playerMover.MoveTo(_player.Root, _selectedCell.Position);
            Dispose();
        }

        private void OnSelected(MapCell mapCell)
        {
            if (mapCell.IsObstacle) return;

            var player = _gameMap.TryGetIndexByPosition(_player.Root.position);
            var targetCell = _gameMap.TryGetIndexByPosition(mapCell.Position.position);
            if (player is null || targetCell is null) throw new Exception();
            
            var isCellLocatedInRadius =
                (int)Math.Abs(targetCell.Value.x - player.Value.x)
                + (int)Math.Abs(targetCell.Value.y - player.Value.y)
                <= _settings.MaxMoveDistance;
            
            if(isCellLocatedInRadius is false) return;
            
            _selectedCell = mapCell;
            _cellSelector.ObserveFlag = false;
        }

        private void Dispose()
        {
            _cellSelector.ObserveFlag = false;
            _selectedCell = null;
        }

        [Serializable]
        public sealed class Settings
        {
            [SerializeField] private int _maxMoveDistance = 8;
            public int MaxMoveDistance => _maxMoveDistance;
        }
    }
}