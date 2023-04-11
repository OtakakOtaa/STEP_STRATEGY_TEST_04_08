using System;
using System.Linq;
using System.Threading.Tasks;
using CodeBase.GameCore.Map._PathFinder;
using CodeBase.GameCore.Map.Data;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace CodeBase.GameCore.Mover
{
    public class PlayerMover
    {
        private readonly Settings _settings;

        private PathFinder _pathFinder;
        private GameMap _gameMap;

        public PlayerMover(Settings settings)
        {
            _settings = settings;
        }

        public void Init(PathFinder pathFinder, GameMap gameMap)
        {
            _pathFinder = pathFinder;
            _gameMap = gameMap;
        }

        public async UniTask MoveTo(Transform target, Transform destination)
        {
            var startIndex = _gameMap.TryGetIndexByPosition(target.position);
            var endIndex = _gameMap.TryGetIndexByPosition(destination.position);
            if (startIndex is null || endIndex is null)
                throw new Exception();

            var path = _pathFinder.FindPath((Vector2)startIndex, (Vector2)endIndex);
            int iterator = 0;
            var position = target.position;
            TaskCompletionSource<bool> locker = new ();
            MoveNext();

            await locker.Task;
            
            void MoveNext()
            {
                target
                    .DOMove(
                        TranslateToPosition(path.ElementAt(iterator), position),
                        _settings.TimeForOneStep)
                    .OnComplete(() =>
                    {
                        iterator++;
                        if (iterator == path.Count())
                        {
                            locker.SetResult(true);
                            return;
                        }
                        MoveNext();
                    });
            }
        }

        private Vector3 TranslateToPosition(Vector2 dot, Vector3 target)
        {
            var cellPosition = _gameMap.GetCellByIndex(dot).Position.position;
            return new()
            {
                x = cellPosition.x,
                y = target.y,
                z = cellPosition.z
            };
        }

        [Serializable]
        public sealed class Settings
        {
            [SerializeField, Range(0, 1)] private float _timeForOneStep;

            public float TimeForOneStep => _timeForOneStep;
        }
    }
}