using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CutsBarrageHolder : MultiInstanceAbilityHolder
{
    private float spawnCooldown = 0.1f;
    private float remainSpawnCooldown = 0;
    private List<ProjectileDirections> projectilesDirections = new List<ProjectileDirections>();

    public override void ActivateAbility()
    {
        List<ProjectileDirections> newEntry = AbilitiesUtilities.GetOrbitalProjectilesSpawnDirections(NumberOfProjectiles, 0.4f);
        projectilesDirections = projectilesDirections.Concat(newEntry).ToList();
    }

    protected override void Update()
    {
        base.Update();
        if(remainSpawnCooldown <= 0 && projectilesDirections.Count > 0)
        {
            Vector3 playerPosition = GameManager.Instance.Player.transform.position;
            ProjectileDirections projectileDirections = projectilesDirections.First();

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
            obj.GetComponent<CutsBarrageInstance>().UseInstance(projectileDirections);
            obj.transform.position = playerPosition;
            obj.SetActive(true);

            projectilesDirections.Remove(projectileDirections);

            remainSpawnCooldown = spawnCooldown;
        }
        else
            remainSpawnCooldown -= Time.deltaTime;
    }

    public override void DestroyAbilityInstanceAndRenewInstanceOnAbilityModification()
    {
        //AbilitiesUtilities.DestroyAbilitiesInstances(abilityInstances);
    }



}
