
public class PoisonousAuraHolder : SingleInstanceAbilityHolder
{
    public override void DestroyAbilityInstanceAndRenewInstanceOnAbilityModification()
    {
        objectInstance.GetComponent<AbilityInstance>().UseInstance();
    }
}
