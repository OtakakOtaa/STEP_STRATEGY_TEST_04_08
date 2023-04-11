using UnityEngine;
using Zenject;

namespace CodeBase.Application.Installers
{
    public sealed class ProjectInstaller : MonoInstaller
    {
        [SerializeField] private SceneSwitcher.Settings _sceneConfig;
        
        public override void InstallBindings()
        {
            ApplicationSystemsInstaller.Install(Container, _sceneConfig);
        }
    }
}