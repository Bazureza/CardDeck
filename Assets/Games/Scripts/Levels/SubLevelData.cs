using GuraGames.Character;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GuraGames.Level
{
    public class SubLevelData : MonoBehaviour
    {
        [SerializeField] private int id;
        [SerializeField] private string sublevelType;
        [SerializeField] private List<BaseCharacterSystem> enemies;
        
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
    }
}