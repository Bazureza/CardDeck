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
        [SerializeField] private int remove_card_price;
        [SerializeField] private int heal_potion_price;
        [SerializeField] private int heal_potion_value;

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

        private List<CardData> selected_card = new List<CardData>();

        private bool isBusy;

        private List<string> card_buys_id = new List<string>();

        public List<string> GetCardBuys { get { return card_buys_id; } }

        public (int Price, int Value) HealPotion { get { return (heal_potion_price, heal_potion_value); } }

        public void InitDataShop(List<string> card_buys_id)
        {
            this.card_buys_id.Clear();
            this.card_buys_id.AddRange(card_buys_id);

            foreach (string card_id in this.card_buys_id)
            {
                if (shopStocks.Exists(x => x.card.card_id.Equals(card_id)))
                {
                    shopStocks.Find(x => x.card.card_id.Equals(card_id)).sold = true;
                }
            }
        }

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

                    if (!card_buys_id.Contains(fetched.card.card_id)) card_buys_id.Add(fetched.card.card_id);
                }

                return status;
            } else GGDebug.Console($"Buy Failed! Card {selectedCard.card_name} is not exist on stock.");

            return false;
        }

        public bool IsBusy()
        {
            return isBusy;
        }

        public void BuyHealPotion()
        {
            var health_full = player.CharacterData.CurrentHealthPoint == player.CharacterData.BaseHealthPoint;
            if (health_full) return;

            var status = player.RemoveCoin(heal_potion_price);

            if (status)
            {
                player.UpdateHealth(heal_potion_value);
            }
        }

        public void RenderDeckToRemove()
        {
            shop_ui.CloseShop();
            selected_card.Clear();
            var decks = deck.GetAllDecks;
            shop_ui.RenderShop_RemoveCard(decks);
            shop_ui.ActivateApplyRemoveButton(false);
            shop_ui.UpdatePriceTotalRemoveCard(player.CollectibleData.coin, 0);
        }

        public void SelectCardToRemovedList(CardData card)
        {
            selected_card.Add(card);
            shop_ui.ActivateApplyRemoveButton(selected_card.Count > 0);
            shop_ui.UpdatePriceTotalRemoveCard(player.CollectibleData.coin, remove_card_price * selected_card.Count);
        }

        public void DeselectCardFromRemovedList(CardData card)
        {
            selected_card.Remove(card);
            shop_ui.ActivateApplyRemoveButton(selected_card.Count > 0);
            shop_ui.UpdatePriceTotalRemoveCard(player.CollectibleData.coin, remove_card_price * selected_card.Count);
        }

        public void AcceptToRemoveCard()
        {
            var status = player.RemoveCoin(remove_card_price * selected_card.Count);
            if (status)
            {
                deck.RemoveCards(selected_card);
                RenderDeckToRemove();
                shop_ui.UpdatePriceTotalRemoveCard(player.CollectibleData.coin, remove_card_price * selected_card.Count);
            }
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