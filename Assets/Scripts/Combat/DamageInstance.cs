public class DamageInstance : HealthAlterationInstance
{
    public readonly SkillContainer skillContainer;
    
    public DamageInstance(SkillContainer _skillContainer, double damageValue) : base(damageValue)
    {
        skillContainer = _skillContainer;
    }

    public DamageInstance(double damageValue) : base(damageValue)
    {}
}
