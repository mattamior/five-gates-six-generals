using System;
using UnityEngine;

namespace SanguoLegend.Combat
{
    /// <summary>Health container shared by player, soldiers and generals.</summary>
    public sealed class Damageable : MonoBehaviour
    {
        public enum Team
        {
            Player,
            Enemy,
            Neutral
        }

        [SerializeField] private Team faction = Team.Enemy;
        [SerializeField] private float maxHealth = 100f;

        public Team Faction => faction;
        public float MaxHealth => maxHealth;
        public float Health { get; private set; }
        public bool IsDead => Health <= 0f;

        public event Action<Damageable, float> Damaged;
        public event Action<Damageable> Died;

        private void Awake() => Health = maxHealth;

        /// <summary>Used by editor tooling and spawners to configure a freshly created actor.</summary>
        public void Configure(Team team, float health)
        {
            faction = team;
            maxHealth = Mathf.Max(1f, health);
            Health = maxHealth;
        }

        public void TakeDamage(float amount, GameObject source = null)
        {
            if (IsDead || amount <= 0f) return;

            Health = Mathf.Max(0f, Health - amount);
            Damaged?.Invoke(this, amount);

            if (Health <= 0f) Died?.Invoke(this);
        }
    }
}
