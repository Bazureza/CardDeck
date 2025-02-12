﻿using TMPro;
using UnityEngine;

namespace GuraGames.UI
{
    public class StatUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI move_value;
        [SerializeField] private TextMeshProUGUI mana_value;
        [SerializeField] private TextMeshProUGUI coin_value;

        public void UpdateMove(int current_move)
        {
            if (move_value) move_value.text = $"{current_move}";
        }

        public void UpdateMana(int current_mana)
        {
            if (mana_value) mana_value.text = $"{current_mana}";
        }

        public void UpdateCoin(int current_coin)
        {
            if (coin_value) coin_value.text = $"{current_coin}";
        }
    }
}