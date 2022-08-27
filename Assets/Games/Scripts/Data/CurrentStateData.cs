using System.Collections.Generic;
using UnityEngine;

namespace GuraGames.Data
{
    public class CurrentStateData
    {
        public string level_id;
        public Vector2 position;
        public int health;
        public int coin;
        public List<string> card_metadata = new List<string>();

        public CurrentStateData()
        {
            position = Vector2.zero;
            coin = 0;
            card_metadata = new List<string>();
        }
    }
}