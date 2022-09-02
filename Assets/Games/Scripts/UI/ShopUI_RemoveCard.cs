using GuraGames.Manager;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using TomGustin.GameDesignPattern;
using UnityEngine;
using UnityEngine.UI;

namespace GuraGames.UI
{
    public class ShopUI_RemoveCard : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI name_card;
        [SerializeField] private Image icon_card;
        [SerializeField] private TextMeshProUGUI mana_card;
        [SerializeField] private GameObject selectLayer;

        private CardData card;
        private bool selected;

        private ShopManager _shop_m;
        private ShopManager shop_m
        {
            get
            {
                if (!_shop_m) _shop_m = ServiceLocator.Resolve<ShopManager>();
                return _shop_m;
            }
        }

        public void InitData(CardData card)
        {
            this.card = card;
            selected = false;

            if (card) UpdateUI();
        }

        private void UpdateUI()
        {
            name_card.text = card.card_name;
            icon_card.sprite = card.card_icon;
            mana_card.text = card.mana_consume.ToString();
            selectLayer.SetActive(selected);
        }

        public void OnClick()
        {
            selected = !selected;

            if (selected) shop_m.SelectCardToRemovedList(card);
            else shop_m.DeselectCardFromRemovedList(card);
            selectLayer.SetActive(selected);
        }
    }
}