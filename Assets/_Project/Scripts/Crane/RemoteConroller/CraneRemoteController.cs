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

        public UnityEvent OnUp => _onUp;
        public UnityEvent OnDown => _onDown;
        public UnityEvent OnWest => _onWest;
        public UnityEvent OnEast => _onEast;
        public UnityEvent OnNorth => _onNorth;
        public UnityEvent OnSouth => _onSouth;


        private void Awake()
        {
            Debug.Assert(
                _inputProvider != null,
                "[CraneRemoteController]: InputProvider reference is missing!"
            );
        }

        private void OnEnable()
        {
            _inputProvider.OnActionStarted += HandleInputEvent;
        }

        private void OnDisable()
        {
            _inputProvider.OnActionStarted -= HandleInputEvent;
        }


        private void HandleInputEvent(CraneAction action)
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

        private void Log(string message)
        {
            if (IsLogging)
            {
                Debug.Log($"[CraneRemoteController]: {message}");
            }
        }
    }
}
