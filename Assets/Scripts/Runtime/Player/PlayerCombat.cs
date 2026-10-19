using SanguoLegend.Combat;
using UnityEngine;

namespace SanguoLegend.Player
{
    /// <summary>Graybox combat: two attack types, block flag, sphere overlap hit detection.</summary>
    public sealed class PlayerCombat : MonoBehaviour
    {
        [Header("Light attack")]
        [SerializeField] private float lightDamage = 14f;
        [SerializeField] private float lightRange = 2.6f;
        [SerializeField] private float lightCooldown = 0.42f;

        [Header("Heavy attack")]
        [SerializeField] private float heavyDamage = 34f;
        [SerializeField] private float heavyRange = 3.0f;
        [SerializeField] private float heavyCooldown = 0.95f;

        [Header("Feel")]
        [SerializeField] private float hitStopSeconds = 0.05f;
        [SerializeField] private LayerMask hitMask = ~0;

        private float nextLightTime;
        private float nextHeavyTime;

        public bool IsBlocking { get; private set; }

        private void Update()
        {
            IsBlocking = Input.GetMouseButton(1);

            if (Input.GetMouseButtonDown(0) && Time.time >= nextLightTime)
            {
                nextLightTime = Time.time + lightCooldown;
                Strike(lightDamage, lightRange);
            }

            if (Input.GetKeyDown(KeyCode.Q) && Time.time >= nextHeavyTime)
            {
                nextHeavyTime = Time.time + heavyCooldown;
                Strike(heavyDamage, heavyRange);
            }
        }

        private void Strike(float damage, float range)
        {
            var origin = transform.position + Vector3.up + transform.forward * (range * 0.45f);
            var hits = Physics.OverlapSphere(origin, range * 0.55f, hitMask, QueryTriggerInteraction.Ignore);
            var connected = false;

            foreach (var hit in hits)
            {
                if (hit.transform.root == transform.root) continue;

                var target = hit.GetComponentInParent<Damageable>();
                if (target == null || target.Faction != Damageable.Team.Enemy || target.IsDead) continue;

                target.TakeDamage(damage, gameObject);
                connected = true;
            }

            if (connected) HitStop.Play(hitStopSeconds);
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            var origin = transform.position + Vector3.up + transform.forward * (lightRange * 0.45f);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(origin, lightRange * 0.55f);
        }
#endif
    }
}
