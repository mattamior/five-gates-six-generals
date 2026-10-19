using UnityEngine;

namespace SanguoLegend.Player
{
    /// <summary>Placeholder mount toggle. Visual is a primitive until the 赤兔 asset lands.</summary>
    public sealed class MountController : MonoBehaviour
    {
        [SerializeField] private GameObject mountVisual;

        private PlayerMotor motor;

        private void Awake() => motor = GetComponent<PlayerMotor>();

        private void Start()
        {
            if (mountVisual != null) return;

            mountVisual = GameObject.CreatePrimitive(PrimitiveType.Cube);
            mountVisual.name = "HorsePlaceholder";
            mountVisual.transform.SetParent(transform, false);
            mountVisual.transform.localPosition = new Vector3(0f, 0.55f, 0.35f);
            mountVisual.transform.localScale = new Vector3(0.55f, 1.1f, 1.9f);
            Destroy(mountVisual.GetComponent<Collider>());
            mountVisual.SetActive(false);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F)) ToggleMount();
        }

        public void ToggleMount()
        {
            motor.SetMounted(!motor.IsMounted);
            if (mountVisual != null) mountVisual.SetActive(motor.IsMounted);
        }
    }
}
