using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using Zenject;

namespace CodeBase.UI
{
    public sealed class TurnHeader : MonoBehaviour
    {
        [SerializeField] private LocalizeStringEvent _localizeStringEvent;
        [SerializeField] private LocalizedString _playerHeader;
        [SerializeField] private LocalizedString _enemyHeader;

        [Inject] private UIScreenFocus _focus;
        
        private void Start()
            => gameObject.SetActive(false);

        public void ShowPlayerTurn()
        {
            gameObject.SetActive(true);
            _localizeStringEvent.StringReference = _playerHeader;
            _focus.FocusOn((RectTransform)transform);
        }

        public void ShowEnemyTurn()
        {
            gameObject.SetActive(true);
            _localizeStringEvent.StringReference = _enemyHeader;
            _focus.FocusOn((RectTransform)transform);
        }

        public void Disable()
            => gameObject.SetActive(false);
    }
}