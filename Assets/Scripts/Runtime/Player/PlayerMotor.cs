using UnityEngine;

namespace SanguoLegend.Player
{
    /// <summary>Camera-relative locomotion on a CharacterController. Mounting is a speed/visual state only for now.</summary>
    [RequireComponent(typeof(CharacterController))]
    public sealed class PlayerMotor : MonoBehaviour
    {
        [Header("Speed")]
        [SerializeField] private float walkSpeed = 3.2f;
        [SerializeField] private float runSpeed = 6.4f;
        [SerializeField] private float mountedMultiplier = 1.9f;
        [SerializeField] private float acceleration = 18f;
        [SerializeField] private float turnSpeed = 14f;

        [Header("Gravity")]
        [SerializeField] private float gravity = -25f;

        private CharacterController controller;
        private Transform cameraTransform;
        private Vector3 planarVelocity;
        private float verticalVelocity;

        public bool IsMounted { get; private set; }
        public float PlanarSpeed => new Vector2(planarVelocity.x, planarVelocity.z).magnitude;
        public float NormalizedSpeed => Mathf.Clamp01(PlanarSpeed / (runSpeed * (IsMounted ? mountedMultiplier : 1f)));

        private void Awake() => controller = GetComponent<CharacterController>();

        private void Start()
        {
            if (Camera.main != null) cameraTransform = Camera.main.transform;
        }

        public void SetMounted(bool mounted) => IsMounted = mounted;

        private void Update()
        {
            var input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            input = Vector2.ClampMagnitude(input, 1f);

            var forward = cameraTransform
                ? Vector3.ProjectOnPlane(cameraTransform.forward, Vector3.up).normalized
                : Vector3.forward;
            if (forward.sqrMagnitude < 0.001f) forward = Vector3.forward;

            var right = Vector3.Cross(Vector3.up, forward);
            var moveDirection = forward * input.y + right * input.x;

            var speed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;
            if (IsMounted) speed *= mountedMultiplier;

            planarVelocity = Vector3.MoveTowards(planarVelocity, moveDirection * speed, acceleration * Time.deltaTime);

            if (controller.isGrounded && verticalVelocity < 0f) verticalVelocity = -2f;
            verticalVelocity += gravity * Time.deltaTime;

            controller.Move((planarVelocity + Vector3.up * verticalVelocity) * Time.deltaTime);

            if (moveDirection.sqrMagnitude > 0.01f)
            {
                var look = Quaternion.LookRotation(moveDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, look, turnSpeed * Time.deltaTime);
            }
        }
    }
}
