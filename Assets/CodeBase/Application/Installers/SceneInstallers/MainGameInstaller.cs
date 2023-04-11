using Cinemachine;
using CodeBase.GameCore;
using CodeBase.GameCore._AttackSystem;
using CodeBase.GameCore._CellSelector;
using CodeBase.GameCore.Map.Factory;
using CodeBase.GameCore.Mover;
using CodeBase.GameCore.Spawner;
using CodeBase.GameCore.TurnActions;
using CodeBase.UI;
using CodeBase.UI.Mediators;
using UnityEngine;
using Zenject;

namespace CodeBase.Application.Installers.SceneInstallers
{
    public sealed class MainGameInstaller : MonoInstaller
    {
        [Header("UI")] [SerializeField] private GameUIMediator _gameUIMediator;
        [SerializeField] private UIScreenFocus _screenFocus;
        [SerializeField] private TurnHeader _turnHeader;
        [SerializeField] private PlayerHealthUI _playerHealth;
        

        [Header("Map")] [SerializeField] private MapConfiguration _mapConfiguration;
        [SerializeField] private Transform _mapRootObject;
        [SerializeField] private PlayersSpawner.Settings _spawnerSettings;
        
        [Header("Camera")] 
        [SerializeField] private CinemachineVirtualCamera _cinemaCamera;
        
        [Header("GameRule")] 
        [SerializeField] private PlayerMover.Settings _moverSettings;
        [SerializeField] private MainGameLoop.Settings _gameLoopSettings;
        [SerializeField] private SelectCellTurnAction.Settings _selectCellSettings;
        [SerializeField] private AttackSystem.Settings _attackSettings;
        
        [Space] 
        [SerializeField] private CellSelector _cellSelector;


        public override void InstallBindings()
        {
            BindUI();
            BindMap();
            BindGameLoop();
            BindCamera();
            BindSpawner();
            BindMover();
            BindCellSelector();
            BindTurnActions();
            BindAttackSystem();
        }

        private void BindUI()
        {
            Container.Bind<GameUIMediator>().FromInstance(_gameUIMediator);
            Container.Bind<UIScreenFocus>().FromInstance(_screenFocus);
            Container.Bind<TurnHeader>().FromInstance(_turnHeader);
            Container.Bind<PlayerHealthUI>().FromInstance(_playerHealth);
        }

        private void BindMap()
            => Container.Bind<MapCreator>().AsSingle().WithArguments(_mapConfiguration, _mapRootObject);

        private void BindGameLoop()
            => Container.Bind<MainGameLoop>().AsSingle().WithArguments(_gameLoopSettings);

        private void BindCamera()
            => Container.Bind<CinemachineVirtualCamera>().FromInstance(_cinemaCamera);
        
        private void BindSpawner()
            => Container.Bind<PlayersSpawner>().AsSingle().WithArguments(_spawnerSettings);

        private void BindMover()
            => Container.Bind<PlayerMover>().AsSingle().WithArguments(_moverSettings);

        private void BindCellSelector()
            => Container.Bind<CellSelector>().FromInstance(_cellSelector);

        private void BindTurnActions()
        {
            Container.Bind<SelectCellTurnAction>().AsSingle().WithArguments(_selectCellSettings);
        }

        private void BindAttackSystem()
            => Container.Bind<AttackSystem>().AsSingle().WithArguments(_attackSettings);
    }
}