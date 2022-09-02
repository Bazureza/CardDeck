using GuraGames.GameSystem;
using GuraGames.Manager;
using System.Collections;
using System.Collections.Generic;
using TGC.MDS;
using UnityEngine;
using UnityEngine.UI;

namespace GuraGames.UI
{
    public class MainMenuManager : BaseManager
    {
        [SerializeField] private Button continueButton;
        [SerializeField] private GameObject mainPanel;
        [SerializeField] private GameObject settingPanel;

        [Header("Setting Properties")]
        [SerializeField] private Slider bgmSlider;
        [SerializeField] private Slider sfxSlider;

        protected override void OnAwake()
        {
            base.OnAwake();

            continueButton.interactable = MDSSaveSystem.IsSaveDataAvailable("current_data", out string full_path);

            SceneSystem.ReadytoLoad();
        }

        public void OpenSetting(bool open)
        {
            mainPanel.SetActive(!open);
            settingPanel.SetActive(open);

            if (open) UpdateSetting();
        }

        public void StartNewGame()
        {
            MDSSaveSystem.DeleteAllSaveData("current_data");
            SceneSystem.LoadScene("GAME");
        }

        public void ContinueGame()
        {
            SceneSystem.LoadScene("GAME");
        }

        public void ExitGame()
        {
            Application.Quit();
        }

        private void UpdateSetting()
        {
            bgmSlider.value = AudioSystem.BGMVolume;
            sfxSlider.value = AudioSystem.SFXVolume;
        }

        public void SetBGMVolume(float volume) { AudioSystem.BGMVolume = volume; }
        public void SetSFXVolume(float volume) { AudioSystem.SFXVolume = volume; }
    }
}