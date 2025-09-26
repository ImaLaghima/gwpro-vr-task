using UnityEngine;
using VRTask.Crane.RemoteController;

namespace VRTask.Crane.Controller
{
    [DisallowMultipleComponent]
    public class CraneController : MonoBehaviour
    {
        [Header("Crane Input")]
        private CraneRemoteController _remoteController = null!;

        [Header("Beam Holder")]
        [SerializeField]
        private GameObject _beamHolder = null!;

        [Header("Beam")]
        [SerializeField]
        private GameObject _beam = null!;

        [Header("Hook")]
        [SerializeField]
        private GameObject _craneHook = null!;


        private void Awake()
        {
            Debug.Assert(
                _remoteController != null,
                "[CraneController] RemoteController reference is missing!"
            );

            Debug.Assert(
                _beamHolder != null,
                "[CraneController] BeamHolder reference is missing!"
            );

            Debug.Assert(
                _beam != null,
                "[CraneController] Beam reference is missing!"
            );

            Debug.Assert(
                _craneHook != null,
                "[CraneController] CraneHook reference is missing!"
            );
        }

        private void OnEnable()
        {
            _remoteController.OnUp.AddListener(HandleUp);
            _remoteController.OnDown.AddListener(HandleDown);
            _remoteController.OnWest.AddListener(HandleWest);
            _remoteController.OnEast.AddListener(HandleEast);
            _remoteController.OnNorth.AddListener(HandleNorth);
            _remoteController.OnSouth.AddListener(HandleSouth);
            _remoteController.OnActiveStopped.AddListener(HandleStop);
        }

        private void OnDisable()
        {
            _remoteController.OnUp.RemoveListener(HandleUp);
            _remoteController.OnDown.RemoveListener(HandleDown);
            _remoteController.OnWest.RemoveListener(HandleWest);
            _remoteController.OnEast.RemoveListener(HandleEast);
            _remoteController.OnNorth.RemoveListener(HandleNorth);
            _remoteController.OnSouth.RemoveListener(HandleSouth);
            _remoteController.OnActiveStopped.AddListener(HandleStop);
        }


        private void HandleUp()
        {

        }

        private void HandleDown()
        {

        }

        private void HandleWest()
        {

        }

        private void HandleEast()
        {

        }

        private void HandleNorth()
        {

        }

        private void HandleSouth()
        {

        }

        private void HandleStop()
        {

        }
    }
}
