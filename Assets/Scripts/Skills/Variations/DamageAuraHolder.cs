
public class DamageAuraHolder : SingleInstanceAbilityHolder
{
    public override void DestroyAbilityInstanceAndRenewInstanceOnAbilityModification()
    {
        objectInstance.GetComponent<AbilityInstance>().UseInstance();
    }
}
