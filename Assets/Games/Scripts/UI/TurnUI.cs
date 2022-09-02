using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GuraGames.UI
{
    public class TurnUI : MonoBehaviour
    {
        [SerializeField] private Transform parentIcons;

        [SerializeField] private GameObject playerIconUI;
        [SerializeField] private GameObject[] enemiesIconUI;

        public void UpdateLengthQueueUI(List<Enums.CharacterType> queue)
        {
            var length = queue.Count - 1;

            for (int i = 0; i < enemiesIconUI.Length; i++)
            {
                enemiesIconUI[i].SetActive(i < length);
            }
        }

        public void UpdateQueueUI(List<Enums.CharacterType> queue)
        {
            var index = queue.IndexOf(Enums.CharacterType.Player);

            playerIconUI.transform.SetSiblingIndex(index);
        }
    }
}