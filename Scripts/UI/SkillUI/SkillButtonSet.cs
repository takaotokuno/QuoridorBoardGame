using UnityEngine;
using Quoridor.Player;
using Quoridor.Player.Skill;
using System.Collections.Generic;

namespace Quoridor.UI
{
    public class SkillButtonSet : UIElement
    {
        private CanvasSkillUIManager _manager;
        private int _turnIndex;
        [SerializeField] private SkillButton[] pSkillButtonList;

        private void Start()
        {
            for(int i = 0; i < pSkillButtonList.Length; i++)
            {
                pSkillButtonList[i].SetManager(this, i);
            }
        }

        public override void AppearanceElement()
        {
            foreach(SkillButton button in pSkillButtonList)
            {
                button.SetActive(true);
            }

            base.AppearanceElement();

            foreach(SkillButton button in pSkillButtonList)
            {
                button.AppearanceElement();
            }
        }

        public void SetManager(CanvasSkillUIManager manager, int turnIndex)
        {
            _manager = manager;
            _turnIndex = turnIndex;
        }

        public void UpdateDisplay(IPlayerView player)
        {
            List<ISkill> slots = player.SkillSlots;
            for(int i = 0; i < pSkillButtonList.Length; i++)
            {
                if(i < slots.Count - 2) pSkillButtonList[i].UpdateDisplay(slots[i + 2]);
                else pSkillButtonList[i].SetEmptyDisplay();
            }
        }

        public void OnUseSkill(UIButton button, int slotIndex)
        => _manager.OnUseSkill(this, _turnIndex, slotIndex);
    }
}