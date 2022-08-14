using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using GuraGames.GameSystem;
using GuraGames.UI;
using TomGustin.GameDesignPattern;
using MonsterLove.StateMachine;
using GuraGames.Enums;
using GuraGames.Manager;
using Sirenix.OdinInspector;
using GuraGames.Interface;
using GuraGames.Utility;
using GuraGames.Level;

namespace GuraGames.Character
{
    public class PlayerCharacterSystem : BaseCharacterSystem
    {
        [Header("References")]
        [SerializeField] private GameObject indicatorMove;
        [SerializeField] private GameObject indicatorBeamAttack;
        [SerializeField] private GameObject indicatorSwordAttack;
        [SerializeField] private HealthUI h_ui;
        [SerializeField] private StatUI s_ui;

        [Space(20)]
        [SerializeField, ReadOnly] private ActionType currentAction;

        private CameraSystem cameraSystem;
        private IndicatorMovesUI imui;
        private DeckManager deckManager;

        private bool active_turn;

        private LayerMask enemyLayer;

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

            cameraSystem = ServiceLocator.Resolve<CameraSystem>();
            imui = ServiceLocator.Resolve<IndicatorMovesUI>();
            deckManager = ServiceLocator.Resolve<DeckManager>();
            deckManager.InitDeck();

            enemyLayer = LayerMask.GetMask(new string[] { "Enemy" });
            h_ui.UpdateHealth(characterData.BaseHealthPoint, characterData.CurrentHealthPoint);
            s_ui.UpdateMove(characterData.CurrentMovePoint);
            s_ui.UpdateMana(characterData.CurrentManaPoint);
        }

        protected override void OnMove()
        {
            Path paths = agent.GetScannedPath();

            new UnityTaskManager.Task(DoMoveThroughPath(paths));
        }

        public void ForceEndTurn()
        {
            if (active_turn) NextTurnWorld();
        }

        public void OnActionCard(CardData actionCard)
        {
            switch (actionCard.action_type)
            {
                case ActionType.RangeAttackLinear:
                    GGDebug.Console($"Player using Action Card: {actionCard.name} with {actionCard.action_type.ToString()}");
                    ShowInteraction(false);
                    ShowIndicator("beam", true);

                    currentAction = actionCard.action_type;
                    break;
                case ActionType.CloseAttackLinear:
                    GGDebug.Console($"Player using Action Card: {actionCard.name} with {actionCard.action_type.ToString()}");
                    ShowInteraction(false);
                    ShowIndicator("sword", true);

                    currentAction = actionCard.action_type;
                    break;
            }
        }

