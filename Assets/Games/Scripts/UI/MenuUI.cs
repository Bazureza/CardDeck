using GuraGames.GameSystem;
using GuraGames.Manager;
using System.Collections;
using System.Collections.Generic;
using TomGustin.GameDesignPattern;
using UnityEngine;
using UnityEngine.UI;

namespace GuraGames.UI
{
    public class MenuUI : MonoBehaviour
    {
        [Header("General Properties")]
        [SerializeField] private GameObject menuPanel;
        [SerializeField] private GameObject[] subPanels;

        [Header("Deck Properties")]
        [SerializeField] private Transform deckParent;
        [SerializeField] private GameObject deckCardPrefabs;
        [SerializeField] private GridLayoutGroup gridDeck;
        [SerializeField] private RectTransform contentRect;

        [Header("Setting Properties")]
        [SerializeField] private Slider bgmSliderbar;
        [SerializeField] private Slider sfxSliderbar;

        private List<CardUI> cards = new List<CardUI>();

        private DeckManager _deck;
        private DeckManager deck
        {
            get
            {
                if (!_deck) _deck = ServiceLocator.Resolve<DeckManager>();
                return _deck;
            }
        }

        public void MainMenu()
        {
            SceneSystem.LoadScene("MENU");
        }

        public void OpenMenu(int index)
        {
            ResetPanel();
            
            menuPanel.SetActive(true);
            subPanels[index].SetActive(true);
            MouseInputSystem.Active = false;

            switch (index)
            {
                case 0:
                    OpenDeckList();
                    break;
                case 2:
                    UpdateSetting();
                    break;
            }
        }

        public void CloseMenu()
        {
            ResetPanel();

            menuPanel.SetActive(false);
            MouseInputSystem.Active = true;
        }

        public void SetBGMVolume(float volume) { AudioSystem.BGMVolume = volume; }
        public void SetSFXVolume(float volume) { AudioSystem.SFXVolume = volume; }

        private void OpenDeckList()
        {
            var decks = deck.GetAllDecks;
            int deckCount = decks.Count;
            int uiDeckCount = cards.Count;

            int row_count = (deckCount / 6) + 1;
            float height_calculated = row_count * 160f + ((row_count -  1) * gridDeck.spacing.y);

            int length_max = Mathf.Max(deckCount, uiDeckCount);

            for (int i = 0; i < length_max; i++)
            {
                if (i < uiDeckCount)
                {
                    if (i < deckCount) cards[i].InitCard(decks[i]);
                    else cards[i].gameObject.SetActive(false);
                } else
                {
                    var spawnedCard = deckCardPrefabs.Spawn(deckParent);
                    spawnedCard.transform.localScale = Vector3.one;
                    CardUI card = spawnedCard.GetComponent<CardUI>();
                    card.InitCard(decks[i]);
                    cards.Add(card);
                }
            }

            contentRect.sizeDelta = new Vector2(contentRect.sizeDelta.x, height_calculated);
        }

        private void ResetPanel()
        {
            foreach (GameObject subPanel in subPanels)
            {
                subPanel.SetActive(false);
            }
        }

        private void UpdateSetting()
        {
            bgmSliderbar.value = AudioSystem.BGMVolume;
            sfxSliderbar.value = AudioSystem.SFXVolume;
        }
    }
}