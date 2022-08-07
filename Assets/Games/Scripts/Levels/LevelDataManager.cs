using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GuraGames.Level
{
    public class LevelDataManager : MonoBehaviour
    {
        [SerializeField] private List<SubLevelData> subLevels = new List<SubLevelData>();

        [Button]
        private void SyncData()
        {
            foreach (SubLevelData subLevelData in subLevels)
            {
                subLevelData.ID = subLevels.IndexOf(subLevelData);
            }
        }

        public SubLevelData GetSubLevelData(int id)
        {
            if (id < subLevels.Count) return subLevels[id];
            return null;
        }
    }
}