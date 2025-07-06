using System.Collections.Generic;
using Quoridor.Board;
using Quoridor.Player.Skill;
using Quoridor.Player.Status;

namespace Quoridor.Player
{
    public interface IPlayerView
    {
        public int TurnIndex{get;}
        public int RemainWalls{get;}
        public List<ISkill> SkillSlots{get;}
        public List<IStatus> ActiveStatusList{get;}
    }

    public interface IPlayer : IPlayerView, IColleague
    {
        public bool JudgeSelectableMove(Pos move);
        public void OnTurnStart();
        public void OnInstructMove(Pos move);
        public void OnInstructUseSkill(int index);
        public void AddActiveStatus(IStatus status);
        public void RemoveActiveStatus(IStatus status);
        public void SwitchAutoManualMode(bool isAuto);
        public void SwitchMovable(bool canMove);
        public void OnGameEnd();
    }
}