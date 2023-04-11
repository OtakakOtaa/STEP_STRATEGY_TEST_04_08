using System;
using System.Collections.Generic;
using System.Linq;
using CodeBase.GameCore.Map.Data;
using UnityEngine;

namespace CodeBase.GameCore.Map._PathFinder
{
    public sealed class PathFinder
    {
        private readonly GameMap _gameMap;
        
        private readonly List<PathNode> _checkedNodes = new();
        private readonly List<PathNode> _waitingPool = new();

        public PathFinder(GameMap gameMap)
            => _gameMap = gameMap;

        public IEnumerable<Vector2> FindPath(Vector2 start, Vector2 target)
        {
            _checkedNodes.Clear();
            _waitingPool.Clear();
            
            PathNode startNode = new(start, target, previousNode: null, 0);
            _checkedNodes.Add(startNode);
            _waitingPool.AddRange(GetNearbyNodes(startNode, target));

            while (_waitingPool.Count is not 0)
            {
                var nodeToCheck = _waitingPool.FirstOrDefault(n => 
                    n.Heft == _waitingPool.Min(pn => pn.Heft));
                if (nodeToCheck is null) throw new Exception();

                var isEnd = (int)nodeToCheck.Position.x == (int)target.x
                            && (int)nodeToCheck.Position.y == (int)target.y;

                if (isEnd) return CalculatePath(nodeToCheck);
                var isObstacle = _gameMap.GetCellByIndex(nodeToCheck.Position).IsObstacle;

                _waitingPool.Remove(nodeToCheck);
                if (isObstacle)
                {
                    _checkedNodes.Add(nodeToCheck);
                    continue;
                }

                if (_checkedNodes.Any(n => n.Position == nodeToCheck.Position) is false)
                {
                    _checkedNodes.Add(nodeToCheck);
                    _waitingPool.AddRange(GetNearbyNodes(nodeToCheck, target));
                }
            }
            throw new Exception();
        }
        
        private IEnumerable<PathNode> GetNearbyNodes(PathNode origin, Vector2 target)
        {
            List<PathNode> nodes = new()
            {
                new PathNode(origin.Position + Vector2.up, target, origin, origin.Remoteness + 1),
                new PathNode(origin.Position + Vector2.down, target, origin, origin.Remoteness + 1),
                new PathNode(origin.Position + Vector2.left, target, origin, origin.Remoteness + 1),
                new PathNode(origin.Position + Vector2.right, target, origin, origin.Remoteness + 1),
            };

            List<PathNode> removeList = new (); 
            foreach (var node in nodes)
            {
                var isIncorrectCellIndex = node.Position.x >= _gameMap.MapScale.x || node.Position.x < 0
                    || node.Position.y >= _gameMap.MapScale.y || node.Position.y < 0;
                if (isIncorrectCellIndex) 
                    removeList.Add(node);
            }
            removeList.ForEach(n => nodes.Remove(n));
            return nodes;
        }

        private IEnumerable<Vector2> CalculatePath(PathNode root, Stack<PathNode> path = default)
        {
            path ??= new Stack<PathNode>();
            path.Push(root);
            return root.PreviousNode is not null ? 
                CalculatePath(root.PreviousNode, path) 
                : path.Select(n => n.Position);
        }

        private sealed class PathNode
        {
            public Vector2 Position { get; }
            public PathNode PreviousNode { get; }
            public int Heft { get; }
            public int Remoteness { get; }

            public PathNode(Vector2 position, Vector2 target, PathNode previousNode, int remoteness)
            {
                Position = position;
                PreviousNode = previousNode;
                Remoteness = remoteness;
                Heft = remoteness * (int)Math.Abs(target.x - position.x) + (int)Math.Abs(target.y - position.y);
            }
        }
    }
}