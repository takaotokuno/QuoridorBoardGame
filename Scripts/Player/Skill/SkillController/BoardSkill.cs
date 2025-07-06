using System.Collections.Generic;
using Quoridor.Board;
using Quoridor.Command;

namespace Quoridor.Player.Skill
{
    public class BoardSkill : SkillController, IBoardSkill
    {
        protected BoardSkillStrategy _strategy;

        public BoardSkill(BoardSkillStrategy strategy, int capacity, int interval)
        : base(capacity, interval)
        => _strategy = strategy;

        public IEnumerable<Pos> GetValidMoves(IBoard board)
        => (_state == SkillState.SELECTED) ? _strategy.GetValidMoves(board) : new List<Pos>();

        public override void Execute() => SetState(SkillState.SELECTED);

        public void Execute(IBoard board, Pos move)
        => _commandInvoker.ExecuteCommand(_strategy.GenerateCommand(board, move, this));

        public BoardSkill Convert2VirtualSkill(ICommandInvoker commandInvoker)
        {
            BoardSkill skill = new BoardSkill(_strategy, _charge, _interval);
            skill.SetCommandInvoker(commandInvoker);
            return skill;
        }
    }
}
