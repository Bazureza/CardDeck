using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GuraGames.GameSystem;
using GuraGames.Enums;
using TomGustin.GameDesignPattern;

namespace GuraGames.Manager
{
    public class GameManager : BaseManager
    {
        private TurnBasedManager tbm;

        protected override void OnAwake()
        {
            base.OnAwake();
            GGDebug.Activate(true);

            tbm = ServiceLocator.Resolve<TurnBasedManager>();
        }

        protected override void OnStart()
        {
            base.OnStart();

            tbm.StartTurnBased(CharacterType.Player);
        }
    }
}