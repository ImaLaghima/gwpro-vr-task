using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTask.GasAnalyzer.DangerZone;
using VRTask.GasAnalyzer.Probe;

namespace VRTask.GasAnalyzer.Controller
{
    [DisallowMultipleComponent]
    public class GasAnalyzerController : MonoBehaviour
    {
        [Header("Gas Analyzer Controller")]
        [SerializeField]
        private bool _isLogging = true;

        [SerializeField]
        private ProbeBehavior _probeBehavior = null!;


        public bool IsLogging => _isLogging;


        private void Awake()
        {
            AssertInspectorRefsNotNull();
        }


        private void AssertInspectorRefsNotNull()
        {
            Debug.Assert(
                _probeBehavior != null,
                "[GasAnalyzerController] Probe Behavior reference is missing!"
            );
        }

        private void Log(string message)
        {
            if (IsLogging)
            {
                Debug.Log($"[GasAnalyzerController] {message}");
            }
        }


        private IEnumerator UpdateCoroutine()
        {
            while (true)
            {
                IReadOnlyDictionary<DangerZoneIdentity, float> dangerZones
                    = _probeBehavior.GetDangerZones();

                KeyValuePair<DangerZoneIdentity, float>? nearestDangerZone = null;
                foreach (KeyValuePair<DangerZoneIdentity, float> kvp in dangerZones)
                {
                    if (!nearestDangerZone.HasValue)
                    {
                        nearestDangerZone = kvp;
                        continue;
                    }
                    if (kvp.Value < nearestDangerZone.Value.Value)
                    {
                        nearestDangerZone = kvp;
                    }
                }

                if (nearestDangerZone.HasValue)
                {
                    // refresh display
                }

                yield return null;
            }
        }
    }
}
