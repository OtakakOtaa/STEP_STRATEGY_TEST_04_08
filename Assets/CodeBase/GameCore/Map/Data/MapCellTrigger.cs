using UnityEngine;

namespace CodeBase.GameCore.Map.Data
{
    [RequireComponent(typeof(Collider))]
    public class MapCellTrigger : MonoBehaviour
    {
        private MapCell _mapCell;

        public void Init(MapCell mapCell)
            => _mapCell = mapCell;
        
        public MapCell MapCell => _mapCell;
    }
}