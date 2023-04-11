using System;
using CodeBase.GameCore.Map.Data;
using UnityEngine;

namespace CodeBase.GameCore._CellSelector
{
    public sealed class CellSelector : MonoBehaviour
    {
        public event Action<MapCell> hasSelected;
        
        [HideInInspector] public bool ObserveFlag = false; 
        
        public void Update()
        {
#if UNITY_EDITOR
            if (ObserveFlag is false || !Input.GetMouseButtonDown(0) || !CastRayInEditor(out var hitE)) return;
            
            if (hitE.transform.TryGetComponent<MapCellTrigger>(out var componentE))
                hasSelected?.Invoke(componentE.MapCell);
            
#else
            if (ObserveFlag is false || Input.touchCount is 0 || !CastRay(out var hit)) return;

            if (hit.transform.TryGetComponent<MapCellTrigger>(out var component))
                hasSelected?.Invoke(component.MapCell);
#endif
        }

        private bool CastRay(out RaycastHit hit)
        {
            var ray = Camera.main!.ScreenPointToRay(Input.GetTouch(0).position);
            return Physics.Raycast(ray, out hit, Mathf.Infinity);
        }
        
        private bool CastRayInEditor(out RaycastHit hit)
        {
            var ray = Camera.main!.ScreenPointToRay(Input.mousePosition);
            return Physics.Raycast(ray, out hit, Mathf.Infinity);
        }
    }
}