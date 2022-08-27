using GuraGames.UI;
using System.Collections;
using System.Collections.Generic;
using TomGustin.GameDesignPattern;
using UnityEngine;

namespace GuraGames.Manager
{
    public class MapManager : MonoBehaviour
    {

        private MenuUI _m_ui;
        private MenuUI m_ui
        {
            get
            {
                if (!_m_ui) _m_ui = ServiceLocator.Resolve<MenuUI>();
                return _m_ui;
            }
        }

        public void OpenMap(bool open)
        {
            
        }
    }
}