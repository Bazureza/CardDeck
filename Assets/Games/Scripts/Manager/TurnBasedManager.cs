using GuraGames.Character;
using GuraGames.Enums;
using GuraGames.Interface;
using GuraGames.Level;
using MonsterLove.StateMachine;
using Sirenix.OdinInspector;
using System.Collections;
using TomGustin.GameDesignPattern;
using UnityEngine;

namespace GuraGames.Manager
{
    public class TurnBasedManager : BaseManager
    {
        [SerializeField, ReadOnly] private CharacterType currentTurn;

        private StateMachine<CharacterType> state;
        private BaseCharacterSystem player;
        private LevelDataManager level;
        
        private bool can_next;

        protected override void OnAwake()
        {
            base.OnAwake();
            player = ServiceLocator.Resolve<PlayerCharacterSystem>();
            level = ServiceLocator.Resolve<LevelDataManager>();
            state = StateMachine<CharacterType>.Initialize(this);
        }

        public void StartTurnBased(CharacterType firstTurn)
        {
            ChangeTurn(firstTurn);
        }

        public void NextTurn()
        {
            can_next = true;
        }

        [Button]
        private void ChangeTurn(CharacterType turn)
        {
            state.ChangeState(turn);
            currentTurn = turn;
        }  

        #region Turn Based
        private IEnumerator Player_Enter()
        {
            can_next = false;

            Console("Player Turn");
            ((ITurnBased)player).StartTurn();

            yield return new WaitUntil(() => can_next);
            Console("Player Done");
            ChangeTurn(CharacterType.Enemy);
        }

        private void Player_Exit()
        {
            ((ITurnBased)player).EndTurn();
        }

        private IEnumerator Enemy_Enter()
        {
            Console("Enemy Turn");

            foreach (BaseCharacterSystem bcs in level.GetActiveSubLevelEnemies())
            {
                can_next = false;
                ((ITurnBased)bcs).StartTurn();
                yield return new WaitUntil(()=> can_next);
                yield return null;
            }
            yield return null;
            yield return new WaitForEndOfFrame();
            Console("Enemy Turn Done");
            ChangeTurn(CharacterType.Player);
        }

        private void Enemy_Exit()
        {
            foreach (BaseCharacterSystem bcs in level.GetActiveSubLevelEnemies())
            {
                ((ITurnBased)bcs).EndTurn();
            }
        }
        #endregion
    }
}