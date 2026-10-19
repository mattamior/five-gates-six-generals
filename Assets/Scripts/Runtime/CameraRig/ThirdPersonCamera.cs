using UnityEngine;

namespace SanguoLegend.CameraRig
{
    /// <summary>Orbit camera with collision. Cinemachine takes over once duel cameras are authored.</summary>
    public sealed class ThirdPersonCamera : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private Vector3 pivotOffset = new Vector3(0f, 1.65f, 0f);
        [SerializeField] private float distance = 4.4f;
        [SerializeField] private float sensitivity = 3f;
        [SerializeField] private float minPitch = -15f;
        [SerializeField] private float maxPitch = 60f;
        [SerializeField] private float followSharpness = 14f;
        [SerializeField] private LayerMask collisionMask = ~0;

        private float yaw;
        private float pitch = 12f;

        public void SetTarget(Transform value) => target = value;

        private void Start()
        {
            if (target == null)
            {
                var player = GameObject.FindGameObjectWithTag("Player");
                if (player != null) target = player.transform;
            }

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void LateUpdate()
        {
            if (target == null) return;

            yaw += Input.GetAxis("Mouse X") * sensitivity;
            pitch = Mathf.Clamp(pitch - Input.GetAxis("Mouse Y") * sensitivity, minPitch, maxPitch);

            var rotation = Quaternion.Euler(pitch, yaw, 0f);
            var pivot = target.position + pivotOffset;
            var desired = pivot - rotation * Vector3.forward * distance;

            if (Physics.Linecast(pivot, desired, out var hit, collisionMask, QueryTriggerInteraction.Ignore))
                desired = hit.point + hit.normal * 0.25f;

            var blend = 1f - Mathf.Exp(-followSharpness * Time.deltaTime);
            transform.position = Vector3.Lerp(transform.position, desired, blend);
            transform.rotation = Quaternion.LookRotation(pivot - transform.position);
        }
    }
}
