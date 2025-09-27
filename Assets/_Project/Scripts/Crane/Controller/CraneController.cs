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

        [SerializeField]
        private GameObject _beamMoveConstraintMin = null!;

        [SerializeField]
        private GameObject _beamMoveConstraintMax = null!;

        [SerializeField]
        private float _beamMoveSpeed = 3.7f;

        [Header("Hook")]
        [SerializeField]
        private GameObject _hookObject = null!;

        [SerializeField]
        private GameObject _hookMoveConstraintMin = null!;

        [SerializeField]
        private GameObject _hookMoveConstraintMax = null!;

        [SerializeField]
        private float _hookMoveSpeed = 3.7f;

        [SerializeField]
        private GameObject _hookWireObject = null!;

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
            _moveDirection = Vector3.up;
        }

        private void HandleDown()
        {
            _moveDirection = Vector3.down;
        }

        private void HandleWest()
        {
            _moveDirection = Vector3.right;
        }

        private void HandleEast()
        {
            _moveDirection = Vector3.left;
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
                _beamMoveConstraintMin != null,
                "[CraneController] Beam's MoveConstraintMin reference is missing!"
            );

            Debug.Assert(
                _beamMoveConstraintMax != null,
                "[CraneController] Beam's MoveConstraintMax reference is missing!"
            );

            Debug.Assert(
                _hookObject != null,
                "[CraneController] CraneHook reference is missing!"
            );

            Debug.Assert(
                _beamMoveConstraintMin != null,
                "[CraneController] Hook's MoveConstraintMin reference is missing!"
            );

            Debug.Assert(
                _beamMoveConstraintMax != null,
                "[CraneController] Hook's MoveConstraintMax reference is missing!"
            );

            Debug.Assert(
                _hookWireObject != null,
                "[CraneController] Hook's Wire reference is missing!"
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

                float nextPositionX = _beamObject.transform.position.x +
                                      (_beamMoveSpeed * Time.deltaTime * _moveDirection.x);
                float clampedPositionX = Mathf.Clamp(
                    nextPositionX,
                    _beamMoveConstraintMin.transform.position.x,
                    _beamMoveConstraintMax.transform.position.x
                );
                _beamObject.transform.position = new Vector3(
                    clampedPositionX,
                    _beamObject.transform.position.y,
                    _beamObject.transform.position.z
                );

                float nextPositionY = _hookObject.transform.position.y +
                                      (_hookMoveSpeed * Time.deltaTime * _moveDirection.y);
                float clampedPositionY = Mathf.Clamp(
                    nextPositionY,
                    _hookMoveConstraintMin.transform.position.y,
                    _hookMoveConstraintMax.transform.position.y
                );
                _hookObject.transform.position = new Vector3(
                    _hookObject.transform.position.x,
                    clampedPositionY,
                    _hookObject.transform.position.z
                );

                _hookWireObject.transform.position = Vector3.Lerp(
                    _beamObject.transform.position,
                    _hookObject.transform.position,
                    0.5f
                );
                float distanceToCover = Vector3.Distance(
                    _beamObject.transform.position,
                    _hookObject.transform.position
                );
                Vector3 adjustedWireScale = _hookWireObject.transform.localScale;
                adjustedWireScale.y = distanceToCover * 0.5f;
                _hookWireObject.transform.localScale = adjustedWireScale;

                yield return null;
            }
        }
    }
}
