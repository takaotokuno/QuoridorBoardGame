using System.Collections.Generic;
using Quoridor.Player.Skill;

namespace Quoridor.Player
{
    public class PlayerInitializer
    {
        public static void SetPlayersConfig(IPlayer[] players, IStartConfig config)
        {
            PlayerConfigSO[] playerConfigs = config.PlayerConfigs;
            List<SkillDefinitionSO> skillDefinitions = config.SkillDefinitions;

            SetPlayerConfig(players[0], playerConfigs[0], skillDefinitions);
            SetPlayerConfig(players[1], playerConfigs[1], skillDefinitions);
        }

        private static void SetPlayerConfig(
            IPlayer player
            , PlayerConfigSO playerConfig
            , List<SkillDefinitionSO> addSkillDefinitions
        )
        {
            List<SkillDefinitionSO> skillDefinitions = new(playerConfig.pSkillDefinitionList);
            if (playerConfig.pPlayerName == "Player") skillDefinitions.AddRange(addSkillDefinitions);
            (player as PlayerController).SetConfig(playerConfig, skillDefinitions);
        }
    }
}