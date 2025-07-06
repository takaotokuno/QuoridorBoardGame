namespace Quoridor.Player.Status
{
    public class Sleep : AbstractStatus
    {
        public Sleep(int remainTurns) : base("Sleep", remainTurns){}
        
        protected override void ApplyStatus(PlayerController player)
        => player.SwitchMovable(false);

        public override void RemoveStatus(PlayerController player)
        => player.SwitchMovable(true);
    }
}