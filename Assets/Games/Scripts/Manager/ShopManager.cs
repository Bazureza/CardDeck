using GuraGames.Character;
using GuraGames.GameSystem;
using GuraGames.UI;
using System.Collections.Generic;
using TomGustin.GameDesignPattern;
using UnityEngine;
using UnityEngine.UI;

namespace GuraGames.Manager
{
    public class ShopManager : MonoBehaviour
    {
        [SerializeField] private List<ShopStock> shopStocks;

        private CardData selectedCard;

        private ShopUI _shop_ui;
        private ShopUI shop_ui
        {
            get
            {
                if (!_shop_ui) _shop_ui = ServiceLocator.Resolve<ShopUI>();
                return _shop_ui;
            }
        }

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

        private bool isBusy;

        public void RenderShop()
        {
            GGDebug.Console("Render Shop");
            shop_ui.RenderShop_BuyCard(shopStocks);
        }

        public bool SelectCard(CardData card)
        {
            selectedCard = card;
            return BuyCard();
        }

        public bool BuyCard()
        {
            var fetched = shopStocks.Find((x) => x.card.Equals(selectedCard));
            if (fetched != null)
            {
                var status = player.RemoveCoin(fetched.price);
                if (status)
                {
                    GGDebug.Console($"Buy Card: {(selectedCard ? selectedCard.card_name : "Null")}");
                    fetched.sold = true;
                    //add to deck
                    deck.AddCardToDeck(fetched.card);
                }

                return status;
            } else GGDebug.Console($"Buy Failed! Card {selectedCard.card_name} is not exist on stock.");

            return false;
        }

        public bool IsBusy()
        {
            return isBusy;
        }

        [System.Serializable]
        public class ShopStock
        {
            public CardData card;
            public int price;
            public bool sold;
        }
    }
}