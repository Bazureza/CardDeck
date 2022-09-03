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
        [SerializeField] private SpriteRenderer visual;

        [Header("References")]
        [SerializeField] private AIDecide decider;
        [SerializeField] private HealthUI healthUI;

        private DropSpawnHandler dropHandler;

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

        protected override void OnAwake()
        {
            base.OnAwake();
            dropHandler = GetComponent<DropSpawnHandler>();
        }

        protected override void OnStart()
        {
            base.OnStart();
            obstacle.Scan();
        }

        [Button]
        protected override void StartTurnWorld()
        {
            if (onAction) return;
            
            base.StartTurnWorld();
            healthUI.SetShieldIcon(BlockState.Active);

            StartCoroutine(DetectAround());
        }

        protected override void NextTurnWorld()
        {
            tbm.NextTurn();
        }

        protected override void Hit(int damage)
        {
            characterData.UpdateHealth(damage);
            healthUI.UpdateHealth(characterData.BaseHealthPoint, characterData.CurrentHealthPoint);
            if (characterData.CurrentHealthPoint == 0) Dead();
        }

        protected override void Dead()
        {
            dropHandler?.DropSpawn();
            level.GetActiveSubLevelData().RemoveEnemy(this);
            tbm.UpdateTurnQueue();
        }

        private IEnumerator DoMoveThroughPath(Path paths)
        {
            var max_move =  paths.path.Count > (farestNodeMove + 1) ? (farestNodeMove + 1) : (paths.path.Count - 1);
            for (int i = 1; i < max_move; i++)
            {
                visual.flipX = (transform.position.x > ((Vector3)paths.path[i].position).x);
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

            yield return StartCoroutine(agent.Scan(player.transform.position, true));

            decider.RefreshFetchedAction();
            Path paths = agent.GetScannedPath();
            decider.FetchedActionBasedOnCondition(ActionType.Block);
            if (paths.path.Count > 2) decider.FetchedActionBasedOnCondition(ActionType.Move);
            if (paths.path.Count == 2)
            {
                decider.FetchedActionBasedOnCondition(ActionType.MeleeAttack);
                decider.FetchedActionBasedOnCondition(ActionType.AttackAndBlock);
                decider.FetchedActionBasedOnCondition(ActionType.PiercedAttack);
            }
            if (paths.path.Count <= 4)
            {
                Vector3 originalPos = (Vector3)paths.path[0].position;
                Vector3 aheadPos = (Vector3)paths.path[paths.path.Count - 1].position;

                Vector3 offset = aheadPos - originalPos;

                if (Mathf.Abs(offset.x) < 0.2f || Mathf.Abs(offset.y) < 0.2f)
                {
                    decider.FetchedActionBasedOnCondition(ActionType.RangedAttack);
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
                    case ActionType.RangedAttack:
                        GGDebug.Console($"{gameObject.name} is attacking: Range Attack!");
                        ((ICharacterHit)player).Hit(gameObject.name, ((AttackPatternData)action).damage);
                        break;
                    case ActionType.MeleeAttack:
                        GGDebug.Console($"{gameObject.name} is attacking: Close Attack!");
                        ((ICharacterHit)player).Hit(gameObject.name, ((AttackPatternData)action).damage);
                        break;
                    case ActionType.Block:
                        GGDebug.Console($"{gameObject.name} is activate: Block Attack!");
                        BlockState = (true, ((AttackPatternData)action).damage);
                        break;
                    case ActionType.AttackAndBlock:
                        GGDebug.Console($"{gameObject.name} is attacking: Close Attack and Blocking!");
                        ((ICharacterHit)player).Hit(gameObject.name, ((AttackPatternData)action).damage);
                        BlockState = (true, ((AttackPatternData)action).damage);
                        break;
                    case ActionType.PiercedAttack:
                        GGDebug.Console($"{gameObject.name} is attacking: Pierced Attack!");
                        ((ICharacterHit)player).HitPierce(gameObject.name, ((AttackPatternData)action).damage);
                        break;
                }

                healthUI.SetShieldIcon(BlockState.Active);
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