using System.Collections.Generic;
using UnityEngine;

public class WaterBladesHolder : MultiInstanceAbilityHolder
{
    protected override GameObject CreateAbilityInstance()
    {
        GameObject obj = Instantiate(prefab, GameManager.Instance.Player.transform.position, Quaternion.identity, transform);
        obj.GetComponent<AbilityInstance>().Init(skillContainer);
        return obj;
    }

    public override void ActivateAbility()
    {
        List<Vector3> projectilesDirections = AbilitiesUtilities.GetOrbitalProjectilesSpawnLocation(GameManager.Instance.Player.transform.position, NumberOfProjectiles);;
        foreach(Vector3 projectileDirections in projectilesDirections)
        {
            GameObject obj = null;
            foreach (GameObject objToPool in objectsToPool)
            {
                if (!objToPool.activeInHierarchy)
                {
                    obj = objToPool;
                    break;
                }
            }
            if (obj == null)
            {
                obj = CreateAbilityInstance();
                objectsToPool.Add(obj);
            }
            obj.GetComponent<AbilityInstance>().UseInstance();
            obj.transform.position = projectileDirections;
            obj.SetActive(true);
        }
    }

    protected override void Update()
    {
        base.Update();
        transform.position = GameManager.Instance.Player.transform.position;
    }

    public override void DestroyAbilityInstanceAndRenewInstanceOnAbilityModification()
    {
        //AbilitiesUtilities.DestroyAbilitiesInstances(abilityInstances);
    }

}
