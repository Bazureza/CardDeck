using GuraGames.Enums;
using UnityEngine;

namespace GuraGames.System
{
    public static class GGDebug
    {
        private static bool _active;

        private const string HEADER_MESSAGE = "<color=#FF00FF>[GGConsole]</color>";
        private const string ACTIVATE_MESSAGE = "<color=#00FF00>Debug Mode Activated!</color>";
        private const string DEACTIVATE_MESSAGE = "<color=#FF0000>Debug Mode Deactivated!</color>";

        public static void Activate(bool active)
        {
            _active = active;
            _console(_active ? ACTIVATE_MESSAGE : DEACTIVATE_MESSAGE, DebugType.Default);
        }

        public static void Console(object message, DebugType debugType = DebugType.Default)
        {
            if (_active)
            {
                _console(message.ToString(), debugType);
            }
        }

        private static void _console(string message, DebugType debugType)
        {
            switch (debugType)
            {
                case DebugType.Default:
                    Debug.Log($"{HEADER_MESSAGE} {message}");
                    break;
                case DebugType.Warning:
                    Debug.LogWarning($"{HEADER_MESSAGE} {message}");
                    break;
                case DebugType.Error:
                    Debug.LogError($"{HEADER_MESSAGE} {message}");
                    break;
            }
        }
    }
}