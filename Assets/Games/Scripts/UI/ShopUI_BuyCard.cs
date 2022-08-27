using GuraGames.Manager;
using TMPro;
using TomGustin.GameDesignPattern;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GuraGames.UI
{
    public class ShopUI_BuyCard : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI name_card;
        [SerializeField] private Image icon_card;
        [SerializeField] private TextMeshProUGUI mana_card;
        [SerializeField] private TextMeshProUGUI price_card;
        [SerializeField] private GameObject soldLayer;

        private CardData card;
        private int price;
        private bool sold;

        private ShopManager _shop_m;
        private ShopManager shop_m
        {
            get
            {
                if (!_shop_m) _shop_m = ServiceLocator.Resolve<ShopManager>();
                return _shop_m;
            }
        }

        public void InitData(CardData card, int price, bool sold)
        {
            this.card = card;
            this.price = price;
            this.sold = sold;

            if (card) UpdateUI();
        }

        private void UpdateUI()
        {
            name_card.text = card.card_name;
            icon_card.sprite = card.card_icon;
            mana_card.text = card.mana_consume.ToString();
            price_card.text = price.ToString();
        }

        public void OnClick()
        {
            if (sold) return;
            var buys = shop_m.SelectCard(card);
            sold = buys;
            soldLayer.SetActive(sold);
        }
    }
}