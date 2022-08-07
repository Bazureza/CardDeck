using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GuraGames.Level
{
    public class SubLevelData : MonoBehaviour
    {
        [SerializeField] private int id;
        [SerializeField] private string sublevelType;
        
        public int ID 
        {
            get { return id; }
            set { id = value; }
        }

        public Vector3 GetLevelPosition()
        {
            return transform.position;
        }
    }
}