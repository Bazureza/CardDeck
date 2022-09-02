using GuraGames.Character;
using GuraGames.Data;
using GuraGames.Level;
using Sirenix.OdinInspector;
using TGC.MDS;
using TomGustin.GameDesignPattern;
using UnityEngine;

namespace GuraGames.Manager
{
    public class AutoSaveManager : BaseManager
    {
        private PlayerCharacterSystem _player;
        private PlayerCharacterSystem player
        {
            get
            {
                if (!_player) _player = ServiceLocator.Resolve<PlayerCharacterSystem>();
                return _player;
            }
        }

        private DeckManager _deck;
        private DeckManager deck
        {
            get
            {
                if (!_deck) _deck = ServiceLocator.Resolve<DeckManager>();
                return _deck;
            }
        }

        private LevelDataManager _level;
        private LevelDataManager level
        {
            get
            {
                if (!_level) _level = ServiceLocator.Resolve<LevelDataManager>();
                return _level;
            }
        }

        private ShopManager _shop;
        private ShopManager shop
        {
            get
            {
                if (!_shop) _shop = ServiceLocator.Resolve<ShopManager>();
                return _shop;
            }
        }

        [SerializeField, ReadOnly] private CurrentStateData currentData;

        public CurrentStateData CurrentData { get { return currentData; } }

        protected override void OnAwake()
        {
            base.OnAwake();

            currentData = new CurrentStateData();
        }

        private void SyncData()
        {
            currentData.position = player.transform.position;
            currentData.current_sublevel_id = level.GetCurrentSubLevelID();
            currentData.health = player.CharacterData.CurrentHealthPoint;
            currentData.coin = player.CollectibleData.coin;

            currentData.card_metadata.Clear();
            currentData.sublevel_id.Clear();
            currentData.shop_card.Clear();

            foreach (CardData card in deck.GetAllDecks)
            {
                currentData.card_metadata.Add(card.card_id);
            }

            currentData.sublevel_id.AddRange(level.GetClearedSubLevel());
            currentData.shop_card.AddRange(shop.GetCardBuys);
        }

        [Button]
        public void AutoSave()
        {
            SyncData();
            MDSSaveSystem.Save(currentData, "current_data");
        }

        public void LoadData(out bool result)
        {
            currentData = MDSSaveSystem.Load("current_data", out result);
        }
    }
}