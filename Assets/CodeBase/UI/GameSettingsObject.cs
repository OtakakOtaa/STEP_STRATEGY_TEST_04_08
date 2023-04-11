using UnityEngine;
using Zenject;

namespace CodeBase.UI
{
    [RequireComponent(typeof(RectTransform))]
    public sealed class GameSettingsObject : MonoBehaviour
    {
        [Inject] private UIScreenFocus _screenFocus;
        
        private void Awake()
            => gameObject.SetActive(false);

        public void Open()
        {
            gameObject.SetActive(true);
            _screenFocus.FocusOn(transform as RectTransform);
        }

        public void Close()
        {
            gameObject.SetActive(false);
            _screenFocus.Disable();
        }
    }
}