using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using VRTask.GasAnalyzer.DangerZone;

namespace VRTask.GasAnalyzer.Probe
{
    public class ProbeBehavior : MonoBehaviour
    {
        [Header("Probe Behavior")]
        [SerializeField]
        private bool _isLogging = true;

        [SerializeField]
        private Collider _detectionCollider = null!;

        [SerializeField]
        private UnityEvent<DangerZoneIdentity> _onDangerZoneEnter = new();

        [SerializeField]
        private UnityEvent<DangerZoneIdentity> _onDangerZoneExit = new();

        private readonly Dictionary<DangerZoneIdentity, float> _dangerZones = new();


        public bool IsLogging => _isLogging;


        public UnityEvent<DangerZoneIdentity> OnDangerZoneEnter => _onDangerZoneEnter;
        public UnityEvent<DangerZoneIdentity> OnDangerZoneExit => _onDangerZoneExit;


        private void Awake()
        {
            AssertInspectorRefsNotNull();
        }

        private void Update()
        {
            // TODO: change calculation method to the ray casting
            foreach (DangerZoneIdentity key in _dangerZones.Keys.ToList())
            {
                float renewedDistance = Vector3.Distance(
                    transform.position,
                    key.transform.position
                );
                _dangerZones[key] = renewedDistance;
            }
        }


        public IReadOnlyDictionary<DangerZoneIdentity, float> GetDangerZones()
        {
            return new Dictionary<DangerZoneIdentity, float>(_dangerZones);
        }


        private void AssertInspectorRefsNotNull()
        {
            Debug.Assert(
                _detectionCollider != null,
                "[ProbeBehavior] Detection Collider reference is missing!"
            );
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("DangerZone"))
            {
                return;
            }

            if (
                other.TryGetComponent(out DangerZoneIdentity identity)
                && _dangerZones.TryAdd(identity, 0)
            )
            {
                _onDangerZoneEnter.Invoke(identity);
                Log(nameof(OnDangerZoneEnter));
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag("DangerZone"))
            {
                return;
            }

            if (
                other.TryGetComponent(out DangerZoneIdentity leaving)
                && _dangerZones.Remove(leaving)
            )
            {
                OnDangerZoneExit.Invoke(leaving);
                Log(nameof(OnDangerZoneExit));
            }
        }

        private void Log(string message)
        {
            if (IsLogging)
            {
                Debug.Log($"[ProbeBehavior] {message}");
            }
        }
    }
}
