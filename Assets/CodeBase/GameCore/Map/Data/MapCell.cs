using UnityEngine;

namespace CodeBase.GameCore.Map.Data
{
    public sealed class MapCell
    {
        public bool IsObstacle { get; private set; }
        public Transform Position { get; }
        
        public MapCell(Transform position, bool isObstacle = default)
        {
            Position = position;
            IsObstacle = isObstacle;
        }
        
        public void MakeAnObstacle()
            => IsObstacle = true;
    }
}