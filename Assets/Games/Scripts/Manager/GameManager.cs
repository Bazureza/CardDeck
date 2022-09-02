using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GuraGames.GameSystem;
using GuraGames.Enums;
using TomGustin.GameDesignPattern;
using GuraGames.Character;
using GuraGames.Level;

namespace GuraGames.Manager
{
    public class GameManager : BaseManager
    {
        private TurnBasedManager tbm;
        private AutoSaveManager autosave;
        private PlayerCharacterSystem player;
        private LevelDataManager level;
        private DeckManager deckManager;
        private CameraSystem cameraSystem;
        private ShopManager shopManager;

        protected override void OnAwake()
        {
            base.OnAwake();
            GGDebug.Activate(true);

            tbm = ServiceLocator.Resolve<TurnBasedManager>();
            autosave = ServiceLocator.Resolve<AutoSaveManager>();
            player = ServiceLocator.Resolve<PlayerCharacterSystem>();
            level = ServiceLocator.Resolve<LevelDataManager>();
            deckManager = ServiceLocator.Resolve<DeckManager>();
            cameraSystem = ServiceLocator.Resolve<CameraSystem>();
            shopManager = ServiceLocator.Resolve<ShopManager>();

            LoadAllData();
        }

        protected override void OnStart()
        {
            base.OnStart();

            MouseInputSystem.Active = true;
            tbm.StartTurnBased(CharacterType.Player);

            SceneSystem.ReadytoLoad();
        }

        private void LoadAllData()
        {
            autosave.LoadData(out bool result);
            var data = autosave.CurrentData;

            if (result)
            {
                level.InitDataLevel(data.sublevel_id);
                deckManager.InitDataDeck(data.card_metadata);
                player.InitDataPlayer(data.position, data.health, data.coin);
                cameraSystem.InitCameraToSubLevel(data.current_sublevel_id);
                level.UpdateIndicatorGlobal();
                shopManager.InitDataShop(data.shop_card);
            }
            else
            {
                level.DefaultInit();
                player.DefaultInit();
                deckManager.DefaultInit();
            }
        }
    }
}