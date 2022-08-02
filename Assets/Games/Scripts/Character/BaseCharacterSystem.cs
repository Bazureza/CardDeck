using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TomGustin.GameDesignPattern;
using UnityEngine;

namespace GuraGames.Character
{
    public class BaseCharacterSystem : MonoBehaviour
    {
        [SerializeField] protected bool allowedDiagonalMoves;

        protected AstarPath aStar;

        protected virtual void OnAwake()
        {
            aStar = ServiceLocator.Resolve<AstarPath>();
        }

        protected virtual void OnStart() { }

        protected virtual void OnLoop() { }

        [Button]
        protected virtual void OnMove(Vector3 move_position)
        {
            var nearest = aStar.GetNearest(move_position);
            print("Move to: " + (Vector3)nearest.node.position);
            print(nearest.node.Walkable ? "Walkable" : "Blocked");
        }

        public void MoveTo(Vector3 move_position)
        {
            OnMove(move_position);
        }

        private void Awake()
        {
            OnAwake();
        }

        private void Start()
        {
            OnStart();
        }

        private void Update()
        {
            OnLoop();
        }
    }
}