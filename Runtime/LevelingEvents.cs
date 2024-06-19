using System;
using UnityEngine.Events;

namespace Leveling
{
    [Serializable]
    public class LevelingEvents
    {
        public UnityEvent<int> OnEarnXPEvent = new UnityEvent<int>();
        public UnityEvent<int> OnTotalXPChangeEvent = new UnityEvent<int>();
        public UnityEvent<int> OnCurrentXPChangeEvent = new UnityEvent<int>();
        public UnityEvent<float> OnCurrentXPChangeNormalizedEvent = new UnityEvent<float>();
        public UnityEvent<int> OnLevelChangeEvent = new UnityEvent<int>();
        public UnityEvent<int> OnXPRequiredChangeEvent = new UnityEvent<int>();
        public UnityEvent OnMaxLevelUpEvent = new UnityEvent();
        public UnityEvent<bool> IsMaxLevelEvent = new UnityEvent<bool>();
        public UnityEvent<bool> NotMaxLevelEvent = new UnityEvent<bool>();
    }
}