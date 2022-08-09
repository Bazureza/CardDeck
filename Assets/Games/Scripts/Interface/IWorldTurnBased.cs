namespace GuraGames.Interface
{
    public interface IWorldTurnBased
    {
        void StartTurn_World();
        void NextTurn_World();
        void EndTurn_World();
    }
}