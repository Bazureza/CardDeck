using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GuraGames.GameSystem
{
    public class MouseInputSystem : MonoBehaviour
    {
        [SerializeField] protected MouseEvent onClick;

        public static bool Active { set; get; }

        private void Update()
        {
            if (Active && Input.GetMouseButtonDown(0))
            {
                onClick?.Invoke(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            }
        }

        [System.Serializable]
        public class MouseEvent : UnityEvent<Vector3> { }
    }
}