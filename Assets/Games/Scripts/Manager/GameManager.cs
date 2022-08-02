using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GuraGames.System;
using GuraGames.Enums;

namespace GuraGames.Manager
{
    public class GameManager : BaseManager
    {
        protected override void OnAwake()
        {
            base.OnAwake();
            GGDebug.Activate(false);

            Console("");
            Console("Test", DebugType.Error);
            Console(1);
            Console(2.3f);
        }
    }
}