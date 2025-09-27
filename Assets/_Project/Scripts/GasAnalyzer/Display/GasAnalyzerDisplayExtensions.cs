using VRTask.GasAnalyzer.Common;

namespace VRTask.GasAnalyzer.Display
{
    public static class GasAnalyzerDisplayExtensions
    {
        public static void UpdateInfo(
            this GasAnalyzerDisplay gasAnalyzerDisplay,
            int zonesCount,
            DangerZoneSize nearZoneSize,
            int nearZoneId,
            float nearZoneDistance
        )
        {
            gasAnalyzerDisplay.UpdateZonesDetected(zonesCount);
            gasAnalyzerDisplay.UpdateNearDangerZoneSize(nearZoneSize);
            gasAnalyzerDisplay.UpdateNearDangerZoneId(nearZoneId);
            gasAnalyzerDisplay.UpdateNearDangerZoneDistance(nearZoneDistance);
        }
    }
}
