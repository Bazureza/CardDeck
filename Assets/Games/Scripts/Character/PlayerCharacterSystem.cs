using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using GuraGames.GameSystem;
using GuraGames.UI;
using TomGustin.GameDesignPattern;

namespace GuraGames.Character
{
    public class PlayerCharacterSystem : BaseCharacterSystem
    {
        [Header("References")]
        [SerializeField] private GameObject indicatorMove;

        private CameraSystem cameraSystem;
        private IndicatorMovesUI imui;

        private bool active_turn;

        protected override void OnAwake()
        {
            base.OnAwake();

            cameraSystem = ServiceLocator.Resolve<CameraSystem>();
            imui = ServiceLocator.Resolve<IndicatorMovesUI>();
        }

        protected override void OnMove()
        {
            Path paths = agent.GetScannedPath();

            new UnityTaskManager.Task(DoMoveThroughPath(paths));
        }

        public void ChangeSubLevel(string move)
        {
            if (onMove) return;

            Vector2 directionMove = Vector2.zero;
            switch (move.ToLower())
            {
                case "up":
                    directionMove = Vector2.up;
                    break;
                case "right":
                    directionMove = Vector2.right;
                    break;
                case "down":
                    directionMove = Vector2.down;
                    break;
                case "left":
                    directionMove = Vector2.left;
                    break;
            }

            new UnityTaskManager.Task(DoMoveSubLevel(directionMove));
        }

        private IEnumerator DoMoveSubLevel(Vector2 direction)
        {
            onMove = true;
            imui.ResetIndicator();
            indicatorMove.SetActive(false);

            Sequence seq = DOTween.Sequence();
            seq.Append(transform.DOMove(transform.position + ((Vector3)direction * agent.GetCurrentNodeSize()), 0.3f).SetEase(Ease.InOutCubic));
            seq.Join(cameraSystem.MoveCameraToSubLevel((int) agent.GetGraphOn(transform.position + ((Vector3)direction * agent.GetCurrentNodeSize())).graphIndex));
            yield return seq.WaitForCompletion();

            yield return null;
            onMove = false;

            NextTurnWorld();
            /*indicatorMove.SetActive(true);
            DetectAdjacentNode();*/
        }

        private IEnumerator DoMoveThroughPath(Path paths)
        {
            onMove = true;
            imui.ResetIndicator();
            indicatorMove.SetActive(false);

            for (int i = 1; i < paths.path.Count; i++)
            {
                Tween tween = transform.DOMove((Vector3)paths.path[i].position, 0.3f).SetEase(Ease.InOutCubic);
                yield return tween.WaitForCompletion();
                yield return null;
            }

            yield return null;
            onMove = false;

            NextTurnWorld();
            /*indicatorMove.SetActive(true);
            DetectAdjacentNode();*/
        }

        private void DetectAdjacentNode() 
        {
            agent.CheckConjunctionNode(out (bool top, bool right, bool bottom, bool left) adjacent);
            GGDebug.Console($"Top:{adjacent.top} - Right:{adjacent.right} - Bottom:{adjacent.bottom} - Left:{adjacent.left}");

            imui.RenderIndicatorMove(adjacent);
        }

        public override void MoveTo(Vector3 move_position)
        {
            if (!active_turn) return;
            base.MoveTo(move_position);
        }

        protected override void StartTurnWorld()
        {
            active_turn = true;
            base.StartTurnWorld();
            indicatorMove.SetActive(true);
            DetectAdjacentNode();
        }

        protected override void NextTurnWorld()
        {
            EndTurnWorld();
        }

        protected override void EndTurnWorld()
        {
            base.EndTurnWorld();
            active_turn = false;
            tbm.NextTurn();
        }
    }
}