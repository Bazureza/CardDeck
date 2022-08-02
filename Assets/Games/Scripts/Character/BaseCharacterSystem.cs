using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GuraGames.Character
{
    public class BaseCharacterSystem : MonoBehaviour
    {
        [SerializeField] protected bool allowedDiagonalMoves;

        protected virtual void OnMove()
        {

        }
    }
}