using GuraGames.Enums;
using UnityEngine;

[CreateAssetMenu(menuName = "Create Action/General", fileName = "Card Data")]
public class CardData : ScriptableObject
{
    public string card_id;
    public string card_name;
    public string card_desc;
    public ActionType action_type;

    public int mana_consume;

    public Sprite card_icon;
}
