using System.Collections.Generic;
using UnityEngine;
using Quoridor.Player;
using Quoridor.Player.Skill;

namespace Quoridor
{
    public interface IStartConfig
    {
        public PlayerConfigSO[] PlayerConfigs { get; }
        public List<SkillDefinitionSO> SkillDefinitions { get; }
        public int FirstTurnIndex { get; }
        public int MaxTurn { get; }
    }

    public class StartConfig : IStartConfig
    {
        private PlayerConfigSO[] _playerConfigs;
        private List<SkillDefinitionSO> _skillDefinitions;
        private int _firstTurnIndex;
        private int _maxTurn;
        public PlayerConfigSO[] PlayerConfigs => _playerConfigs;
        public List<SkillDefinitionSO> SkillDefinitions => _skillDefinitions;
        public int FirstTurnIndex => _firstTurnIndex;
        public int MaxTurn => _maxTurn;

        public StartConfig()
        {
            _playerConfigs = new PlayerConfigSO[2];
            PlayerConfigSO playerConfig = Resources.Load<PlayerConfigSO>("ScriptableObjects/Player/HumanPlayer");
            SetPlayerConfigs(playerConfig, playerConfig);
            _skillDefinitions = new List<SkillDefinitionSO>();
            _firstTurnIndex = 0;
            _maxTurn = 99;
        }

        public void SetPlayerConfigs(PlayerConfigSO frontPlayer, PlayerConfigSO backPlayer)
        {
            _playerConfigs[0] = frontPlayer;
            _playerConfigs[1] = backPlayer;
        }

        public void AddSkillDefinitions(List<SkillDefinitionSO> skillDefinitions)
        => _skillDefinitions.AddRange(skillDefinitions);

        public void SetFirstTurnIndex(int turnIndex)
        => _firstTurnIndex = turnIndex;

        public static Dictionary<string, PlayerConfigSO> LoadPlayerConfigs()
        {
            Dictionary<string, PlayerConfigSO> dic = new Dictionary<string, PlayerConfigSO>();
            PlayerConfigSO[] allPlayerConfigs
            = Resources.LoadAll<PlayerConfigSO>("ScriptableObjects/Player");
            foreach(PlayerConfigSO config in allPlayerConfigs)
            {
                dic.Add(config.pPlayerName, config);
            }
            return dic;
        }

        public static List<SkillDefinitionSO> LoadSkillDefinitions(List<string> skillNames)
        {
            List<SkillDefinitionSO> li = new ();
            foreach(string skillName in skillNames) li.Add(LoadSkillDefinition(skillName));
            return li;
        }

        public static Dictionary<string, SkillDefinitionSO> LoadSkillDefinitions()
        {
            Dictionary<string, SkillDefinitionSO> dic = new Dictionary<string, SkillDefinitionSO>();
            SkillDefinitionSO[] allSkillDefinitions
            = Resources.LoadAll<SkillDefinitionSO>("ScriptableObjects/Skill");
            foreach(SkillDefinitionSO config in allSkillDefinitions)
            {
                dic.Add(config.pSkillName, config);
            }
            return dic;
        }

        public static PlayerConfigSO LoadPlayerConfig(string fileName)
        => Resources.Load<PlayerConfigSO>("ScriptableObjects/Player/" + fileName);

        public static SkillDefinitionSO LoadSkillDefinition(string fileName)
        => Resources.Load<SkillDefinitionSO>("ScriptableObjects/Skill/" + fileName);
    }
}