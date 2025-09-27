using System;
using System.Collections;
using UnityEngine;
using VRTask.Crane.RemoteController;

namespace VRTask.Crane.Controller
{
    [DisallowMultipleComponent]
    public class CraneController : MonoBehaviour
    {
        [Header("Crane Input")]
        [SerializeField]
        private CraneRemoteController _remoteController = null!;

        [Header("Beam Holder (BH)")]
        [SerializeField]
        private GameObject _bhObject = null!;

        [SerializeField]
        private GameObject _bhMoveConstraintMin = null!;

        [SerializeField]
        private GameObject _bhMoveConstraintMax = null!;

        [SerializeField]
        private float _bhMoveSpeed = 3.7f;

        [Header("Beam")]
        [SerializeField]
        private GameObject _beamObject = null!;

        [Header("Hook")]
        [SerializeField]
        private GameObject _hookObject = null!;

        private Coroutine? _moveCoroutine;
        private Vector3 _moveDirection = Vector3.zero;


        private void Awake()
        {
            AssertInspectorRefsNotNull();
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

            _moveCoroutine = StartCoroutine(MoveCoroutine());
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

            StopCoroutine(_moveCoroutine);
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
            _moveDirection = Vector3.forward;
        }

        private void HandleSouth()
        {
            _moveDirection = Vector3.back;
        }

        private void HandleStop()
        {
            _moveDirection = Vector3.zero;
        }

        private void AssertInspectorRefsNotNull()
        {
            Debug.Assert(
                _remoteController != null,
                "[CraneController] RemoteController reference is missing!"
            );

            Debug.Assert(
                _bhObject != null,
                "[CraneController] BeamHolder reference is missing!"
            );

            Debug.Assert(
                _bhMoveConstraintMin != null,
                "[CraneController] BeamHolder's MoveConstraintMin reference is missing!"
            );

            Debug.Assert(
                _bhMoveConstraintMax != null,
                "[CraneController] BeamHolder's MoveConstraintMax reference is missing!"
            );

            Debug.Assert(
                _beamObject != null,
                "[CraneController] Beam reference is missing!"
            );

            Debug.Assert(
                _hookObject != null,
                "[CraneController] CraneHook reference is missing!"
            );
        }


        private IEnumerator MoveCoroutine()
        {
            // TODO: fix coroutine: prevent infinite work
            while (true)
            {
                float nextPositionZ = _bhObject.transform.position.z +
                                     (_bhMoveSpeed * Time.deltaTime * _moveDirection.z);
                float clampedPositionZ = Mathf.Clamp(
                    nextPositionZ,
                    _bhMoveConstraintMin.transform.position.z,
                    _bhMoveConstraintMax.transform.position.z
                );
                _bhObject.transform.position = new Vector3(
                    _bhObject.transform.position.x,
                    _bhObject.transform.position.y,
                    clampedPositionZ
                );
                yield return null;
            }
        }
    }
}
