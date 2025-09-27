using System.Globalization;
using TMPro;
using UnityEngine;
using VRTask.GasAnalyzer.Common;

namespace VRTask.GasAnalyzer.Display
{
    [DisallowMultipleComponent]
    public class GasAnalyzerDisplay : MonoBehaviour
    {
        [Header("Gas Analyzer Display")]
        [SerializeField]
        private TextMeshProUGUI _dangerZonesDetectedText = null!;

        [SerializeField]
        private TextMeshProUGUI _nearDangerZoneSizeText = null!;

        [SerializeField]
        private TextMeshProUGUI _nearDangerZoneIdText = null!;

        [SerializeField]
        private TextMeshProUGUI _nearDangerZoneDistanceText = null!;


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
