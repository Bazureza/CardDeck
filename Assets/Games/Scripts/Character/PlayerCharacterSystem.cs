using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace GuraGames.Character
{
    public class PlayerCharacterSystem : BaseCharacterSystem
    {
        protected override void OnMove()
        {
            Path paths = agent.GetScannedPath();

            new UnityTaskManager.Task(DoMoveThroughPath(paths));
        }

        private IEnumerator DoMoveThroughPath(Path paths)
        {
            onMove = true;
            for (int i = 1; i < paths.path.Count; i++)
            {
                Tween tween = transform.DOMove((Vector3)paths.path[i].position, 0.3f).SetEase(Ease.Linear);
                yield return tween.WaitForCompletion();
                yield return null;
            }

            yield return null;
            onMove = false;
        }

    }
}