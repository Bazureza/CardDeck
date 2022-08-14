namespace GuraGames.Enums
{
    public enum DebugType { Default, Warning, Error }
    public enum ManagerState { Inactive, Active }
    public enum CharacterType { None, Player, Enemy }
    public enum CharacterState { None, Idle, InTurn, Skip, Waiting }
    public enum ActionType { Move, RangeAttackRadial, RangeAttackLinear, CloseAttackRadial, CloseAttackLinear, Skip }
}