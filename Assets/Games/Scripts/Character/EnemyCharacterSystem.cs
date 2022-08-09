using DG.Tweening;
using GuraGames.GameSystem;
using Pathfinding;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TomGustin.GameDesignPattern;
using UnityEngine;

namespace GuraGames.Character
{
    public class EnemyCharacterSystem : BaseCharacterSystem
    {
        [Header("Properties")]
        [SerializeField] private int farestNodeMove;
        [SerializeField] private DynamicGridObstacle obstacle;

        private PlayerCharacterSystem _player;
        private PlayerCharacterSystem player
        {
            get
            {
                if (!_player) _player = ServiceLocator.Resolve<PlayerCharacterSystem>();
                return _player;
            }
        }

        [Button]
        protected override void StartTurnWorld()
        {
            MoveToPlayer();
        }

        protected override void NextTurnWorld()
        {
            EndTurnWorld();
            tbm.NextTurn();
        }

        protected override void OnMove()
        {
            GGDebug.Console("Generate Path Enemy!");
            Path paths = agent.GetScannedPath();

            new UnityTaskManager.Task(DoMoveThroughPath(paths));
        }

        private void MoveToPlayer()
        {
            if (onMove) return;
            obstacle.EnableCollider(false);
            obstacle.Scan();
            StartCoroutine(agent.Scan(player.transform.position, OnMove, true));
        }

        private IEnumerator DoMoveThroughPath(Path paths)
        {
            onMove = true;

            var max_move =  paths.path.Count > (farestNodeMove + 1) ? (farestNodeMove + 1) : (paths.path.Count - 1);
            for (int i = 1; i < max_move; i++)
            {
                Tween tween = transform.DOMove((Vector3)paths.path[i].position, 0.3f).SetEase(Ease.InOutCubic);
                yield return tween.WaitForCompletion();
                yield return null;
            }

            yield return null;
            onMove = false;
            obstacle.EnableCollider(true);
            obstacle.Scan();

            NextTurnWorld();
        }
    }
}