using UnityEngine;

namespace CodeBase.UI
{
    public class UIScreenFocus : MonoBehaviour
    {
        private Transform _cachedParent;
        private Transform _target;
        
        private void Awake()
            => gameObject.SetActive(false);

        public void FocusOn(RectTransform @object)
        {
            _target = @object;
            _cachedParent = (RectTransform)_target.parent;

            _target.SetParent(transform);
            gameObject.SetActive(true);
        }

        public void Disable()
        {
            _target.SetParent(_cachedParent);
            gameObject.SetActive(false);    
        }
        
    }
}