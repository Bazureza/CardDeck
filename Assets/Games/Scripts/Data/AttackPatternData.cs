using GuraGames.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Create Action/Attack", fileName = "Attack Data")]
public class AttackPatternData : CardData
{
    public int range_node;
    public int damage;
}
