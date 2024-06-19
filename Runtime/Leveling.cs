using UnityEngine;

namespace Leveling
{
    public class Leveling : MonoBehaviour
    {
        private const string XP_REQUIRED_KEY = "_XPRequired";
        private const string CURRENT_LEVEL_KEY = "_CurrentLevel";
        private const string CURRENT_XP_KEY = "_CurrentXP";
        private const string TOTAL_XP_KEY = "_TotalXP";

        public LevelingData levelingData;
        public bool saveOnQuit;
        public bool saveOnDestroy;
        public bool loadOnStart;
        public LevelingEvents events = new LevelingEvents();

        private int currentLevel;
        public int CurrentLevel
        {
            get => currentLevel;
            set
            {
                currentLevel = value;
                bool isMaxLevel = currentLevel == levelingData.Traits.MaxLevel;
                events.OnLevelChangeEvent?.Invoke(currentLevel);
                events.IsMaxLevelEvent?.Invoke(isMaxLevel);
                events.NotMaxLevelEvent?.Invoke(!isMaxLevel);
            }
        }
        private int currentXP;
        public int CurrentXP
        {
            get => currentXP;
            set
            {
                currentXP = value;
                events.OnCurrentXPChangeEvent?.Invoke(currentXP);
                events.OnCurrentXPChangeNormalizedEvent?.Invoke((float)currentXP / (float)xpRequired);
            }
        }
        private int totalXP;
        public int TotalXP
        {
            get => totalXP;
            set
            {
                totalXP = value;
                events.OnTotalXPChangeEvent?.Invoke(totalXP);
            }
        }

        private int xpRequired;
        public int XPRequired
        {
            get => xpRequired;
            set
            {
                xpRequired = value;
                events.OnXPRequiredChangeEvent?.Invoke(xpRequired);
            }
        }

        private void Start()
        {
            if (loadOnStart)
            {
                Load();
            }
            else
            {
                ResetLevel();
            }
        }

        private void OnApplicationQuit()
        {
            if (saveOnQuit)
            {
                Save();
            }
        }

        private void OnDestroy()
        {
            if (saveOnDestroy)
            {
                Save();
            }
        }

        private void UpdateAll(int currentLevel, int xpRequired, int currentXP, int totalXP)
        {
            CurrentLevel = currentLevel;
            XPRequired = xpRequired;
            CurrentXP = currentXP;
            TotalXP = totalXP;
        }

        public void Save()
        {
            string UniqueKey = levelingData.Traits.UniqueKey;
            PlayerPrefs.SetInt(UniqueKey + CURRENT_LEVEL_KEY, CurrentLevel);
            PlayerPrefs.SetInt(UniqueKey + CURRENT_XP_KEY, CurrentXP);
            PlayerPrefs.SetInt(UniqueKey + TOTAL_XP_KEY, TotalXP);
            PlayerPrefs.SetInt(UniqueKey + XP_REQUIRED_KEY, XPRequired);
            PlayerPrefs.Save();
        }

        public void Load()
        {
            string UniqueKey = levelingData.Traits.UniqueKey;
            if (PlayerPrefs.HasKey(UniqueKey + CURRENT_LEVEL_KEY))
            {
                UpdateAll(
                    currentLevel: PlayerPrefs.GetInt(UniqueKey + CURRENT_LEVEL_KEY),
                    xpRequired: PlayerPrefs.GetInt(UniqueKey + XP_REQUIRED_KEY),
                    currentXP: PlayerPrefs.GetInt(UniqueKey + CURRENT_XP_KEY),
                    totalXP: PlayerPrefs.GetInt(UniqueKey + TOTAL_XP_KEY));
            }
            else
            {
                ResetLevel();
            }
        }

        public void ResetLevel()
        {
            (_, _, int StartingXPRequired, _, _) = levelingData.Traits;
            UpdateAll(currentLevel: 1, xpRequired: StartingXPRequired, currentXP: 0, totalXP: 0);
        }

        public void AddXP(int xp)
        {
            int maxLevel = levelingData.Traits.MaxLevel;
            int newCurrentXP = CurrentXP + xp;
            int newTotalXP = TotalXP + xp;
            int newCurrentLevel = CurrentLevel;
            int newXPRequired = XPRequired;

            if (CurrentLevel >= maxLevel)
            {
                return;
            }

            events.OnEarnXPEvent?.Invoke(xp);

            while (newCurrentXP >= newXPRequired && newCurrentLevel < maxLevel)
            {
                newCurrentXP -= newXPRequired;
                newCurrentLevel++;
                newXPRequired = CalculateXPRequiredForNextLevel(newCurrentLevel);
            }

            if (newCurrentLevel >= maxLevel)
            {
                newXPRequired = CalculateXPRequiredForNextLevel(newCurrentLevel);
                newCurrentXP = 0;
                newTotalXP = CalculateTotalXPForLevel(maxLevel);
                events.OnMaxLevelUpEvent?.Invoke();
            }

            UpdateAll(newCurrentLevel, newXPRequired, newCurrentXP, newTotalXP);
        }

        private int CalculateXPRequiredForNextLevel(int level)
        {
            (_, int MaxLevel, int StartingXPRequired, int MaxLevelXPRequired, AnimationCurve XPCurve) = levelingData.Traits;
            float t = (float)(level) / (MaxLevel - 1);
            float curveValue = XPCurve.Evaluate(t);
            int xpRequired = (int)Mathf.Lerp(StartingXPRequired, MaxLevelXPRequired, curveValue);

            return xpRequired;
        }

        private int CalculateTotalXPForLevel(int level)
        {
            (_, int MaxLevel, int StartingXPRequired, int MaxLevelXPRequired, AnimationCurve XPCurve) = levelingData.Traits;
            int totalXP = 0;

            for (int i = 1; i <= level; i++)
            {
                float t = (float)(i) / (MaxLevel - 1);
                float curveValue = XPCurve.Evaluate(t);
                int xpRequiredForLevel = (int)Mathf.Lerp(StartingXPRequired, MaxLevelXPRequired, curveValue);
                totalXP += xpRequiredForLevel;
            }

            return totalXP;
        }
    }
}
