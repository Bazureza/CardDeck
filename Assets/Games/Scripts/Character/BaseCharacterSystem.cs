using GuraGames.GameSystem;
using GuraGames.Interface;
using GuraGames.Manager;
using Pathfinding;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TomGustin.GameDesignPattern;
using UnityEngine;

namespace GuraGames.Character
{
    public class BaseCharacterSystem : MonoBehaviour, IWorldTurnBased
    {
        [SerializeField] protected bool allowedDiagonalMoves;
        [SerializeField] protected CharacterData characterData;

        [Header("AI")]
        [SerializeField] protected AIAgent agent;

        private TurnBasedManager _tbm;
        protected TurnBasedManager tbm
        {
            get
            {
                if (!_tbm) _tbm = ServiceLocator.Resolve<TurnBasedManager>();
                return _tbm;
            }
        }

        protected bool onMove;

        protected virtual void OnAwake() { agent.Init(); }

        protected virtual void OnStart() { }

        protected virtual void OnLoop() { }

        protected virtual void OnMove() { }

        public virtual void MoveTo(Vector3 move_position)
        {
            if (onMove) return;
            DecideMoveTo(move_position);
        }

        private void DecideMoveTo(Vector3 move_position)
        {
            StartCoroutine(agent.Scan(move_position, () => OnMove()));
        }

        protected virtual void StartTurnWorld()
        {

        }

        protected virtual void NextTurnWorld()
        {
            characterData.UpdateMana(-1);
        }

        protected virtual void EndTurnWorld()
        {
            characterData.SetMana(characterData.BaseMana, true);
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

        void IWorldTurnBased.StartTurn_World()
        {
            StartTurnWorld();
        }

        void IWorldTurnBased.NextTurn_World()
        {
            NextTurnWorld();
        }

        void IWorldTurnBased.EndTurn_World()
        {
            EndTurnWorld();
        }
    }
}