using UnityEngine;
using TMPro;
using Quoridor.Player.Skill;

namespace Quoridor.UI
{
    public class SkillButton : UIButton
    {
        private SkillButtonSet _manager;
        private int _slotIndex;

        [SerializeField] private TextMeshProUGUI pSkillName;
        [SerializeField] private TextMeshProUGUI pSkillCharge;

        public void SetManager(SkillButtonSet manager, int slotIndex)
        {
            _manager = manager;
            _slotIndex = slotIndex;
        }

        public void SetEmptyDisplay()
        {
            SetInteractable(false);
            pSkillName.text = "";
            pSkillCharge.text = "";
        }

        public void UpdateDisplay(ISkillView skill)
        {
            SetInteractable(SkillUtil.CanUse(skill));
            pSkillName.text = skill.Name;
            pSkillCharge.text = skill.Charge.ToString();
        }

        public void OnUseSkill()
        => _manager.OnUseSkill(this, _slotIndex);
    }
}