        public void ChangeSubLevel(string move)
        {
            if (onAction) return;

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

        public override void MoveTo(Vector3 move_position)
        {
            if (!active_turn || !currentAction.Equals(ActionType.Move)) return;
            if (characterData.CurrentMovePoint == 0) return;
            base.MoveTo(move_position);
        }

        public void AttackTo(Vector3 attack_position)
        {
            if (!active_turn || currentAction.Equals(ActionType.Move) || onScan) return;

            new UnityTaskManager.Task(DoAttackScan(attack_position));
        }

        public bool IsSufficientMana(int mana_require)
        {
            return characterData.CurrentManaPoint >= mana_require;
        }

        private IEnumerator DoAttackScan(Vector3 attack_position)
        {
            onScan = true;
            Vector3 nodeAttackPosition;
            float angle = 0f;
            float length = 0f;
            float node_size = agent.GetCurrentNodeSize();
            GraphNode attackNode, currentNode;

            switch (currentAction)
            {
                case ActionType.RangeAttackLinear:
                    var rangeAttackData = ((AttackPatternData)deckManager.CurrentUsedCard);

                    nodeAttackPosition = agent.GetNodePositionOn(attack_position);
                    angle = MathExtend.VectorToAngle(nodeAttackPosition - transform.position);

                    attackNode = agent.GetCurrentActiveGraph().GetNearest(attack_position).node;
                    currentNode = agent.GetCurrentActiveGraph().GetNearest(transform.position).node;

                    if (attackNode.Equals(currentNode))
                    {
                        onScan = false;
                        yield break;
                    }

                    yield return null;
                    if (MathExtend.IsAngleInLinear(angle, 5))
                    {
                        length = (nodeAttackPosition - transform.position).magnitude;
                        
                        if (length <= (rangeAttackData.range_node * node_size) + (node_size/4))
                        {
                            StartCoroutine(DoAttackOfArea(nodeAttackPosition, rangeAttackData));
                        }
                    }
                    break;
                case ActionType.CloseAttackLinear:
                    var normalAttackData = ((AttackPatternData)deckManager.CurrentUsedCard);

                    nodeAttackPosition = agent.GetNodePositionOn(attack_position);
                    angle = MathExtend.VectorToAngle(nodeAttackPosition - transform.position);

                    attackNode = agent.GetCurrentActiveGraph().GetNearest(attack_position).node;
                    currentNode = agent.GetCurrentActiveGraph().GetNearest(transform.position).node;

                    if (attackNode.Equals(currentNode))
                    {
                        onScan = false;
                        yield break;
                    }

                    yield return null;
                    if (MathExtend.IsAngleInLinear(angle, 5))
                    {
                        length = (nodeAttackPosition - transform.position).magnitude;

                        if (length <= (normalAttackData.range_node * node_size) + (node_size / 4))
                        {
                            StartCoroutine(DoNormalAttack(nodeAttackPosition, normalAttackData));
                        }
                    }
                    break;
            }
            deckManager.UpdatePreview(null);
            onScan = false;
        }

        private IEnumerator DoMoveSubLevel(Vector2 direction)
        {
            onAction = true;
            ShowInteraction(false);

            Sequence seq = DOTween.Sequence();
            seq.Append(transform.DOMove(transform.position + ((Vector3)direction * agent.GetCurrentNodeSize()), 0.3f).SetEase(Ease.InOutCubic));
            seq.Join(cameraSystem.MoveCameraToSubLevel((int) agent.GetGraphOn(transform.position + ((Vector3)direction * agent.GetCurrentNodeSize())).graphIndex));
            yield return seq.WaitForCompletion();

            yield return null;
            onAction = false;

            ResetAction();
            CheckingTurnCondition();
            /*indicatorMove.SetActive(true);
            DetectAdjacentNode();*/
        }

        private IEnumerator DoMoveThroughPath(Path paths)
        {
            onAction = true;
            ShowInteraction(false);
            if (level.GetActiveSubLevelData().IsEnemiesClear())
            {
                if (characterData.CurrentMovePoint != characterData.BaseMovePoint) characterData.SetMove(characterData.BaseMovePoint, true);
            } else characterData.UpdateMove(-1);
            s_ui.UpdateMove(characterData.CurrentMovePoint);

            for (int i = 1; i < paths.path.Count; i++)
            {
                Tween tween = transform.DOMove((Vector3)paths.path[i].position, 0.3f).SetEase(Ease.InOutCubic);
                yield return tween.WaitForCompletion();
                yield return null;
            }

            yield return null;
            onAction = false;

            CheckingTurnCondition();
            /*indicatorMove.SetActive(true);
            DetectAdjacentNode();*/
        }

        private IEnumerator DoNormalAttack(Vector3 attack_position, AttackPatternData attackData)
        {
            onAction = true;
            ShowInteraction(false);
            characterData.UpdateMana(-deckManager.CurrentUsedCard.mana_consume);
            s_ui.UpdateMana(characterData.CurrentManaPoint);

            Collider2D coll = Physics2D.OverlapPoint(attack_position, enemyLayer);
            if (coll)
            {
                coll.GetComponent<ICharacterHit>().Hit("Player", attackData.damage);
            }

            yield return null;
            onAction = false;

            
            CheckingTurnCondition();
        }

        private IEnumerator DoAttackOfArea(Vector3 attack_position, AttackPatternData attackData)
        {
            onAction = true;
            characterData.UpdateMana(-deckManager.CurrentUsedCard.mana_consume);
            s_ui.UpdateMana(characterData.CurrentManaPoint);

            ShowInteraction(false);
            RaycastHit2D[] hits;

            //var node_size = agent.GetCurrentNodeSize();
            //var radius = attackData.range_node * node_size;
            hits = Physics2D.LinecastAll(transform.position, attack_position, enemyLayer);

            foreach (RaycastHit2D hit in hits)
            { 
                if (hit.collider) hit.collider.GetComponent<ICharacterHit>().Hit("Player", attackData.damage);
            }

            yield return null;
            onAction = false;

            characterData.UpdateMana(-deckManager.CurrentUsedCard.mana_consume);
            CheckingTurnCondition();
        }

        private void CheckingTurnCondition()
        {
            var checkingCondition = characterData.CurrentMovePoint > 0 || characterData.CurrentManaPoint > 0;
            if (checkingCondition)
            {
                StartTurnWorld();
            } else NextTurnWorld();
        }

        private void DetectAdjacentNode() 
        {
            if (!level.GetActiveSubLevelData().IsEnemiesClear()) return;
            agent.CheckConjunctionNode(out (bool top, bool right, bool bottom, bool left) adjacent);
            GGDebug.Console($"Top:{adjacent.top} - Right:{adjacent.right} - Bottom:{adjacent.bottom} - Left:{adjacent.left}");

            imui.RenderIndicatorMove(adjacent);
        }

        private void ShowInteraction(bool show)
        {
            if (!show)
            {
                ShowIndicator("beam", false);
                ShowIndicator("sword", false);
                ShowIndicator("move", false);
            }
            deckManager.ShowHandDecks(show);
            if (show) deckManager.UpdateHandCard();
            else imui.ResetIndicator();
        }

        private void ShowIndicator(string indicator_type, bool value)
        {
            switch (indicator_type)
            {
                case "beam":
                    indicatorBeamAttack.SetActive(value);
                    break;
                case "sword":
                    indicatorSwordAttack.SetActive(value);
                    break;
                case "move":
                    indicatorMove.SetActive(value);
                    break;
            }
        }

        private void ResetAction()
        {
            characterData.SetMove(characterData.BaseMovePoint, true);
            characterData.SetMana(characterData.BaseManaPoint, true);

            s_ui.UpdateMana(characterData.CurrentManaPoint);
            s_ui.UpdateMove(characterData.CurrentMovePoint);
        }

        protected override void Hit(int damage)
        {
            characterData.UpdateHealth(damage);
            h_ui.UpdateHealth(characterData.BaseHealthPoint, characterData.CurrentHealthPoint);
            if (characterData.CurrentHealthPoint == 0) Dead();
        }

        protected override void StartTurnWorld()
        {
            base.StartTurnWorld();

            active_turn = true;
            currentAction = ActionType.Move;
            if (characterData.CurrentMovePoint != 0) ShowIndicator("move", true);
            ShowInteraction(true);
            DetectAdjacentNode();
        }

        protected override void NextTurnWorld()
        {
            ResetAction();
            tbm.NextTurn();
        }

        protected override void EndTurnWorld()
        {
            base.EndTurnWorld();
            ShowInteraction(false);
            active_turn = false;
        }
    }
}