using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace VRTask.Crane.InputProvider
{
    public class PowerButton
        : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [Header("Panel Button")]
        [SerializeField]
        private bool _isLogging = true;


        public bool IsLogging => _isLogging;


        public event Action? OnPressed;
        public event Action? OnReleased;


        public void OnPointerDown(PointerEventData eventData)
        {
            OnPressed?.Invoke();
            Log($"Event.{nameof(OnPointerDown)}");
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            OnReleased?.Invoke();
            Log($"Event.{nameof(OnPointerUp)}");
        }

        public void SimulatePointerDown()
        {
            OnPressed?.Invoke();
            Log($"Event.{nameof(SimulatePointerDown)}");
        }

        public void SimulatePointerUp()
        {
            OnReleased?.Invoke();
            Log($"Event.{nameof(SimulatePointerUp)}");
        }


        private void Log(string message)
        {
            if (IsLogging)
            {
                Debug.Log($"[PowerButton] {message}");
            }
        }
    }
}
