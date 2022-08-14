using GuraGames.AI;
using GuraGames.GameSystem;
using GuraGames.Interface;
using GuraGames.Manager;
using System.Collections;
using TomGustin.GameDesignPattern;
using UnityEngine;
using UnityTaskManager;

namespace GuraGames.Character
{
    public class BaseCharacterSystem : MonoBehaviour, ITurnBased, ICharacterHit
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

        protected bool onAction;
        protected bool onScan;

        protected virtual void OnAwake() 
        {
            agent.Init();
            characterData.SetHealth();
            characterData.SetMana();
            characterData.SetMove();
        }

        protected virtual void OnStart() { }

        protected virtual void OnLoop() { }

        protected virtual void OnMove() { }

        public virtual void MoveTo(Vector3 move_position)
        {
            if (onAction || onScan) return;
            new Task(DecideMoveTo(move_position));
        }

        private IEnumerator DecideMoveTo(Vector3 move_position)
        {
            onScan = true;
            yield return agent.Scan(move_position, () => OnMove());
            onScan = false;
        }

        protected virtual void Hit(int damage)
        {
            characterData.UpdateHealth(damage);

            if (characterData.CurrentHealthPoint == 0) Dead();
        }

        protected virtual void Dead() { }

        protected virtual void StartTurnWorld()
        {

        }

        protected virtual void NextTurnWorld()
        {
            characterData.UpdateMana(-1);
        }

        protected virtual void EndTurnWorld()
        {
            characterData.SetMana(characterData.BaseManaPoint, true);
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

        void ITurnBased.StartTurn()
        {
            StartTurnWorld();
        }

        void ITurnBased.NextTurn()
        {
            NextTurnWorld();
        }

        void ITurnBased.EndTurn()
        {
            EndTurnWorld();
        }

        void ICharacterHit.Hit(string sender, int damage)
        {
            GGDebug.Console($"{sender} is hitting {gameObject.name}! -{damage}HP");
            Hit(-damage);
        }
    }
}