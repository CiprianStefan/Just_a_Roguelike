
public class VampiricAuraHolder : SingleInstanceAbilityHolder
{
    public override void DestroyAbilityInstanceAndRenewInstanceOnAbilityModification()
    {
        objectInstance.GetComponent<AbilityInstance>().UseInstance();
    }
}
