using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTask.Crane.InputProvider;
using VRTask.GasAnalyzer.Common;
using VRTask.GasAnalyzer.DangerZone;
using VRTask.GasAnalyzer.Display;
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

        [SerializeField]
        private GasAnalyzerDisplay _display = null!;

        [SerializeField]
        private PowerButton _powerButton = null!;

        [SerializeField]
        private float _powerDelaySeconds = 3.0f;

        private Coroutine? _powerCoroutine;
        private Coroutine? _updateCoroutine;


        public bool IsLogging => _isLogging;
        public GasAnalyzerState State { get; private set; }
            = GasAnalyzerState.Inactive;


        private void Awake()
        {
            AssertInspectorRefsNotNull();
        }

        private void OnEnable()
        {
            _powerButton.OnPressed += PowerOn;
            _powerButton.OnReleased += PowerOff;
        }

        private void OnDisable()
        {
            _powerButton.OnPressed -= PowerOn;
            _powerButton.OnReleased -= PowerOff;
        }



        public void PowerOn()
        {
            if (_powerCoroutine == null)
            {
                Log(nameof(PowerOn) + " started");
                _powerCoroutine = StartCoroutine(PowerCoroutine());
            }
        }

        public void PowerOff()
        {
            if (_powerCoroutine != null)
            {
                StopCoroutine(_powerCoroutine);
                Log(nameof(PowerOff) + " started");
            }
        }


        private void AssertInspectorRefsNotNull()
        {
            Debug.Assert(
                _probeBehavior != null,
                "[GasAnalyzerController] Probe Behavior reference is missing!"
            );

            Debug.Assert(
                _display != null,
                "[GasAnalyzerController] Display reference is missing!"
            );

            Debug.Assert(
                _powerButton != null,
                "[GasAnalyzerController] Power Button reference is missing!"
            );
        }

        private void Log(string message)
        {
            if (IsLogging)
            {
                Debug.Log($"[GasAnalyzerController] {message}");
            }
        }

        private void SwitchState()
        {
            if (State == GasAnalyzerState.Inactive)
            {
                State = GasAnalyzerState.Active;
                _powerCoroutine = null;
                _updateCoroutine = StartCoroutine(UpdateCoroutine());
                // activate display
                // activate probe
                Log(State.ToString());
            }
            else if (State == GasAnalyzerState.Active)
            {
                State = GasAnalyzerState.Inactive;
                _powerCoroutine = null;
                StopCoroutine(_updateCoroutine);
                // deactivate display
                // deactivate probe
                Log(State.ToString());
            }
        }


        private IEnumerator PowerCoroutine()
        {
            yield return new WaitForSeconds(_powerDelaySeconds);
            SwitchState();
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
                    _display.UpdateInfo(
                        zonesCount: dangerZones.Count,
                        nearZoneSize: nearestDangerZone.Value.Key.Size,
                        nearZoneId: nearestDangerZone.Value.Key.Id,
                        nearZoneDistance: nearestDangerZone.Value.Value
                    );
                }

                yield return null;
            }
        }
    }
}
