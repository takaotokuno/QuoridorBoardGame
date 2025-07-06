using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Quoridor.Player;
using Quoridor.Player.Skill;

namespace Quoridor.UI
{
    public class StartPanelManager : UIManager
    {
        private Dictionary<string, PlayerConfigSO> _playerConfigDic;
        private Dictionary<string, SkillDefinitionSO> _skillDefinitionDic;

        // Game Objects
        [SerializeField] private GameObject _firstPlayerName;
        [SerializeField] private GameObject _secondPlayerName;
        [SerializeField] private GameObject _turnSelection;
        [SerializeField] private GameObject _skillSlot1;
        [SerializeField] private GameObject _skillSlot2;
        [SerializeField] private GameObject _skillSlot3;
        [SerializeField] private GameObject _startButton;


        // Initial Settings
        protected override void Awake()
        {
            _playerConfigDic = StartConfig.LoadPlayerConfigs();
            _skillDefinitionDic = StartConfig.LoadSkillDefinitions();

            SetNameOptions(_firstPlayerName);
            SetNameOptions(_secondPlayerName);

            SetSkillOptions(_skillSlot1);
            SetSkillOptions(_skillSlot2);
            SetSkillOptions(_skillSlot3);

            _startButton.GetComponent<StartButton>().SetManager(this);

            base.Awake();
        }
        private void SetNameOptions(GameObject dropdownObject)
        => SetOptions(dropdownObject, new (_playerConfigDic.Keys));
        private void SetSkillOptions(GameObject dropdownObject)
        => SetOptions(dropdownObject, new (_skillDefinitionDic.Keys));

        // Inform to Mediator
        public void InformGameStart() => _facade?.OnStart(this, SetStartConfig());

        private IStartConfig SetStartConfig()
        {
            StartConfig config = new StartConfig();
            config.SetPlayerConfigs(
                _playerConfigDic[GetSelectedValue(_firstPlayerName)]
                , _playerConfigDic[GetSelectedValue(_secondPlayerName)]
            );
            config.AddSkillDefinitions(GetSkillDefinitions());
            config.SetFirstTurnIndex((GetSelectedValue(_turnSelection) == "First")? 0 : 1);

            return config;
        }

        private List<SkillDefinitionSO> GetSkillDefinitions()
        => new (){
            _skillDefinitionDic[GetSelectedValue(_skillSlot1)]
            , _skillDefinitionDic[GetSelectedValue(_skillSlot2)]
            , _skillDefinitionDic[GetSelectedValue(_skillSlot3)]
        };

        private static void SetOptions(GameObject dropdownObject, List<string> options)
        {
            TMP_Dropdown dropdown = dropdownObject.GetComponent<TMP_Dropdown>();
            foreach(string option in options)
            {
                dropdown.options.Add(new TMP_Dropdown.OptionData{text = option});
            }
            dropdown.RefreshShownValue();
        }

        private static string GetSelectedValue(GameObject dropdownObject)
        {
            TMP_Dropdown dropdown = dropdownObject.GetComponent<TMP_Dropdown>();
            return dropdown.options[dropdown.value].text;
        }
    }

    public interface StartPanelButton
    {
        public void SetManager(StartPanelManager manager);
    }
}
