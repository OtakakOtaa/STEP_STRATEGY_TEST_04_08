using System;
using Cinemachine;
using CodeBase.GameCore._AttackSystem;
using CodeBase.GameCore.Map.Data;
using CodeBase.GameCore.Map.Factory;
using CodeBase.GameCore.Spawner;
using CodeBase.GameCore.TurnActions;
using CodeBase.UI.Mediators;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CodeBase.GameCore
{
    public sealed class MainGameLoop : IDisposable
    {
        private readonly Settings _settings;

        private readonly GameUIMediator _gameUIMediator;
        
        private readonly MapCreator _mapCreator;
        private readonly PlayersSpawner _playersSpawner;
        private readonly CinemachineVirtualCamera _cinemaCamera;
        private readonly SelectCellTurnAction _selectCellTurnAction;
        private readonly AttackSystem _attackSystem;
        
        private GameMap _gameMap;
        private Player _player;
        private Player _enemy;

        private bool _wasSelectedAttack;

        public MainGameLoop(Settings settings, MapCreator mapCreator, AttackSystem attackSystem, 
            PlayersSpawner playersSpawner, GameUIMediator gameUIMediator,
            CinemachineVirtualCamera camera, SelectCellTurnAction selectCellTurnAction)
        {
            _settings = settings;
            _gameUIMediator = gameUIMediator;
            _attackSystem = attackSystem;
            _selectCellTurnAction = selectCellTurnAction;
            _mapCreator = mapCreator;
            _cinemaCamera = camera;
            _playersSpawner = playersSpawner;

            gameUIMediator.StartAttack += ChooseAttack;
        }

        public void EnterLoop()
        {
            InitGame();
            StartLoop();
        }

        public void ChooseAttack()
            => _wasSelectedAttack = true;
        
        private async UniTaskVoid StartLoop()
        {
            while (true)
            {
                await StartPlayerTurn();
                await StartControlledEnemyTurn();
            }
        }

        private async UniTask StartPlayerTurn()
        {
            _gameUIMediator.ChangePlayerHealth(_player.Health);
            _gameUIMediator.StartPlayerTurn();
            _selectCellTurnAction.InjectPlayer(_player);
            ClampCamera(_player.Root);
            for (var i = 0; i < _settings.TurnActionsCount; i++)
                await MakeTurnAction(_player, _enemy);
        }
        private async UniTask StartControlledEnemyTurn()
        {
            _gameUIMediator.ChangePlayerHealth(_enemy.Health);
            _gameUIMediator.StartEnemyTurn();
            _selectCellTurnAction.InjectPlayer(_enemy);
            ClampCamera(_enemy.Root);
            for (var i = 0; i < _settings.TurnActionsCount; i++)
                await MakeTurnAction(_enemy, _player);
        }
        
        private async UniTask MakeTurnAction(Player player, Player enemy)
        {
            var moveAction = _selectCellTurnAction.EnterAction();
            var attackAction = UniTask.WaitUntil(() => _wasSelectedAttack);
            await UniTask.WhenAny(moveAction, attackAction);
            if(_wasSelectedAttack) _attackSystem.TryAttack(_player, _enemy);
            _wasSelectedAttack = false;
        }
        
        private void ClampCamera(Transform target)
        {
            _cinemaCamera.Follow = target;
            _cinemaCamera.LookAt = target;
        }
        
        public void Dispose()
        {
            _gameUIMediator.StartAttack -= ChooseAttack;
        }
        
        private void InitGame()
        {
            _gameMap = _mapCreator.Create();
            _playersSpawner.Spawn(_gameMap, out _player, out _enemy);
            _selectCellTurnAction.InjectGameMap(_gameMap);
        }

        [Serializable] public sealed class Settings
        {
            [SerializeField] private int _turnActionsCount = 2;
            
            public int TurnActionsCount => _turnActionsCount;
        }
    }
}