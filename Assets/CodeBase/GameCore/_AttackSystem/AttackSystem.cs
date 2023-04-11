using System;
using UnityEngine;
using Random = System.Random;

namespace CodeBase.GameCore._AttackSystem
{
    public sealed class AttackSystem
    {
        private readonly Settings _settings;

        public AttackSystem(Settings settings)
            => _settings = settings;

        public void TryAttack(Player attacker, Player target)
        {
            if (CanMakeDamage(attacker, attacker)) 
                target.TakeDamage(RandomDamageAmount);
        }

        private bool CanMakeDamage(Player attacker, Player target)
        {
            var position = attacker.Root.position;
            var direction = target.Root.position - position;
            Physics.Raycast(position, direction,  out var hit, Mathf.Infinity);

            var isBulletHitInObstacle = hit.transform != target.Root;
            return isBulletHitInObstacle is false;
        }
        
        private int RandomDamageAmount
            => new Random().Next(_settings.MinDamage, _settings.MaxDamage);

        [Serializable] public sealed class Settings
        {
            [SerializeField, Range(0,15)] private int _minDamage = 15;
            [SerializeField, Range(15,25)] private int _maxDamage = 25;

            public int MinDamage => _minDamage;
            public int MaxDamage => _maxDamage;
        }
    }
}