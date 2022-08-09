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

        [SerializeField] private int baseCharacterHealth;
        [SerializeField] private int baseCharacterMana;

        [SerializeField, ReadOnly] private int health_point;
        [SerializeField, ReadOnly] private int mana_point;

        public void SetHealth(int health, bool forceChange = false)
        {
            baseCharacterHealth = (forceChange ? 0 : baseCharacterHealth) + health;
        }

        public void SetMana(int mana, bool forceChange = false)
        {
            baseCharacterMana = (forceChange ? 0 : baseCharacterMana) + mana;
        }

        public void UpdateHealth(int health)
        {
            baseCharacterHealth += health;

            if (baseCharacterHealth < 0) baseCharacterHealth = 0;
        }

        public void UpdateMana(int mana)
        {
            baseCharacterMana += mana;

            if (baseCharacterMana < 0) baseCharacterMana = 0;
        }

        public int BaseMana { get { return baseCharacterMana; } }
    }
}