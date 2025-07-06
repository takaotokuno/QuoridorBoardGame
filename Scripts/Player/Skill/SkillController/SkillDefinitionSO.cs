using System.Collections.Generic;
using UnityEngine;

namespace Quoridor.Player.Skill
{
    public interface IBoardSkillDefinition
    {
        public BoardSkill CreateSkill(int turnIndex);
    }

    public interface IBuffSkillDefinition
    {
        public BuffSkill CreateSkill(IPlayer player, IPlayer opponent);
    }

    public abstract class SkillDefinitionSO : ScriptableObject
    {
        public string pSkillName;
        public int pCapacity;
        public int pInterval;
        public List<string> pTagList;
    }

    public static class MainTags {
        public const string Normal = "Normal";
        public const string Special = "Special";
        public const string Pawn = "Pawn";
        public const string Wall = "Wall";
        public const string Passive = "Passive";
        public const string Buff = "Buff";
        public const string Debuff = "Debuff";
    }
}