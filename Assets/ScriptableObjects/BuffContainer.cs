using UnityEngine;

[CreateAssetMenu(fileName = "BuffContainer", menuName = "Skills/BuffContainer", order = 3)]
public class BuffContainer : SkillContainer
{
    public BuffContainer() : base(SkillType.Buff)
    {
    }
}
