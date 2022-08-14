using GuraGames.Manager;
using TMPro;
using TomGustin.GameDesignPattern;
using UnityEngine;
using UnityEngine.UI;

namespace GuraGames.UI
{
    public class CardUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI mana_text;
        [SerializeField] private TextMeshProUGUI card_name_text;
        [SerializeField] private Image icon_card;

        private DeckManager _deck_m;
        private DeckManager deck_m
        {
            get
            {
                if (!_deck_m) _deck_m = ServiceLocator.Resolve<DeckManager>();
                return _deck_m;
            }
        }

        private CardData card;

        public void InitCard(CardData card)
        {
            this.card = card;

            Render();
        }

        public void Render()
        {
            mana_text.text = card.mana_consume.ToString();
            card_name_text.text = card.card_name;
            icon_card.sprite = card.card_icon;
        }

        public void UseCard()
        {
            deck_m.ShowCardInfo(card);
        }
    }
}