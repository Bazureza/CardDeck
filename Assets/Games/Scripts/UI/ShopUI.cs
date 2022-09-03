using GuraGames.GameSystem;
using GuraGames.Manager;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using TomGustin.GameDesignPattern;
using UnityEngine;
using UnityEngine.UI;
using static GuraGames.Manager.ShopManager;

namespace GuraGames.UI
{
    public class ShopUI : MonoBehaviour
    {
        [Header("Shop Properties")]
        [SerializeField] private GameObject shopPanel;
        [SerializeField] private GameObject buyCardPrefabs;
        [SerializeField] private Transform buyCardParent;
        [SerializeField] private TextMeshProUGUI priceHealPotion;
        [SerializeField] private TextMeshProUGUI valueHealPotion;

        [Header("Deck Properties")]
        [SerializeField] private GameObject deckPanel;
        [SerializeField] private GameObject deckCardPrefabs;
        [SerializeField] private RectTransform deckParent;
        [SerializeField] private Button applyRemoveButton;
        [SerializeField] private TextMeshProUGUI currentCoin;
        [SerializeField] private TextMeshProUGUI priceTotalRemoveCard;

        private List<ShopUI_BuyCard> buyCards = new List<ShopUI_BuyCard>();
        private List<ShopUI_RemoveCard> removeCards = new List<ShopUI_RemoveCard>();

        private ShopManager _shop;
        private ShopManager shop
        {
            get
            {
                if (!_shop) _shop = ServiceLocator.Resolve<ShopManager>();
                return _shop;
            }
        }

        public void RenderShop_BuyCard(List<ShopStock> shopStocks)
        {
            MouseInputSystem.Active = false;
            shopPanel.SetActive(true);
            var length = shopStocks.Count;
            var spawnedLength = buyCardParent.childCount;

            valueHealPotion.text = $"+{shop.HealPotion.Value}";
            priceHealPotion.text = $"Price: {shop.HealPotion.Price}";
            
            for (int i = 0; i < length; i++)
            {
                if (i < spawnedLength)
                {
                    buyCards[i].gameObject.SetActive(true);
                    buyCards[i].InitData(shopStocks[i].card, shopStocks[i].price, shopStocks[i].sold);
                } else
                {
                    var spawnedObject = buyCardPrefabs.Spawn(buyCardParent);
                    spawnedObject.transform.localScale = Vector3.one;
                    var buyCardUI = spawnedObject.GetComponent<ShopUI_BuyCard>();
                    buyCardUI.InitData(shopStocks[i].card, shopStocks[i].price, shopStocks[i].sold);
                    buyCards.Add(buyCardUI);
                }
            }
        }

        public void CloseShop()
        {
            MouseInputSystem.Active = true;
            shopPanel.SetActive(false);
        }

        public void RenderShop_RemoveCard(List<CardData> cards)
        {
            deckPanel.SetActive(true);
            var length = cards.Count;
            var spawnedLength = deckParent.childCount;
            var max_length = Mathf.Max(length, spawnedLength);

            int offset = length - 5;

            if (offset > 0)
            {
                deckParent.sizeDelta = new Vector2(180f * offset, deckParent.sizeDelta.y);
            } else deckParent.sizeDelta = new Vector2(0f * offset, deckParent.sizeDelta.y);

            for (int i = 0; i < max_length; i++)
            {
                if (i < spawnedLength)
                {
                    if (i < length)
                    {
                        removeCards[i].gameObject.SetActive(true);
                        removeCards[i].InitData(cards[i]);
                    } else removeCards[i].gameObject.SetActive(false);
                }
                else
                {
                    if (i < length)
                    {
                        var spawnedObject = deckCardPrefabs.Spawn(deckParent);
                        spawnedObject.transform.localScale = Vector3.one;
                        var removeCardUI = spawnedObject.GetComponent<ShopUI_RemoveCard>();
                        removeCardUI.InitData(cards[i]);
                        removeCards.Add(removeCardUI);
                    } else removeCards[i].gameObject.SetActive(false);
                }
            }
        }

        public void UpdatePriceTotalRemoveCard(int current_coin, int expense_price)
        {
            string color_code = "black";

            if (current_coin >= expense_price) color_code = "black";
            else color_code = "red";

            if (expense_price == 0) color_code = "black";

            currentCoin.text = $"Current Coin: {current_coin}";
            priceTotalRemoveCard.text = $"Price: <color={color_code}>{expense_price}</color>";
        }

        public void ActivateApplyRemoveButton(bool active)
        {
            applyRemoveButton.interactable = active;
        }

        public void CloseRemovePanel()
        {
            deckPanel.SetActive(false);
            shopPanel.SetActive(true);
        }
    }
}