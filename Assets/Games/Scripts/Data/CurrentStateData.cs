using System.Collections.Generic;
using UnityEngine;

namespace GuraGames.Data
{
    [System.Serializable]
    public class CurrentStateData
    {
        public string level_id;
        public int current_sublevel_id;
        public List<int> sublevel_id = new List<int>();
        public Vector2 position;
        public int health;
        public int coin;
        public List<string> card_metadata = new List<string>();
        public List<string> shop_card = new List<string>();

        public CurrentStateData()
        {
            level_id = string.Empty;
            current_sublevel_id = 0;
            sublevel_id = new List<int>();
            position = Vector2.zero;
            health = 30;
            coin = 0;
            card_metadata = new List<string>();
            shop_card = new List<string>();
        }

        public CurrentStateData(CurrentStateData copyData)
        {
            level_id = copyData.level_id;
            current_sublevel_id = copyData.current_sublevel_id;
            sublevel_id.AddRange(copyData.sublevel_id);
            position = copyData.position;
            health = copyData.health;
            coin = copyData.coin;
            card_metadata.AddRange(copyData.card_metadata);
            shop_card.AddRange(copyData.shop_card);
        }
    }
}