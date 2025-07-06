using UnityEngine;
using Quoridor.Player;

namespace Quoridor.UI
{
    public class CanvasSkillUIManager : UIManager
    {
        // Game Objects
        [SerializeField] private SkillButtonSet pSkillButtonSetP1;
        [SerializeField] private SkillButtonSet pSkillButtonSetP2;

        // Initial Settings
        private void Start()
        {
            pSkillButtonSetP1.SetManager(this, 0);
            pSkillButtonSetP2.SetManager(this, 1);
        }

        // Inform to Mediator
        public void OnUseSkill(SkillButtonSet button, int turnIndex, int slotIndex)
        => _facade.OnUseSkill(this, turnIndex, slotIndex);

        public override void AppearanceElement()
        {
            base.AppearanceElement();

            pSkillButtonSetP1.AppearanceElement();
            pSkillButtonSetP2.AppearanceElement();
        }
        
        public void UpdateDisplay(IPlayerView[] players)
        {
            pSkillButtonSetP1.UpdateDisplay(players[0]);
            pSkillButtonSetP2.UpdateDisplay(players[1]);
        }
    }
}
