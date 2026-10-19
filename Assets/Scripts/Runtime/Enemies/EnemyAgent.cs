using SanguoLegend.Combat;
using UnityEngine;

namespace SanguoLegend.Enemies
{
    /// <summary>Graybox melee brain: approach the player and attack on cooldown. No navigation mesh yet.</summary>
    [RequireComponent(typeof(Damageable))]
    public sealed class EnemyAgent : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 2.1f;
        [SerializeField] private float attackRange = 2f;
        [SerializeField] private float attackDamage = 8f;
        [SerializeField] private float attackCooldown = 1.6f;
        [SerializeField] private float aggroRange = 20f;
        [SerializeField] private float turnSpeed = 8f;

        private Damageable self;
        private Damageable targetDamageable;
        private Transform target;
        private float nextAttackTime;
        private Vector3 spawnPosition;

        private void Awake()
        {
            self = GetComponent<Damageable>();
            spawnPosition = transform.position;
        }

        private void Start()
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                target = player.transform;
                targetDamageable = player.GetComponentInParent<Damageable>();
            }

            self.Died += OnDied;
        }

        private void OnDestroy()
        {
            if (self != null) self.Died -= OnDied;
        }

        private void OnDied(Damageable damageable)
        {
            foreach (var hitbox in GetComponentsInChildren<Collider>()) hitbox.enabled = false;
            transform.position = spawnPosition;
            enabled = false;
        }

        private void Update()
        {
            if (self.IsDead || target == null) return;

            var toTarget = target.position - transform.position;
            toTarget.y = 0f;
            var distance = toTarget.magnitude;
            if (distance > aggroRange) return;

            if (distance > attackRange)
            {
                var step = Mathf.Min(moveSpeed * Time.deltaTime, distance - attackRange * 0.9f);
                transform.position += toTarget.normalized * step;

                if (toTarget.sqrMagnitude > 0.01f)
                {
                    var look = Quaternion.LookRotation(toTarget);
                    transform.rotation = Quaternion.Slerp(transform.rotation, look, turnSpeed * Time.deltaTime);
                }

                return;
            }

            if (Time.time < nextAttackTime) return;

            nextAttackTime = Time.time + attackCooldown;
            if (targetDamageable != null && !targetDamageable.IsDead)
                targetDamageable.TakeDamage(attackDamage, gameObject);
        }
    }
}
