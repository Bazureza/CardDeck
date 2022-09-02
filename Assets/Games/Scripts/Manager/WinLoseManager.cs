using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GuraGames.Manager
{
    public class WinLoseManager : BaseManager
    {
        [SerializeField] private GameObject winPanel;
        [SerializeField] private GameObject losePanel;

        public void GameWin()
        {
            winPanel.SetActive(true);
        }

        public void GameLose()
        {
            losePanel.SetActive(true);
        }
    }
}