namespace GuraGames.Interface
{
    public interface ITurnBased
    {
        void StartTurn();
        void NextTurn();
        void EndTurn();
    }
}