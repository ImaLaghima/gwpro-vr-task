using UnityEngine;
using VRTask.Crane.Common;

namespace VRTask.Crane.InputProvider
{
    [DisallowMultipleComponent]
    public class KeyboardInputProvider : InputProviderBase
    {
        [Header("Keyboard Input Provider")]
        [Space]
        [Header("Input Keys")]
        [SerializeField]
        private KeyCode _keyUp = KeyCode.Keypad9;

        [SerializeField]
        private KeyCode _keyDown = KeyCode.Keypad7;

        [SerializeField]
        private KeyCode _keyWest = KeyCode.Keypad6;

        [SerializeField]
        private KeyCode _keyEast = KeyCode.Keypad4;

        [SerializeField]
        private KeyCode _keyNorth = KeyCode.Keypad5;

        [SerializeField]
        private KeyCode _keySouth = KeyCode.Keypad8;


        private void Update()
        {
            // up

            if (Input.GetKeyDown(_keyUp))
            {
                StartActionWithLog(CraneAction.Up);
            }

            if (Input.GetKeyUp(_keyUp))
            {
               StopActionWithLog(CraneAction.Up);
            }

            // down

            if (Input.GetKeyDown(_keyDown))
            {
                StartActionWithLog(CraneAction.Down);
            }

            if (Input.GetKeyUp(_keyDown))
            {
                StopActionWithLog(CraneAction.Down);
            }

            // west

            if (Input.GetKeyDown(_keyWest))
            {
                StartActionWithLog(CraneAction.West);
            }

            if (Input.GetKeyUp(_keyWest))
            {
                StopActionWithLog(CraneAction.West);
            }

            // east

            if (Input.GetKeyDown(_keyEast))
            {
                StartActionWithLog(CraneAction.East);
            }

            if (Input.GetKeyUp(_keyEast))
            {
                StopActionWithLog(CraneAction.East);
            }

            // north

            if (Input.GetKeyDown(_keyNorth))
            {
                StartActionWithLog(CraneAction.North);
            }

            if (Input.GetKeyUp(_keyNorth))
            {
                StopActionWithLog(CraneAction.North);
            }

            // south

            if (Input.GetKeyDown(_keySouth))
            {
                StartActionWithLog(CraneAction.South);
            }

            if (Input.GetKeyUp(_keySouth))
            {
                StopActionWithLog(CraneAction.South);
            }
        }
    }
}
