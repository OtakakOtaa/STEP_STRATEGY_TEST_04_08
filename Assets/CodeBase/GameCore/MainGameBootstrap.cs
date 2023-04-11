using UnityEngine;
using Zenject;

namespace CodeBase.GameCore
{
    public class MainGameBootstrap : MonoBehaviour
    {
        [Inject] private MainGameLoop _mainGameLoop;
        
        private void Start()
            => _mainGameLoop.EnterLoop();
    }
}