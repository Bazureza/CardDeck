using GuraGames.GameSystem;
using Pathfinding;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TomGustin.GameDesignPattern;
using UnityEngine;

namespace GuraGames.Character
{
    public class BaseCharacterSystem : MonoBehaviour
    {
        [SerializeField] protected bool allowedDiagonalMoves;

        [Header("AI")]
        [SerializeField] protected AIAgent agent;

        protected bool onMove;

        protected virtual void OnAwake() { agent.Init(); }

        protected virtual void OnStart() { }

        protected virtual void OnLoop() { }

        protected virtual void OnMove() { }

        public void MoveTo(Vector3 move_position)
        {
            if (onMove) return;
            DecideMoveTo(move_position);
        }

        private void DecideMoveTo(Vector3 move_position)
        {
            var result = agent.Scan(move_position);

            if (result) OnMove();
        }

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