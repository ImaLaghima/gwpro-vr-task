using UnityEngine;
using VRTask.GasAnalyzer.Common;

namespace VRTask.GasAnalyzer.DangerZone
{
    [DisallowMultipleComponent]
    public class DangerZoneIdentity : MonoBehaviour
    {
        [SerializeField]
        private DangerZoneSize _size = DangerZoneSize.Unset;
        [SerializeField]
        private int _id;


        public DangerZoneSize Size => _size;
        public int Id => _id;


        public (DangerZoneSize size, int id) Get()
        {
            return (Size, Id);
        }
    }
}
