using GuraGames.Character;
using System.Collections;
using System.Collections.Generic;
using TomGustin.GameDesignPattern;
using UnityEngine;

namespace GuraGames.UI
{
    public class IndicatorMovesUI : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private RectTransform upMove;
        [SerializeField] private RectTransform rightMove;
        [SerializeField] private RectTransform downMove;
        [SerializeField] private RectTransform leftMove;

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

        public void RenderIndicatorMove((bool up, bool right, bool down, bool left) indicatorMove)
        {
            upMove.gameObject.SetActive(indicatorMove.up);
            rightMove.gameObject.SetActive(indicatorMove.right);
            downMove.gameObject.SetActive(indicatorMove.down);
            leftMove.gameObject.SetActive(indicatorMove.left);
        }

        public void ResetIndicator()
        {
            RenderIndicatorMove((false, false, false, false));
        }
    }
}