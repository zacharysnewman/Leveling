using System;
using UnityEngine;

namespace Leveling
{
    [Serializable]
    public class LevelingTraits
    {
        [SerializeField]
        private string uniqueKey;
        public string UniqueKey { get => uniqueKey; }

        [SerializeField]
        private int maxLevel;
        public int MaxLevel { get => maxLevel; }

        [SerializeField]
        private int startingXPRequired;
        public int StartingXPRequired { get => startingXPRequired; }

        [SerializeField]
        private int maxLevelXPRequired;
        public int MaxLevelXPRequired { get => maxLevelXPRequired; }

        [SerializeField]
        private AnimationCurve xpCurve;
        public AnimationCurve XPCurve { get => xpCurve; }

        public LevelingTraits()
        {
            this.uniqueKey = "PlayerData";
            this.maxLevel = 20;
            this.startingXPRequired = 100;
            this.maxLevelXPRequired = 300;
            this.xpCurve = new AnimationCurve();
        }

        public void Deconstruct(out string uniqueKey, out int maxLevel, out int startingXPRequired, out int maxLevelXPRequired, out AnimationCurve xpPerLevelCurve)
        {
            (uniqueKey, maxLevel, startingXPRequired, maxLevelXPRequired, xpPerLevelCurve) = (this.uniqueKey, this.maxLevel, this.startingXPRequired, this.maxLevelXPRequired, this.xpCurve);
        }
    }
}