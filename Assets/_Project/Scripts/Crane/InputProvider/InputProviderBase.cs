using System;
using UnityEngine;
using VRTask.Crane.Common;

namespace VRTask.Crane.InputProvider
{
    public abstract class InputProviderBase : MonoBehaviour
    {
        [Header("Input Provider Base")]
        [Space]
        [SerializeField]
        private bool _isLogging = true;


        protected bool IsLogging => _isLogging;


        public event Action<CraneAction>? OnActionStarted;
        public event Action<CraneAction>? OnActionStopped;


        private void OnDisable()
        {
            OnActionStarted = null;
            OnActionStopped = null;
        }


        protected void StartAction(CraneAction action)
        {
            OnActionStarted?.Invoke(action);
        }

        protected void StartActionWithLog(CraneAction action)
        {
            StartAction(action);
            if (IsLogging)
            {
                Debug.Log($"[InputProvider] START Action.{action.ToString()}");
            }
        }

        protected void StopAction(CraneAction action)
        {
            OnActionStopped?.Invoke(action);
        }

        protected void StopActionWithLog(CraneAction action)
        {
            StopAction(action);
            if (IsLogging)
            {
                Debug.Log($"[InputProvider] STOP Action.{action.ToString()}");
            }
        }
    }
}
