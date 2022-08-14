using GuraGames.Enums;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GuraGames.Character
{
    [System.Serializable]
    public class CharacterData
    {
        [SerializeField] private CharacterType characterType;
        [SerializeField] private string characterID;
        [SerializeField] private string characterName;

        [SerializeField] private int baseCharacterHealthPoint;
        [SerializeField] private int baseCharacterManaPoint;
        [SerializeField] private int baseCharacterMovePoint;

        [SerializeField, ReadOnly] private int health_point;
        [SerializeField, ReadOnly] private int mana_point;
        [SerializeField, ReadOnly] private int move_point;

        public void SetHealth(int health = 0, bool forceChange = false)
        {
            health_point = (forceChange ? 0 : baseCharacterHealthPoint) + health;
        }

        public void SetMana(int mana = 0, bool forceChange = false)
        {
            mana_point = (forceChange ? 0 : baseCharacterManaPoint) + mana;
        }

        public void SetMove(int move = 0, bool forceChange = false)
        {
            move_point = (forceChange ? 0 : baseCharacterMovePoint) + move;
        }

        public void UpdateHealth(int health)
        {
            health_point += health;

            if (health_point < 0) health_point = 0;
        }

        public void UpdateMana(int mana)
        {
            mana_point += mana;

            if (mana_point < 0) mana_point = 0;
        }

        public void UpdateMove(int move)
        {
            move_point += move;

            if (move_point < 0) move_point = 0;
        }

        public int BaseHealthPoint { get { return baseCharacterHealthPoint; } }
        public int BaseManaPoint { get { return baseCharacterManaPoint; } }
        public int BaseMovePoint { get { return baseCharacterMovePoint; } }
        public int CurrentHealthPoint { get { return health_point; } }
        public int CurrentManaPoint { get { return mana_point; } }
        public int CurrentMovePoint { get { return move_point; } }
    }
}