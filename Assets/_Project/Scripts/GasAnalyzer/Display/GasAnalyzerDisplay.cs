using System.Globalization;
using TMPro;
using UnityEngine;
using VRTask.GasAnalyzer.DangerZone;

namespace VRTask.GasAnalyzer.Display
{
    [DisallowMultipleComponent]
    public class GasAnalyzerDisplay : MonoBehaviour
    {
        [Header("Gas Analyzer Display")]
        [SerializeField]
        private TextMeshPro _dangerZonesDetectedText = null!;
        [SerializeField]
        private TextMeshPro _nearDangerZoneSizeText = null!;
        [SerializeField]
        private TextMeshPro _nearDangerZoneIdText = null!;
        [SerializeField]
        private TextMeshPro _nearDangerZoneDistanceText = null!;
        [SerializeField]
        private GameObject? _visibleObjectsRoot;


        private void Awake()
        {
            AssertInspectorRefsNotNull();
        }


        public void Activate()
        {
            _visibleObjectsRoot?.SetActive(true);
        }

        public void Deactivate()
        {
            _visibleObjectsRoot?.SetActive(false);
        }


        public void UpdateZonesDetected(int dangerZonesDetected)
        {
            _dangerZonesDetectedText.text = dangerZonesDetected.ToString();
        }

        public void UpdateNearDangerZoneSize(DangerZoneSize size)
        {
            _nearDangerZoneSizeText.text = size.ToString();
        }

        public void UpdateNearDangerZoneId(int nearDangerZoneId)
        {
            _nearDangerZoneIdText.text = nearDangerZoneId.ToString();
        }

        public void UpdateNearDangerZoneDistance(float nearDangerZoneDistance)
        {
            _nearDangerZoneDistanceText.text = nearDangerZoneDistance.ToString(
                CultureInfo.CurrentCulture
            );
        }


        private void AssertInspectorRefsNotNull()
        {
            Debug.Assert(
                _dangerZonesDetectedText != null,
                "[GasAnalyzerDisplay] DangerZonesDetected Text reference is missing"
            );

            Debug.Assert(
                _nearDangerZoneSizeText != null,
                "[GasAnalyzerDisplay] Near DangerZone Size reference is missing"
            );

            Debug.Assert(
                _nearDangerZoneIdText != null,
                "[GasAnalyzerDisplay] Near DangerZone ID reference is missing"
            );

            Debug.Assert(
                _nearDangerZoneDistanceText != null,
                "[GasAnalyzerDisplay] Near DangerZone Distance reference is missing"
            );
        }
    }
}
