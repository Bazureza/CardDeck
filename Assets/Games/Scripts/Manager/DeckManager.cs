using GuraGames.Character;
using GuraGames.GameSystem;
using GuraGames.UI;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TomGustin.GameDesignPattern;
using UnityEngine;

namespace GuraGames.Manager
{
    public class DeckManager : BaseManager
    {
        [Header("Properties")]
        [SerializeField] private List<CardData> actions;
        [SerializeField] private int handCardCount;

        [Header("Runtime Status")]
        [SerializeField, ReadOnly] private List<CardData> onDeck;
        [SerializeField, ReadOnly] private List<CardData> onHand;
        [SerializeField, ReadOnly] private List<CardData> onGrave;

        private CardData currentUsedCard;

        public int GetHandCardCount { get { return handCardCount; } }
        public List<CardData> GetHomeDecks { get { return onDeck; } }
        public List<CardData> GetHandDecks { get { return onHand; } }
        public List<CardData> GetGraveDecks { get { return onGrave; } }
        public List<CardData> GetAllDecks
        {
            get 
            {
                var merged_list = new List<CardData>();
                merged_list.AddRange(onDeck);
                merged_list.AddRange(onHand);
                merged_list.AddRange(onGrave);
                return merged_list;
            }
        }
        public CardData CurrentUsedCard { get { return currentUsedCard; } }

        private DeckUI _deck_ui;
        private DeckUI deck_ui
        {
            get
            {
                if (!_deck_ui) _deck_ui = ServiceLocator.Resolve<DeckUI>();
                return _deck_ui;
            }
        }

        private TurnBasedManager _tbm;
        private TurnBasedManager tbm
        {
            get
            {
                if (!_tbm) _tbm = ServiceLocator.Resolve<TurnBasedManager>();
                return _tbm;
            }
        }

        private PlayerCharacterSystem _pcs;
        private PlayerCharacterSystem pcs 
        {
            get
            {
                if (!_pcs) _pcs = ServiceLocator.Resolve<PlayerCharacterSystem>();
                return _pcs;
            }
        }

        #region External Deck Functions
        public void AddCardToDeck(CardData card)
        {
            onDeck.Add(card);
            deck_ui.UpdateDeck(onDeck.Count);
        }

        public void RemoveCardFromDeck(CardData card)
        {
            onDeck.Remove(card);
            deck_ui.UpdateDeck(onDeck.Count);
        }

        public void RemoveCards(List<CardData> cards)
        {
            foreach (CardData card in cards)
            {
                onDeck.Remove(card);
                //onHand.Remove(card);
                //onGrave.Remove(card);
            }

            deck_ui.UpdateDeck(onDeck.Count);
        }

        public void ShowHandDecks(bool show)
        {
            deck_ui.ShowHandDecks(show);
        }

        public void UpdateHandCard()
        {
            deck_ui.UpdateHandCard();
        }

        [Button]
        public void DefaultInit()
        {
            InitiateDeck();

            deck_ui.UpdateDeck(onDeck.Count);
            deck_ui.UpdateGrave(onGrave.Count);
        }

        public void InitDataDeck(List<string> deck_data)
        {
            onDeck = new List<CardData>();
            foreach (string card in deck_data)
            {
                if (actions.Exists(x => x.card_id.Equals(card)))
                {
                    var fetched = actions.Find(x => x.card_id.Equals(card));
                    onDeck.Add(fetched);
                }
            }

            ShuffleDeck();
            deck_ui.UpdateDeck(onDeck.Count);
            deck_ui.UpdateGrave(onGrave.Count);
        }

        [Button]
        public void FillHandCard(bool show = true)
        {
            int count_card = handCardCount - onHand.Count;
            if (count_card.Equals(0)) return;

            List<CardData> cards;
            
            if (onDeck.Count >= count_card) {
                cards = onDeck.GetRange(0, count_card);
                onDeck.RemoveRange(0, count_card);
            }
            else {
                count_card = onDeck.Count;
                cards = onDeck.GetRange(0, count_card);
                onDeck.RemoveRange(0, count_card);

                RenewDeck();

                count_card = (handCardCount - onHand.Count) - count_card;

                if (onDeck.Count >= count_card)
                {
                    cards.AddRange(onDeck.GetRange(0, count_card));
                    onDeck.RemoveRange(0, count_card);
                } else
                {
                    count_card = onDeck.Count;
                    cards.AddRange(onDeck.GetRange(0, count_card));
                    onDeck.RemoveRange(0, count_card);
                }
            }

            onHand.AddRange(cards);

            if (show) UpdateHandCard();

            deck_ui.UpdateDeck(onDeck.Count);
            deck_ui.UpdateGrave(onGrave.Count);
        }

        public void UpdatePreview(CardData card)
        {
            deck_ui.UpdatePreview(card);
        }

        public void ShowCardInfo(CardData card)
        {
            currentUsedCard = card;

            deck_ui.UpdatePreview(currentUsedCard);
        }

        public void BringToGrave()
        {
            onGrave.AddRange(onHand);
            onHand.Clear();
            deck_ui.UpdateHandCard();
            deck_ui.UpdateGrave(onGrave.Count);
        }

        public void BringToDeck()
        {
            if (onHand.Count > 0) BringToGrave();
            if (onGrave.Count > 0)
            {
                onDeck.AddRange(onGrave);
                onGrave.Clear();
                deck_ui.UpdateGrave(onGrave.Count);
                deck_ui.UpdateDeck(onDeck.Count);
                onDeck.Shuffle();
            }
        }

        public void UseCard()
        {
            if (!currentUsedCard) return;
            if (pcs.IsSufficientMana(currentUsedCard.mana_consume))
            {

                Console($"Using {currentUsedCard.card_name}");
                if (onHand.Contains(currentUsedCard) && pcs)
                {
                    onHand.Remove(currentUsedCard);
                    onGrave.Add(currentUsedCard);

                    UpdateHandCard();
                    pcs.OnActionCard(currentUsedCard);
                    deck_ui.UpdateGrave(onGrave.Count);

                    if (onHand.Count == 0) FillHandCard(true);
                }
                else
                {
                    Console($"Card failed activate: {currentUsedCard.card_name} because of some null value", Enums.DebugType.Warning);
                }
            } else
            {
                Console($"Card failed activate: {currentUsedCard.card_name} because of not enough mana", Enums.DebugType.Warning);
            }
        }

        [Button]
        public void RenewDeck()
        {
            //ShuffleGrave();
            onDeck.AddRange(onGrave);
            onGrave.Clear();
            ShuffleDeck();
        }

        public void ReleaseUsedCard()
        {
            currentUsedCard = null;
            UpdatePreview(currentUsedCard);
        }
        #endregion

        #region Internal Deck Functions
        private void InitiateDeck()
        {
            onDeck = new List<CardData>();
            foreach (CardData card in actions)
            {
                onDeck.Add(card);
            }

            ShuffleDeck();
        }

        [Button]
        private void ShuffleDeck()
        {
            onDeck.Shuffle();
        }

        [Button]
        private void ShuffleGrave()
        {
            onGrave.Shuffle();
        }

/*        [Button]
        private void TestUseCard()
        {
            var card_count = 2;
            if (onHand.Count >= card_count)
            {
                onGrave.AddRange(onHand.GetRange(0, card_count));
                onHand.RemoveRange(0, card_count);
                UpdateHandCard();
            }
        }*/
        #endregion
    }
}