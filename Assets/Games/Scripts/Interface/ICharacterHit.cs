namespace GuraGames.Interface
{
    public interface ICharacterHit
    {
        void Hit(string sender, int damage);
        void HitPierce(string sender, int damage);
    }
}