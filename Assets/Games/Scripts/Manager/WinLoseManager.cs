using System.Collections;
using System.Collections.Generic;
using TomGustin.GameDesignPattern;
using UnityEngine;

namespace GuraGames.Manager
{
    public class WinLoseManager : BaseManager
    {
        [SerializeField] private GameObject winPanel;
        [SerializeField] private GameObject losePanel;

        private AutoSaveManager _autosave;
        private AutoSaveManager autosave
        {
            get
            {
                if (!_autosave) _autosave = ServiceLocator.Resolve<AutoSaveManager>();
                return _autosave;
            }
        }

        public void GameWin()
        {
            winPanel.SetActive(true);
        }

        public void GameLose()
        {
            autosave.RestoreData();
            losePanel.SetActive(true);
        }
    }
}