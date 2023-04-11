using CodeBase._Localization;
using Zenject;

namespace CodeBase.Application.Installers
{
    public sealed class ApplicationSystemsInstaller 
        : Installer<SceneSwitcher.Settings, ApplicationSystemsInstaller>
    {
        private readonly SceneSwitcher.Settings _scenesSettings;

        public ApplicationSystemsInstaller(SceneSwitcher.Settings scenesSettings)
            => _scenesSettings = scenesSettings;
        
        public override void InstallBindings()
        {
            BindGate();
            BindSceneSwitcher();
            BindLocalization();
        }

        private void BindGate()
            => Container.Bind<ApplicationGate>().AsSingle();

        private void BindSceneSwitcher()
            => Container.Bind<SceneSwitcher>().AsSingle().WithArguments(_scenesSettings);

        private void BindLocalization()
            => Container.Bind<Localization>().AsSingle();
    }
}