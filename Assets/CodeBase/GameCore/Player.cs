using System;
using UnityEngine;

namespace CodeBase.GameCore
{
    public sealed class Player
    {
        public event Action _playerDied;
        
        private readonly Transform _root;
        private int _health;

        public Player(Transform root, int health = 100)
        {
            _health = health;
            _root = root;
        }

        public Transform Root => _root;
        public int Health => _health;

        public void TakeDamage(int damage)
        {
            _health = Mathf.Clamp(_health - damage, 0, 100);
            if(_health is 0) _playerDied?.Invoke();
        }
    }
}