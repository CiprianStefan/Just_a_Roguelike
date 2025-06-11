using UnityEngine;
using System.Collections.Generic;

public class MultiInstanceAbilityHolder : AbilityHolder
{
    [SerializeField]
    protected GameObject prefab;
    protected List<GameObject> objectsToPool;
    [SerializeField]
    protected int amountToPool = 50;

    protected override void Start()
    {
        base.Start();
        objectsToPool = new List<GameObject>();
        for (int i = 0; i < amountToPool; i++)
        {
            GameObject obj = CreateAbilityInstance();
            obj.SetActive(false);
            objectsToPool.Add(obj);
        }
    }

    public override void ActivateAbility()
    {
        throw new System.NotImplementedException();
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
        foreach (GameObject obj in objectsToPool)
        {
            Destroy(obj);
        }
    }
}

