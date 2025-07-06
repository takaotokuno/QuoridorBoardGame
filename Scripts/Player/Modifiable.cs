using System;

namespace Quoridor.Player
{
    public struct ModifiableInt
    {
        public int pBase;
        public int pRelative;
        public int pAbsolute;
        public ModifiableInt(int value)
        {
            pBase = value;
            pRelative = 0;
            pAbsolute = 999;
        }
        public int Value => Math.Max(Math.Min(pBase + pRelative, pAbsolute), 0);
    }

    public struct ModifiableBool
    {
        public bool pBase;
        public bool pModifier;
        public ModifiableBool(bool value)
        {
            pBase = value;
            pModifier = value;
        }
        public bool AndBool => pBase && pModifier;
        public bool OrBool => pBase || pModifier;
    }
}

