using System.Collections.Generic;
using UnityEngine;

public class SpearsHolder : MultiInstanceAbilityHolder
{
    public override void ActivateAbility()
    {
        List<ProjectileDirections> projectilesDirections = AbilitiesUtilities.GetOrbitalProjectilesSpawnDirections(NumberOfProjectiles);
        foreach(ProjectileDirections projectileDirections in projectilesDirections)
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
            obj.transform.position = GameManager.Instance.Player.transform.position;
            obj.GetComponent<SpearsInstance>().UseInstance(projectileDirections);
            obj.SetActive(true);
        }
    }

    public override void DestroyAbilityInstanceAndRenewInstanceOnAbilityModification()
    {
        //AbilitiesUtilities.DestroyAbilitiesInstances(abilityInstances);
    }
}
