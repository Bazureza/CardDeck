using GuraGames.Character;
using GuraGames.GameSystem;
using GuraGames.UI;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TomGustin.GameDesignPattern;
using UnityEngine;
using UnityEngine.Events;
using static GuraGames.Level.SubLevelData;

namespace GuraGames.Level
{
    public class LevelDataManager : MonoBehaviour
    {
        [SerializeField] private string level_id;
        [SerializeField] private int start_level;
        [SerializeField] private List<SubLevelData> subLevels = new List<SubLevelData>();
        [SerializeField] private UnityEvent onClearEnemy;

        [SerializeField, ReadOnly] private int current_subLevel;

        private IndicatorMovesUI indicator_ui;

        private void Awake()
        {
            current_subLevel = start_level;
            indicator_ui = ServiceLocator.Resolve<IndicatorMovesUI>();

            indicator_ui.ResetIndicatorGlobal();
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
            CheckEnemyClearanceActiveSubLevel();
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

        public bool IsClearActiveSubLevel()
        {
            var enemies = GetActiveSubLevelEnemies();
            return enemies == null || enemies.Count == 0;
        }

        public void CheckEnemyClearanceActiveSubLevel()
        {
            if (IsClearActiveSubLevel())
            {
                subLevels[current_subLevel].OnClearEnemy(onClearEnemy);
                
                ConnectionData connection = subLevels[current_subLevel].connection;
                indicator_ui.RenderIndicatorMoveGlobal((connection.up, connection.right, connection.down, connection.left));
                GGDebug.Console($"Connection available on {subLevels[current_subLevel].name} is " +
                    $"{(connection.up ? "Up" : string.Empty)}  " +
                    $"{(connection.right ? "Right" : string.Empty)}  " +
                    $"{(connection.down ? "Down" : string.Empty)}  " +
                    $"{(connection.left ? "Left" : string.Empty)}");
            } else
            {
                indicator_ui.ResetIndicatorGlobal();
            }
        }
    }
}