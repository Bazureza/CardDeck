using DG.Tweening;
using GuraGames.AI;
using GuraGames.Enums;
using GuraGames.GameSystem;
using GuraGames.Interface;
using GuraGames.Level;
using GuraGames.UI;
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

        [Header("References")]
        [SerializeField] private AIDecide decider;
        [SerializeField] private HealthUI healthUI;

        private PlayerCharacterSystem _player;
        private PlayerCharacterSystem player
        {
            get
            {
                if (!_player) _player = ServiceLocator.Resolve<PlayerCharacterSystem>();
                return _player;
            }
        }

        private LevelDataManager _level;
        private LevelDataManager level
        {
            get
            {
                if (!_level) _level = ServiceLocator.Resolve<LevelDataManager>();
                return _level;
            }
        }

        [Button]
        protected override void StartTurnWorld()
        {
            if (onAction) return;

            StartCoroutine(DetectAround());
        }

        protected override void NextTurnWorld()
        {
            tbm.NextTurn();
        }

        protected override void OnMove()
        {
            GGDebug.Console("Generate Path Enemy!");
            Path paths = agent.GetScannedPath();

            new UnityTaskManager.Task(DoMoveThroughPath(paths));
        }

        protected override void Hit(int damage)
        {
            characterData.UpdateHealth(damage);
            healthUI.UpdateHealth(characterData.BaseHealthPoint, characterData.CurrentHealthPoint);
            if (characterData.CurrentHealthPoint == 0) Dead();
        }

        protected override void Dead()
        {
            level.GetActiveSubLevelData().RemoveEnemy(this);
        }

        private IEnumerator DoMoveThroughPath(Path paths)
        {
            var max_move =  paths.path.Count > (farestNodeMove + 1) ? (farestNodeMove + 1) : (paths.path.Count - 1);
            for (int i = 1; i < max_move; i++)
            {
                Tween tween = transform.DOMove((Vector3)paths.path[i].position, 0.3f).SetEase(Ease.InOutCubic);
                yield return tween.WaitForCompletion();
                yield return null;
            }

            yield return null;
        }

        private IEnumerator DetectAround()
        {
            onAction = true;
            obstacle.EnableCollider(false);
            obstacle.Scan();

            yield return StartCoroutine(agent.Scan(player.transform.position, null, true));

            decider.RefreshFetchedAction();
            Path paths = agent.GetScannedPath();
            if (paths.path.Count > 2) decider.FetchedActionBasedOnCondition(ActionType.Move);
            if (paths.path.Count == 2) decider.FetchedActionBasedOnCondition(ActionType.CloseAttackLinear);
            if (paths.path.Count <= 4)
            {
                Vector3 originalPos = (Vector3)paths.path[0].position;
                Vector3 aheadPos = (Vector3) paths.path[paths.path.Count - 1].position;

                Vector3 offset = aheadPos - originalPos;

                if (Mathf.Abs(offset.x) < 0.2f || Mathf.Abs(offset.y) < 0.2f)
                {
                    decider.FetchedActionBasedOnCondition(ActionType.RangeAttackLinear);
                }
            }

            yield return null;
            CardData action = decider.DecideAction();

            if (action)
            {
                switch (action.action_type)
                {
                    case ActionType.Move:
                        GGDebug.Console($"{gameObject.name} is moving!");
                        yield return StartCoroutine(DoMoveThroughPath(paths));
                        break;
                    case ActionType.RangeAttackLinear:
                        GGDebug.Console($"{gameObject.name} is attacking: Range Attack!");
                        ((ICharacterHit)player).Hit(gameObject.name, ((AttackPatternData)action).damage);
                        break;
                    case ActionType.CloseAttackLinear:
                        GGDebug.Console($"{gameObject.name} is attacking: Close Attack!");
                        ((ICharacterHit)player).Hit(gameObject.name, ((AttackPatternData)action).damage);
                        break;
                }
            } else
            {
                GGDebug.Console("There is no action happen!");
            }
            obstacle.EnableCollider(true);
            obstacle.Scan();
            onAction = false;
            NextTurnWorld();
        }
    }
}