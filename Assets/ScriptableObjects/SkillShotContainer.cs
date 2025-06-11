using UnityEngine;

public enum SkillShotLaunchType
{
    Frontal,
    MouseTarget,
    Other,
    Left_Right,
    Up_Down,
    Orbital
}

[CreateAssetMenu(fileName = "SkillShotContainer", menuName = "Skills/SkillShotContainer", order = 1)]
public class SkillShotContainer : SkillContainer
{
    public int NumberOfProjectiles;
    public float ProjectileSpeed;
    public SkillShotLaunchType LaunchType;
    
    public SkillShotContainer() : base(SkillType.SkillShot)
    {
    }
}
