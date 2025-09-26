using UnityEngine;
using UnityEngine.Events;
using VRTask.Crane.Common;
using VRTask.Crane.InputProvider;

namespace VRTask.Crane.RemoteController
{
    [DisallowMultipleComponent]
    public class CraneRemoteController : MonoBehaviour
    {
        [SerializeField]
        private bool _isLogging = true;

        [SerializeField]
        private InputProviderBase _inputProvider = null!;


        public bool IsLogging => _isLogging;


        [SerializeField]
        private UnityEvent _onUp = new();

        [SerializeField]
        private UnityEvent _onDown = new();

        [SerializeField]
        private UnityEvent _onWest = new();

        [SerializeField]
        private UnityEvent _onEast = new();

        [SerializeField]
        private UnityEvent _onNorth = new();

        [SerializeField]
        private UnityEvent _onSouth = new();

        [SerializeField]
        private UnityEvent _onActiveStopped = new();

        public UnityEvent OnUp => _onUp;
        public UnityEvent OnDown => _onDown;
        public UnityEvent OnWest => _onWest;
        public UnityEvent OnEast => _onEast;
        public UnityEvent OnNorth => _onNorth;
        public UnityEvent OnSouth => _onSouth;
        public UnityEvent OnActiveStopped => _onActiveStopped;


        private void Awake()
        {
            Debug.Assert(
                _inputProvider != null,
                "[CraneRemoteController]: InputProvider reference is missing!"
            );
        }

        private void OnEnable()
        {
            _inputProvider.OnActionStarted += HandleInputStarted;
            _inputProvider.OnActionStopped += HandleInputStopped;
        }

        private void OnDisable()
        {
            _inputProvider.OnActionStarted -= HandleInputStarted;
            _inputProvider.OnActionStopped -= HandleInputStopped;
        }


        private void HandleInputStarted(CraneAction action)
        {
            switch (action)
            {
                case CraneAction.Up:
                {
                    OnUp.Invoke();
                    Log(nameof(OnUp));
                    break;
                }

                case CraneAction.Down:
                {
                    OnDown.Invoke();
                    Log(nameof(OnDown));
                    break;
                }

                case CraneAction.West:
                {
                    OnWest.Invoke();
                    Log(nameof(OnWest));
                    break;
                }

                case CraneAction.East:
                {
                    OnEast.Invoke();
                    Log(nameof(OnEast));
                    break;
                }

                case CraneAction.North:
                {
                    OnNorth.Invoke();
                    Log(nameof(OnNorth));
                    break;
                }

                case CraneAction.South:
                {
                    OnSouth.Invoke();
                    Log(nameof(OnSouth));
                    break;
                }
            }
        }

        private void HandleInputStopped(CraneAction action)
        {
            // We deliberately will not check for the stopped action to be
            // the same action that had started before (at least for now);
            // We assume that each started action always ends before the next one;
            OnActiveStopped.Invoke();
        }

        private void Log(string message)
        {
            if (IsLogging)
            {
                Debug.Log($"[CraneRemoteController]: {message}");
            }
        }
    }
}
