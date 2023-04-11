using System;
using CodeBase.GameCore;
using TMPro;
using UnityEngine;
using Zenject;

namespace CodeBase.UI.Mediators
{
    public sealed class GameUIMediator : MonoBehaviour
    {
        [Inject] private PlayerHealthUI _playerHealth;
        [Inject] private TurnHeader _turnHeader;
        [Inject] private UIScreenFocus _focus;

        public event Action StartAttack;
        
        public void StartEnemyTurn()
            => _turnHeader.ShowEnemyTurn();

        public void StartPlayerTurn()
            => _turnHeader.ShowPlayerTurn();

        public void ChangePlayerHealth(int amount)
            => _playerHealth.ChangeHeath(amount);

        public void AttackEnemy()
            => StartAttack?.Invoke();

        public void DisableFocus()
            => _focus.Disable();
    }
}