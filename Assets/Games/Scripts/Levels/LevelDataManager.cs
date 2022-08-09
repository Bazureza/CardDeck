using GuraGames.Character;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GuraGames.Level
{
    public class LevelDataManager : MonoBehaviour
    {
        [SerializeField] private int start_level;
        [SerializeField] private List<SubLevelData> subLevels = new List<SubLevelData>();

        [SerializeField, ReadOnly] private int current_subLevel;

        private void Awake()
        {
            current_subLevel = start_level;
        }

        [Button]
        private void SyncData()
        {
            foreach (SubLevelData subLevelData in subLevels)
            {
                subLevelData.ID = subLevels.IndexOf(subLevelData);
            }
        }

        public void ChangeActiveSubLevel(int id)
        {
            current_subLevel = id;
        }

        public SubLevelData GetSubLevelData(int id)
        {
            if (id < subLevels.Count) return subLevels[id];
            return null;
        }

        public SubLevelData GetActiveSubLevelData()
        {
            return subLevels[current_subLevel];
        }

        public List<BaseCharacterSystem> GetActiveSubLevelEnemies()
        {
            return subLevels[current_subLevel].GetEnemiesOnSubLevel();
        }
    }
}