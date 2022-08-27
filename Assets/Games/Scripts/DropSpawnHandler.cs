using GuraGames.Character;
using GuraGames.Enums;
using TomGustin.GameDesignPattern;
using UnityEngine;

public class DropSpawnHandler : MonoBehaviour
{
    [SerializeField] private DropData[] drops;

    private PlayerCharacterSystem _player;
    private PlayerCharacterSystem player
    {
        get
        {
            if (!_player) _player = ServiceLocator.Resolve<PlayerCharacterSystem>();
            return _player;
        }
    }

    public void DropSpawn()
    {
        foreach (DropData drop in drops)
        {
            Drop(drop);
        }
    }

    private void Drop(DropData dropData)
    {
        switch (dropData.dropType)
        {
            case DropSpawnType.Coin:
                DropCoin((dropData.minValue, dropData.maxValue));
                break;
        }
    }

    private void DropCoin((int min, int max) value)
    {
        var randomDrop = Random.Range(value.min, value.max);
        randomDrop = randomDrop - (randomDrop % 10);
        player.AddCoin(randomDrop);
    }

    [System.Serializable]
    public class DropData
    {
        public DropSpawnType dropType;
        public int minValue;
        public int maxValue;
    }
}
