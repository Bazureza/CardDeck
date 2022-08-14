/*
 * Script ini diadaptasi dan diambil dari script WeightedRandomizer milik Clay Game Studio
 * https://www.claygamestudio.com/
 */

using GuraGames.GameSystem;
using System.Collections.Generic;
using UnityEngine;

namespace GuraGames.Utility
{
    [System.Serializable]
    public class Gacha<T>
    {
        private int decrease_roll_value;
        private int increase_roll_value;

        public Gacha(int decrease_roll_value, int increase_roll_value)
        {
            this.decrease_roll_value = decrease_roll_value;
            this.increase_roll_value = increase_roll_value;
        }

        public class GachaMember
        {
            public T member;
            public float weight;

            public GachaMember(T member, float weight)
            {
                this.member = member;
                this.weight = weight;
            }

            public override string ToString()
            {
                return $"{member.ToString()} with weight {weight}";
            }
        }

        private List<GachaMember> members = new List<GachaMember>();

        public void Add(T member, float weight)
        {
            members.Add(new GachaMember(member, weight));
        }

        public void Remove(T member)
        {
            if (members.Exists(x => x.member.Equals(member)))
            {
                var fetchedMember = members.Find(x => x.member.Equals(member));
                members.Remove(fetchedMember);
            }
        }

        public void DebugToConsole()
        {
            foreach (GachaMember member in members)
            {
                GGDebug.Console(member.ToString());
            }
        }

        public T Roll()
        {
            float totalWeight = 0;
            T fetchedMember = members[members.Count - 1].member;

            foreach (GachaMember member in members)
            {
                totalWeight += member.weight;
            }

            float roll = Random.Range(0, totalWeight);
            float currentWeight = 0;
            foreach (GachaMember member in members)
            {
                currentWeight += member.weight;
                if (roll <= currentWeight)
                {
                    fetchedMember = member.member;
                    break;
                }
            }

            UpdatePityChance(fetchedMember);
            return fetchedMember;
        }

        private void UpdatePityChance(T member)
        {
            foreach (GachaMember gacha_member in members)
            {
                if (gacha_member.member.Equals(member))
                {
                    gacha_member.weight = Mathf.Max(0, gacha_member.weight - decrease_roll_value);
                } else
                {
                    gacha_member.weight = gacha_member.weight + increase_roll_value;
                }
            }

            DebugToConsole();
        }
    }
}
