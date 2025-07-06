using System;
using System.Collections.Generic;
using Quoridor.Command;

namespace Quoridor.Player.Skill
{
    public abstract class SkillController : ISkill
    {
        protected int _capacity;
        protected int _charge;
        protected int _interval;
        protected int _intervalCount;
        protected SkillState _state;
        private List<string> _tagSet;
        protected ICommandInvoker _commandInvoker;
        
        // Getter
        public int Charge => _charge;
        public SkillState State => _state;
        public string Name => _tagSet[0];
        public List<string> TagSet => _tagSet;

        public SkillController(int capacity, int interval)
        {
            _capacity = capacity;
            _charge = capacity;
            _interval = interval;
            _intervalCount = 0;
            _tagSet = new List<string>();
        }
        public void SetCommandInvoker(ICommandInvoker commandInvoker)
        {
            _commandInvoker = commandInvoker;
        }

        public void AddTag(string tag) => _tagSet.Add(tag);
        public void AddTags(List<string> tagList) => _tagSet.AddRange(tagList);

        public void Initialize()
        {
            _charge = _capacity;
            Refresh();
            Release();
        }

        public void AdjustCharge(int amount) => _charge += amount;
        public int CountInterval(int amount)
        {
            int intervalCount = _intervalCount;
            _intervalCount = Math.Max(_intervalCount + amount, 0);
            return intervalCount;
        }

        public void SetState(SkillState state)
        {
            // 残数が0ならEMPTY
            if(_charge == 0) _state = SkillState.EMPTY;

            // 現在封印中 or 封印されたならSEALED
            else if(_state == SkillState.SEALED) return;
            else if(state == SkillState.SEALED) _state = SkillState.SEALED;
            
            // インターバル中ならCOOLDOWN
            else if(_intervalCount > 0) _state = SkillState.COOLDOWN;

            // それ以外の場合は入力値に従う
            else _state = state;
        }
        public void Release()
        {
            _state = SkillState.READY;
            SetState(_state);
        }
        public void CoolDown()
        {
            _intervalCount = _interval;
            SetState(SkillState.COOLDOWN);
        }
        public void Refresh()
        {
            _intervalCount = 0;
            SetState(SkillState.READY);
        }

        public abstract void Execute();
    }
}