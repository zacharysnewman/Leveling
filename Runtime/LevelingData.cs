using UnityEngine;

namespace Leveling
{
    [CreateAssetMenu(fileName = "LevelingData", menuName = "LevelUp/LevelingData")]
    public class LevelingData : ScriptableObject
    {
        [SerializeField]
        private LevelingTraits traits;
        public LevelingTraits Traits { get => traits; }

        public LevelingData()
        {
            this.traits = new LevelingTraits();
        }
    }
}
