public abstract class HealthAlterationInstance
{
    private readonly double healthAlterationValue;
    public double HealthAlterationValue {get {return healthAlterationValue;}}

    public HealthAlterationInstance(double _helathAlterationValue)
    {
        healthAlterationValue = _helathAlterationValue;
    }
}
