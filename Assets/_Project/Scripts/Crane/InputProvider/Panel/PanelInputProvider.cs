using UnityEngine;
using System.Collections.Generic;

using VRTask.Crane.Common;

namespace VRTask.Crane.InputProvider
{
    [DisallowMultipleComponent]
    public class PanelInputProvider : InputProviderBase
    {
        [Header("Panel Input Provider")]
        [Space]
        [SerializeField]
        private List<PanelButton> _buttons = new();


        private void OnEnable()
        {
            foreach (PanelButton button in _buttons)
            {
                button.OnPressed += HandleActionStart;
                button.OnReleased += HandleActionStop;
            }
        }

        private void OnDisable()
        {
            foreach (PanelButton button in _buttons)
            {
                button.OnPressed -= HandleActionStart;
                button.OnReleased -= HandleActionStop;
            }
        }


        private void HandleActionStart(CraneAction action)
        {
            StartActionWithLog(action);
        }

        private void HandleActionStop(CraneAction action)
        {
            StopActionWithLog(action);
        }
    }
}
