using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GuraGames.Manager.ShopManager;

namespace GuraGames.UI
{
    public class ShopUI : MonoBehaviour
    {
        [SerializeField] private GameObject shopPanel;
        [SerializeField] private GameObject buyCardPrefabs;
        [SerializeField] private Transform buyCardParent;

        private List<ShopUI_BuyCard> buyCards = new List<ShopUI_BuyCard>();

        public void RenderShop_BuyCard(List<ShopStock> shopStocks)
        {
            shopPanel.SetActive(true);
            var length = shopStocks.Count;
            var spawnedLength = buyCardParent.childCount;
            
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
            shopPanel.SetActive(false);
        }

    }
}