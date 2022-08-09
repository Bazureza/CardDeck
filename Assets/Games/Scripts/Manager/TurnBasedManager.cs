using GuraGames.Character;
using GuraGames.Enums;
using GuraGames.GameSystem;
using GuraGames.Interface;
using GuraGames.Level;
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

        private bool can_next;

        protected override void OnAwake()
        {
            base.OnAwake();
            player = ServiceLocator.Resolve<PlayerCharacterSystem>();
            level = ServiceLocator.Resolve<LevelDataManager>();

            state = StateMachine<CharacterType>.Initialize(this);
            //state.Changed += (CharacterType st) => { currentTurn = st; };
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
            GGDebug.Console("Player Turn");
            ((IWorldTurnBased)player).StartTurn_World();

            yield return new WaitUntil(() => can_next);
            GGDebug.Console("Player Done");
            ChangeTurn(CharacterType.Enemy);
        }

        private IEnumerator Enemy_Enter()
        {
            GGDebug.Console("Enemy Turn");

            foreach (BaseCharacterSystem bcs in level.GetActiveSubLevelEnemies())
            {
                can_next = false;
                ((IWorldTurnBased)bcs).StartTurn_World();
                yield return new WaitUntil(()=> can_next);
                yield return null;
            }
            yield return null;
            yield return new WaitForEndOfFrame();
            GGDebug.Console("Enemy Turn Done");
            ChangeTurn(CharacterType.Player);
        }
        #endregion
    }
}