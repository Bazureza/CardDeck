using GuraGames.GameSystem;
using GuraGames.Manager;
using TMPro;
using TomGustin.GameDesignPattern;
using UnityEngine;
using UnityEngine.UI;

namespace GuraGames.UI
{
    public class DeckUI : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameObject prefabs;
        [SerializeField] private Transform handCardParent;
        [SerializeField] private Image previewImageCard;
        [SerializeField] private TextMeshProUGUI previewManaCardText;
        [SerializeField] private TextMeshProUGUI previewNameCardText;
        [SerializeField] private TextMeshProUGUI previewDetailCardText;

        [SerializeField] private TextMeshProUGUI deckCount;
        [SerializeField] private TextMeshProUGUI graveCount;

        private DeckManager _deck_m;
        private DeckManager deck_m
        {
            get
            {
                if (!_deck_m) _deck_m = ServiceLocator.Resolve<DeckManager>();
                return _deck_m;
            }
        }

        public void UpdateHandCard()
        {
            int target_spawned = deck_m.GetHandCardCount;
            int spawned_card = handCardParent.childCount;
            int deck_count = deck_m.GetHandDecks.Count;

            for (int i = 0; i < target_spawned; i++)
            {
                if (i < spawned_card)
                {
                    if (i < deck_count)
                    {
                        handCardParent.GetChild(i).gameObject.SetActive(true);
                        CardUI cardUI = handCardParent.GetChild(i).GetComponent<CardUI>();
                        cardUI.InitCard(deck_m.GetHandDecks[i]);
                    }
                    else
                    {
                        handCardParent.GetChild(i).gameObject.SetActive(false);
                        GGDebug.Console($"Disable Card Index: {i}");
                    }
                } else
                {
                    if (i < deck_count)
                    {
                        GameObject card = prefabs.Spawn(handCardParent);
                        card.transform.localScale = Vector3.one;
                        CardUI cardUI = card.GetComponent<CardUI>();

                        cardUI.InitCard(deck_m.GetHandDecks[i]);
                    }
                }
            }
        }

        public void ShowHandDecks(bool show)
        {
            handCardParent.gameObject.SetActive(show);
        }

        public void UpdatePreview(CardData card)
        {
            previewImageCard.enabled = card;
            if (card)
            {
                previewManaCardText.text = card.mana_consume.ToString();
                previewImageCard.sprite = card.card_icon;
                previewNameCardText.text = card.card_name;
                previewDetailCardText.text = card.card_desc;
            } else
            {
                previewManaCardText.text = string.Empty;
                previewImageCard.sprite = null;
                previewNameCardText.text = string.Empty;
                previewDetailCardText.text = string.Empty;
            }
        }

        public void UpdateDeck(int count)
        {
            deckCount.text = count.ToString();
        }

        public void UpdateGrave(int count)
        {
            graveCount.text = count.ToString();
        }
    }
}