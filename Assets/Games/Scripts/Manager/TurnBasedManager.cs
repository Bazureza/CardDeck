using GuraGames.Character;
using GuraGames.Enums;
using GuraGames.Interface;
using GuraGames.Level;
using GuraGames.UI;
using MonsterLove.StateMachine;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
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
        private DeckManager deck;
        private TurnUI t_ui;

        private bool lastConditionIsEnemyExist;
        private bool can_next;

        [SerializeField, ReadOnly] private List<CharacterType> turn_queue = new List<CharacterType>();

        protected override void OnAwake()
        {
            base.OnAwake();
            player = ServiceLocator.Resolve<PlayerCharacterSystem>();
            level = ServiceLocator.Resolve<LevelDataManager>();
            deck = ServiceLocator.Resolve<DeckManager>();
            t_ui = ServiceLocator.Resolve<TurnUI>();
            state = StateMachine<CharacterType>.Initialize(this);
        }

        public void StartTurnBased(CharacterType firstTurn)
        {
            currentTurn = firstTurn;
            UpdateTurnQueue();
            ChangeTurn(firstTurn);
        }

        public void NextTurn()
        {
            can_next = true;
            level.CheckEnemyClearanceActiveSubLevel();
        }

        public void UpdateTurnQueue()
        {
            turn_queue.Clear();

            var enemies = level.GetActiveSubLevelEnemies();

            for (int i = 0; i < enemies.Count; i++) turn_queue.Add(CharacterType.Enemy);

            if (currentTurn.Equals(CharacterType.Player)) turn_queue.Insert(0, CharacterType.Player);
            else turn_queue.Add(CharacterType.Player);

            t_ui.UpdateLengthQueueUI(turn_queue);
        }

        private void ShiftQueue()
        {
            if (turn_queue[0].Equals(currentTurn))
            {
                var last = turn_queue[0];
                turn_queue.RemoveAt(0);
                turn_queue.Add(last);
            }

            t_ui.UpdateQueueUI(turn_queue);
        }

        [Button]
        private void ChangeTurn(CharacterType turn)
        {
            ShiftQueue();

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

            var enemies = level.GetActiveSubLevelEnemies();
            yield return null;
            foreach (BaseCharacterSystem bcs in enemies)
            {
                can_next = false;
                ((ITurnBased)bcs).StartTurn();
                yield return new WaitUntil(() => can_next);
                yield return null;
                ShiftQueue();
            }

            yield return new WaitForEndOfFrame();
            Console("Enemy Turn Done");

            yield return null;
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