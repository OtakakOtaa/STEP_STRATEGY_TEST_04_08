using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CodeBase.Application
{
    public sealed class SceneSwitcher
    {
        private readonly Settings _settings;

        public SceneSwitcher(Settings settings)
            => _settings = settings;

        public async void ToMainMenu()
            => await SceneManager.LoadSceneAsync(_settings.MainMenuScene).ToUniTask();
        
        public async void ToMainGame()
            => await SceneManager.LoadSceneAsync(_settings.MainGameScene).ToUniTask();
        

        [Serializable] public sealed class Settings
        {
            [SerializeField] private string _mainMenuScene;
            [SerializeField] private string _mainGameScene;

            public string MainGameScene => _mainGameScene;
            public string MainMenuScene => _mainMenuScene;
        }
    }
}