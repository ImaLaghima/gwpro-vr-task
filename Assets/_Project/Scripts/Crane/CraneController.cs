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

        [Header("Additional Extra")]
        [SerializeField]
        private GameObject? _tube;
        [SerializeField]
        private AudioSource? _rotationAudioSource;
        [SerializeField]
        private float _tubeRotationSpeed = 3.7f;

        private Coroutine? _moveCoroutine;
        private Coroutine? _tubeRotateCoroutine;


        private event Action? _OnStopHandled;


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

            _OnStopHandled = null;
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

        private void HandleUp()
        {
            HandleMoveStart(Vector3.up);
        }

        private void HandleDown()
        {
            HandleMoveStart(Vector3.down);
        }

        private void HandleWest()
        {
            HandleMoveStart(Vector3.right);
        }

        private void HandleEast()
        {
            HandleMoveStart(Vector3.left);
        }

        private void HandleNorth()
        {
            HandleMoveStart(Vector3.back);
        }

        private void HandleSouth()
        {
            HandleMoveStart(Vector3.forward);
        }

        private void HandleStop()
        {
            HandleMoveStop();
        }

        private void HandleMoveStart(Vector3 direction)
        {
            if (_moveCoroutine != null)
            {
                return;
            }

            // Assume only one axis is not a zero
            if (direction.z != 0)
            {
                _moveCoroutine = StartCoroutine(MoveBeamHolderCoroutine(direction));
            }
            else if (direction.x != 0)
            {
                _moveCoroutine = StartCoroutine(MoveBeamCoroutine(direction));
            }
            else if (direction.y != 0)
            {
                _moveCoroutine = StartCoroutine(MoveHookCoroutine(direction));
                StartTubeRotation(direction);
            }
        }

        private void StartTubeRotation(Vector3 direction)
        {
            if (direction.y == 0)
            {
                return;
            }

            if (_tubeRotateCoroutine == null)
            {
                _tubeRotateCoroutine = StartCoroutine(
                    TubeRotateCoroutine(isReversed: direction.y > 0)
                );
                _OnStopHandled += () =>
                {
                    StopCoroutine(_tubeRotateCoroutine);
                };
            }

            if (_rotationAudioSource != null)
            {
                _rotationAudioSource.Play();
                _OnStopHandled += () =>
                {
                    _rotationAudioSource.Stop();
                };
            }
        }

        private void HandleMoveStop()
        {
            if (_moveCoroutine == null)
            {
                return;
            }

            StopCoroutine(_moveCoroutine);
            _moveCoroutine = null;
            _OnStopHandled?.Invoke();
            _OnStopHandled = null;
        }


        private IEnumerator MoveBeamHolderCoroutine(Vector3 direction)
        {
            if (direction.z == 0)
            {
                yield break;
            }

            while (true)
            {
                float nextPositionZ = _bhObject.transform.localPosition.z +
                                      (_bhMoveSpeed * Time.deltaTime * direction.z);
                float clampedPositionZ = Mathf.Clamp(
                    nextPositionZ,
                    _bhMoveConstraintMin.transform.localPosition.z,
                    _bhMoveConstraintMax.transform.localPosition.z
                );
                _bhObject.transform.localPosition = new Vector3(
                    _bhObject.transform.localPosition.x,
                    _bhObject.transform.localPosition.y,
                    clampedPositionZ
                );

                yield return null;
            }
        }

        private IEnumerator MoveBeamCoroutine(Vector3 direction)
        {
            if (direction.x == 0)
            {
                yield break;
            }

            while (true)
            {
                float nextPositionX = _beamObject.transform.localPosition.x +
                                      (_beamMoveSpeed * Time.deltaTime * direction.x);
                float clampedPositionX = Mathf.Clamp(
                    nextPositionX,
                    _beamMoveConstraintMin.transform.localPosition.x,
                    _beamMoveConstraintMax.transform.localPosition.x
                );
                _beamObject.transform.localPosition = new Vector3(
                    clampedPositionX,
                    _beamObject.transform.localPosition.y,
                    _beamObject.transform.localPosition.z
                );

                yield return null;
            }
        }

        private IEnumerator MoveHookCoroutine(Vector3 direction)
        {
            if (direction.y == 0)
            {
                yield break;
            }

            while (true)
            {
                float nextPositionY = _hookObject.transform.localPosition.y +
                                      (_hookMoveSpeed * Time.deltaTime * direction.y);
                float clampedPositionY = Mathf.Clamp(
                    nextPositionY,
                    _hookMoveConstraintMin.transform.localPosition.y,
                    _hookMoveConstraintMax.transform.localPosition.y
                );
                _hookObject.transform.localPosition = new Vector3(
                    _hookObject.transform.localPosition.x,
                    clampedPositionY,
                    _hookObject.transform.localPosition.z
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

        private IEnumerator TubeRotateCoroutine(bool isReversed = false)
        {
            if (_tube == null)
            {
                yield break;
            }

            float multiplier = isReversed ? -1 : 1;
            float rotationSpeed = _tubeRotationSpeed * multiplier;

            while (true)
            {
                _tube.transform.Rotate(Vector3.right, rotationSpeed);
                yield return null;
            }
        }
    }
}
