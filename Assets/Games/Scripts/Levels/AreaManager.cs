using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GuraGames.Level
{
    public class AreaManager : MonoBehaviour
    {
        [SerializeField] private Transform parentSubArea;
        [SerializeField] private List<SubLevelData> subLevels = new List<SubLevelData>();

        [Header("Debug Properties")]
        [SerializeField] private bool drawGizmos;
        [SerializeField] private float areaSize;
        [SerializeField, Range(0f,1f)] private float opacity;

        public List<SubLevelData> GetSubLevel()
        {
            return subLevels;
        }

        [Button]
        private void SyncArea()
        {
            subLevels.Clear();
            for (int i = 0; i < parentSubArea.childCount; i++)
            {
                var area = parentSubArea.GetChild(i);
                area.name = $"[{i.ToString("00")}] SubArea";
                subLevels.Add(area.GetComponent<SubLevelData>());
            }
        }

        private void OnDrawGizmos()
        {
            if (parentSubArea == null || !drawGizmos) return;
            for (int i = 0; i < parentSubArea.childCount; i ++) DrawSubLevel(i);
        }

        private void DrawSubLevel(int index)
        {
            var value = index/(parentSubArea.childCount - 1f);
            Gizmos.color = new Color(value, value, value, opacity);
            Gizmos.DrawCube(parentSubArea.GetChild(index).position, new Vector3(areaSize, areaSize, 1));
        }
    }
}