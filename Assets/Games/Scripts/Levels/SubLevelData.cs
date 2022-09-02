using GuraGames.Character;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GuraGames.Level
{
    public class SubLevelData : MonoBehaviour
    {
        [SerializeField] private int id;
        [SerializeField] private List<BaseCharacterSystem> enemies;
        [SerializeField] private UnityEvent onClearSubLevel;

        public ConnectionData connection;

        private bool isClear;
        
        public int ID 
        {
            get { return id; }
            set { id = value; }
        }

        public Vector3 GetLevelPosition()
        {
            return transform.position;
        }

        public List<BaseCharacterSystem> GetEnemiesOnSubLevel()
        {
            return enemies;
        }

        public void RemoveEnemy(BaseCharacterSystem enemy)
        {
            enemies.Remove(enemy);
            enemy.gameObject.Recycle();
        }

        public void RemoveAllEnemy()
        {
            foreach (BaseCharacterSystem enemy in enemies)
            {
                enemy.gameObject.Recycle();
            }

            enemies.Clear();
        }

        public bool IsEnemiesClear()
        {
            return enemies == null || enemies.Count == 0;
        }

        public void OnClearEnemy(UnityEvent onClearAction)
        {
            if (isClear) return;
            isClear = true;
            onClearAction?.Invoke();
            onClearSubLevel.Invoke();
        }

        [System.Serializable]
        public class ConnectionData
        {
            public bool up;
            public bool right;
            public bool down;
            public bool left;
        }
    }
}