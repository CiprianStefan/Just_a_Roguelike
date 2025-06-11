using System;
using UnityEngine;

public class PlayerLevelManager : MonoBehaviour
{
    public static PlayerLevelManager Instance;

    private int level = 1;
    private float experience = 0;
    private float experienceToNextLevel = 100;

    public int Level { get => level; }
    public float Experience { get => experience; }
    public float ExperienceToNextLevel { get => experienceToNextLevel; }

    public static EventHandler OnExperienceChanged;
    public static EventHandler OnLevelChanged;

    
    protected void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    public void AddExperience(float exp)
    {
        float multipliedExp = exp * PlayerStatsManager.Instance.ExperienceGain / 100;
        float newExperience = experience + multipliedExp;
        experience = newExperience;
        while (experience >= experienceToNextLevel)
            LevelUp();
        OnExperienceChanged?.Invoke(this, EventArgs.Empty);
    }

    private void LevelUp()
    {
        level++;
        experience -= experienceToNextLevel;
        experienceToNextLevel = (100 + level) * Mathf.Sqrt(level) * level / 6;
        OnLevelChanged?.Invoke(this, EventArgs.Empty);
    }

}
