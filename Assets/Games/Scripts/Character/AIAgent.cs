using GuraGames.GameSystem;
using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using TomGustin.GameDesignPattern;
using UnityEngine;

namespace GuraGames.Character
{
    public class AIAgent : MonoBehaviour
    {
        //Pathfinding props
        protected AstarPath aStar;
        protected Seeker seeker;
        protected Path currentPath;

        private GraphNode lastNode;

        public void Init()
        {
            aStar = ServiceLocator.Resolve<AstarPath>();
            seeker = GetComponent<Seeker>();
        }

        public bool Scan(Vector3 target_position)
        {
            var nearest = aStar.GetNearest(target_position);

            if (lastNode != null && lastNode.Equals(nearest.node)) return false;
            lastNode = nearest.node;

            if (lastNode.Walkable)
            {
                GGDebug.Console("Move to: " + (Vector3)lastNode.position);
                currentPath = seeker.StartPath(transform.position, (Vector3)lastNode.position);
                return true;
            }
            else
            {
                GGDebug.Console("Blocked", Enums.DebugType.Warning);
                return false;
            }
        }

        public Path GetScannedPath()
        {
            return currentPath;
        }
    }
}