using GuraGames.Enums;
using GuraGames.GameSystem;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GuraGames.AI
{
    public class AIDecide : MonoBehaviour
    {
        [SerializeField] private List<CardData> actions;

        private List<CardData> fetched_action = new List<CardData>();

        public CardData DecideAction()
        {
            if (fetched_action.Count > 0)
            {
                int roll_number = Random.Range(0, fetched_action.Count);
                return fetched_action[roll_number];
            }

            return null;
        }

        public void RefreshFetchedAction()
        {
            fetched_action.Clear();
        }

        public void FetchedActionBasedOnCondition(ActionType action_type)
        {
            var fetch = actions.FirstOrDefault((x)=> x.action_type.Equals(action_type));
            if (fetch) fetched_action.Add(fetch);
        }
    }
}