using System.Collections.Generic;
using UnityEngine;
using Quoridor.Player.Skill;

namespace Quoridor.Player
{
    [CreateAssetMenu(fileName = "PlayerConfig", menuName = "Player/Create New Player Config")]
    public class PlayerConfigSO : ScriptableObject
    {
        public string pPlayerName;
        public bool pIsAuto;
        public int pCpuLevel;
        public EvaluationSO pEvaluationFunc;
        public List<SkillDefinitionSO> pSkillDefinitionList;
    }
}