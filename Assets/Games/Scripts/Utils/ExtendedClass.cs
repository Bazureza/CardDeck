using UnityEngine;
using System.Collections.Generic;
using GuraGames.GameSystem;

public static class ExtendedClass
{
    public static void Shuffle(this List<CardData> list)
    {
        List<CardData> newList = new List<CardData>();

        while (list.Count > 0)
        {
            var randomValue = Random.Range(0, list.Count);
            newList.Add(list[randomValue]);
            list.RemoveAt(randomValue);
        }

        list.AddRange(newList);
    }
}
