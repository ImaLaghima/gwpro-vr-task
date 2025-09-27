using System;
using UnityEngine;
using UnityEngine.EventSystems;
using VRTask.Crane.Common;

namespace VRTask.Crane.RemoteController
{
    public class RemoteControllerButton
        : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [Header("Remote Controller Button")]
        [SerializeField]
        private bool _isLogging = true;
        [Space]
        [SerializeField]
        private CraneAction _action;


        public bool IsLogging => _isLogging;


        public event Action<CraneAction>? OnPressed;
        public event Action<CraneAction>? OnReleased;


        public void OnPointerDown(PointerEventData eventData)
        {
            OnPressed?.Invoke(_action);
            Log($"Event.{nameof(OnPointerDown)}");
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            OnReleased?.Invoke(_action);
            Log($"Event.{nameof(OnPointerUp)}");
        }


        private void Log(string message)
        {
            if (IsLogging)
            {
                Debug.Log($"[RemoteControllerButton] {message}");
            }
        }
    }
}
