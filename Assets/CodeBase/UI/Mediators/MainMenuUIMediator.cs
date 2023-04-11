using CodeBase._Localization;
using CodeBase.Application;
using UnityEngine;
using Zenject;

namespace CodeBase.UI.Mediators
{
    public sealed class MainMenuUIMediator : MonoBehaviour
    {
        [SerializeField] private GameSettingsObject _gameSettings;

        [Inject] private ApplicationGate _applicationGate;
        [Inject] private SceneSwitcher _sceneSwitcher;
        [Inject] private Localization _localization;


        public void StartGame()
            => _sceneSwitcher.ToMainGame();

        public void ExitGame()
            => _applicationGate.Exit();

        public void OpenGameSettings()
            => _gameSettings.Open();
        
        public void CloseGameSettings()
            => _gameSettings.Close();

        public void ChangeToEnLocal()
            => _localization.ChangeLocale(Localization.LocalizationKey.En);

        public void ChangeToRuLocal()
            => _localization.ChangeLocale(Localization.LocalizationKey.Ru);
        
    }
}