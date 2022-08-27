using GuraGames.Character;
using GuraGames.Data;
using TGC.MDS;
using TomGustin.GameDesignPattern;

namespace GuraGames.Manager
{
    public class AutoSaveManager : BaseManager
    {
        private PlayerCharacterSystem _player;
        private PlayerCharacterSystem player
        {
            get
            {
                if (_player) _player = ServiceLocator.Resolve<PlayerCharacterSystem>();
                return _player;
            }
        }

        private DeckManager _deck;
        private DeckManager deck
        {
            get
            {
                if (_deck) _deck = ServiceLocator.Resolve<DeckManager>();
                return _deck;
            }
        }

        private CurrentStateData currentData;

        protected override void OnAwake()
        {
            base.OnAwake();

            currentData = new CurrentStateData();
        }

        private void SyncData()
        {
            currentData.position = player.transform.position;
            currentData.health = player.CharacterData.CurrentHealthPoint;
            currentData.coin = player.CollectibleData.coin;
            currentData.card_metadata.Clear();

            foreach (CardData card in deck.GetAllDecks)
            {
                currentData.card_metadata.Add(card.card_id);
            }
        }

        public void AutoSave()
        {
            SyncData();
            MDSSaveSystem.Save(currentData, "current_data");
        }
    }
}