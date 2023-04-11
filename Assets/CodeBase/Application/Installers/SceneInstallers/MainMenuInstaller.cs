using CodeBase.UI;
using CodeBase.UI.Mediators;
using UnityEngine;
using Zenject;

namespace CodeBase.Application.Installers.SceneInstallers
{
    public class MainMenuInstaller : MonoInstaller
    {
        [SerializeField] private MainMenuUIMediator _mainMenu;
        [SerializeField] private UIScreenFocus _screenFocus;

        public override void InstallBindings()
        {
            BindUI();
        }

        private void BindUI()
        {
            Container.Bind<MainMenuUIMediator>().FromInstance(_mainMenu);
            Container.Bind<UIScreenFocus>().FromInstance(_screenFocus);
        }
    }
}