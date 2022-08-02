using GuraGames.System;
using GuraGames.Enums;
using UnityEngine;

namespace GuraGames.Manager
{
    public abstract class BaseManager : MonoBehaviour
    {
        protected virtual void OnAwake() { }
        protected virtual void OnStart() { }
        protected virtual void OnLoop() { }

        protected virtual void Console(object message, DebugType debugType = DebugType.Default) { GGDebug.Console(message, debugType); }

        private void Awake()
        {
            OnAwake();
        }

        private void Start()
        {
            OnStart();
        }

        private void Update()
        {
            OnLoop();
        }
    }
}