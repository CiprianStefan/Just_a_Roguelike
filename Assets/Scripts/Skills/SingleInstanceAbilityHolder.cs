using UnityEngine;

public class SingleInstanceAbilityHolder : AbilityHolder
{
    [SerializeField]
    protected GameObject prefab;
    protected GameObject objectInstance;

    protected override void Start()
    {
        base.Start();
    }

    public override void ActivateAbility()
    {
        objectInstance = CreateAbilityInstance();
    }
    
    protected virtual GameObject CreateAbilityInstance()
    {
        GameObject obj = Instantiate(prefab, GameManager.Instance.Player.transform.position, Quaternion.identity);
        obj.GetComponent<AbilityInstance>().Init(skillContainer);
        return obj;
    }

    public override void DestroyAbilityInstanceAndRenewInstanceOnAbilityModification()
    {
        throw new System.NotImplementedException();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        Destroy(objectInstance);
    }

}
