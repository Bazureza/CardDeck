using GuraGames.Character;
using System.Collections;
using System.Collections.Generic;
using TomGustin.GameDesignPattern;
using UnityEngine;

namespace GuraGames.UI
{
    public class IndicatorMovesUI : MonoBehaviour
    {
        [Header("References Local")]
        [SerializeField] private RectTransform upMove;
        [SerializeField] private RectTransform rightMove;
        [SerializeField] private RectTransform downMove;
        [SerializeField] private RectTransform leftMove;

        [Header("References Global")]
        [SerializeField] private RectTransform upMoveMainUI;
        [SerializeField] private RectTransform rightMoveMainUI;
        [SerializeField] private RectTransform downMoveMainUI;
        [SerializeField] private RectTransform leftMoveMainUI;

        private PlayerCharacterSystem _player;
        private PlayerCharacterSystem player
        {
            get
            {
                if (_player == null) _player = ServiceLocator.Resolve<PlayerCharacterSystem>();
                return _player;
            }
        }

        public void Move(string move)
        {
            player?.ChangeSubLevel(move);
        }

        public void RenderIndicatorMoveLocal((bool up, bool right, bool down, bool left) indicatorMove)
        {
            upMove.gameObject.SetActive(indicatorMove.up);
            rightMove.gameObject.SetActive(indicatorMove.right);
            downMove.gameObject.SetActive(indicatorMove.down);
            leftMove.gameObject.SetActive(indicatorMove.left);
        }

        public void RenderIndicatorMoveGlobal((bool up, bool right, bool down, bool left) indicatorMove)
        {
            upMoveMainUI.gameObject.SetActive(indicatorMove.up);
            rightMoveMainUI.gameObject.SetActive(indicatorMove.right);
            downMoveMainUI.gameObject.SetActive(indicatorMove.down);
            leftMoveMainUI.gameObject.SetActive(indicatorMove.left);
        }

        public void ResetIndicatorLocal()
        {
            RenderIndicatorMoveLocal((false, false, false, false));
        }

        public void ResetIndicatorGlobal()
        {
            RenderIndicatorMoveGlobal((false, false, false, false));
        }
    }
}