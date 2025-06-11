using UnityEngine;

public enum AOEType
{
    Aura,
    BindToPlayer,
    Other
}

[CreateAssetMenu(fileName = "AOEContainer", menuName = "Skills/AOEContainer", order = 2)]
public class AOEContainer : SkillContainer
{
    public AOEType AOEType;

    public AOEContainer() : base(SkillType.AreaOfEffect)
    {
    }

}
