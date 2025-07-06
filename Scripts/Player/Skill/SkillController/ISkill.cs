using System.Collections.Generic;
using Quoridor.Board;

namespace Quoridor.Player.Skill
{
    public enum SkillState { READY, SELECTED, COOLDOWN, SEALED, EMPTY }
    public interface ISkillView
    {
        public int Charge {get;}
        public SkillState State {get;}
        public string Name {get;}
        public List<string> TagSet {get;} // ※TagにはNameを含む
    }

    public interface ISkill : ISkillView
    {
        public void Initialize();
        public void AdjustCharge(int amount);
        public int CountInterval(int amount);
        public void SetState(SkillState state);
        public void Release();
        public void CoolDown();
        public void Refresh();
        public void Execute();
    }

    public interface IBoardSkill
    {
        public IEnumerable<Pos> GetValidMoves(IBoard board);
        public void Execute(IBoard board, Pos move);
    }

    public interface IInstanceSkill{}

    public class SkillUtil
    {
        public static bool CanUse(ISkillView skill)
        {
            return (skill.State == SkillState.READY || skill.State == SkillState.SELECTED);
        }
    }
}