using GuraGames.Character;
using GuraGames.GameSystem;
using GuraGames.Manager;
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
        [SerializeField] private AreaManager area;
        [SerializeField] private UnityEvent onClearEnemy;

        [SerializeField, ReadOnly] private int current_subLevel;

        private List<SubLevelData> subLevels = new List<SubLevelData>();
        private List<int> cleared_sublevel = new List<int>(); 

        private AutoSaveManager _autosave;
        private AutoSaveManager autosave
        {
            get
            {
                if (!_autosave) _autosave = ServiceLocator.Resolve<AutoSaveManager>();
                return _autosave;
            }
        }

        private IndicatorMovesUI _indicator_ui;
        private IndicatorMovesUI indicator_ui
        {
            get
            {
                if (!_indicator_ui) _indicator_ui = ServiceLocator.Resolve<IndicatorMovesUI>();
                return _indicator_ui;
            }
        }

        private TurnBasedManager _tbm;
        private TurnBasedManager tbm
        {
            get
            {
                if (!_tbm) _tbm = ServiceLocator.Resolve<TurnBasedManager>();
                return _tbm;
            }
        }

        [Button]
        private void SyncData()
        {
            foreach (SubLevelData subLevelData in subLevels)
            {
                subLevelData.ID = subLevels.IndexOf(subLevelData);
            }
        }

        public void DefaultInit()
        {
            subLevels = area.GetSubLevel();
            current_subLevel = start_level;
        }

        public void InitDataLevel(List<int> cleared_sublevel)
        {
            subLevels = area.GetSubLevel();

            this.cleared_sublevel.Clear();
            this.cleared_sublevel.AddRange(cleared_sublevel);

            foreach (int sublevel_id in cleared_sublevel)
            {
                subLevels[sublevel_id].RemoveAllEnemy();
            }
        }

        public void UpdateIndicatorGlobal()
        {
            ConnectionData connection = subLevels[current_subLevel].connection;
            indicator_ui.RenderIndicatorMoveGlobal((connection.up, connection.right, connection.down, connection.left));
        }

        public void ChangeActiveSubLevel(int id, bool autosave = true)
        {
            current_subLevel = id;
            CheckEnemyClearanceActiveSubLevel(autosave);
            tbm.UpdateTurnQueue();
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

        public void CheckEnemyClearanceActiveSubLevel(bool autosave = true)
        {
            if (IsClearActiveSubLevel())
            {
                subLevels[current_subLevel].OnClearEnemy(onClearEnemy);
                if (!cleared_sublevel.Contains(current_subLevel)) cleared_sublevel.Add(current_subLevel);
                
                ConnectionData connection = subLevels[current_subLevel].connection;
                indicator_ui.RenderIndicatorMoveGlobal((connection.up, connection.right, connection.down, connection.left));
                GGDebug.Console($"Connection available on {subLevels[current_subLevel].name} is " +
                    $"{(connection.up ? "Up" : string.Empty)}  " +
                    $"{(connection.right ? "Right" : string.Empty)}  " +
                    $"{(connection.down ? "Down" : string.Empty)}  " +
                    $"{(connection.left ? "Left" : string.Empty)}");

                if (autosave) this.autosave.AutoSave();
            } else
            {
                indicator_ui.ResetIndicatorGlobal();
            }
        }

        public List<int> GetClearedSubLevel()
        {
            return cleared_sublevel;
        }

        public int GetCurrentSubLevelID()
        {
            return current_subLevel;
        }
    }
}