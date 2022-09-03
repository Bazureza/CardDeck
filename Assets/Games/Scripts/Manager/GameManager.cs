using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GuraGames.GameSystem;
using GuraGames.Enums;
using TomGustin.GameDesignPattern;
using GuraGames.Character;
using GuraGames.Level;
using TGC.MDS;
using Sirenix.OdinInspector;

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
        private WinLoseManager winLoseManager;

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
            winLoseManager = ServiceLocator.Resolve<WinLoseManager>();

            LoadAllData();
        }

        protected override void OnStart()
        {
            base.OnStart();

            MouseInputSystem.Active = true;
            tbm.StartTurnBased(CharacterType.Player);
            
            SceneSystem.ReadytoLoad();
        }

        public void Lose()
        {
            winLoseManager.GameLose();
            StopGame();
        }

        public void Win()
        {
            winLoseManager.GameWin();
            StopGame();
        }

        public void MainMenu()
        {
            SceneSystem.LoadScene("MENU");
        }

        public void BactToMenuAfterWin()
        {
            MDSSaveSystem.DeleteAllSaveData("current_data");
            SceneSystem.LoadScene("MENU");
        }

        public void Retry()
        {
            SceneSystem.LoadScene("GAME");
        }

        private void StopGame()
        {
            MouseInputSystem.Active = false;
            tbm.StopTurn();
        }

        private void LoadAllData()
        {
            autosave.LoadData(out bool result);
            var data = autosave.CurrentData;

            if (result)
            {
                level.InitDataLevel(data.level_id, data.sublevel_id);
                deckManager.InitDataDeck(data.card_metadata);
                player.InitDataPlayer(data.position, data.health, data.coin);
                cameraSystem.InitCameraToSubLevel(data.current_sublevel_id);
                level.UpdateIndicatorGlobal();
                shopManager.InitDataShop(data.shop_card);
            }
            else
            {
                level.DefaultInit();
                cameraSystem.InitCameraToSubLevel(0);
                player.DefaultInit();
                deckManager.DefaultInit();
            }
        }
    }
}