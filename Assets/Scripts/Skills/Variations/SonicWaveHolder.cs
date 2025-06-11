using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class SonicWaveHolder : MultiInstanceAbilityHolder
{
    public override void ActivateAbility()
    {
        int instances = Random.Range(1, 4);
        List<PlayerDirection> playerDirections = new List<PlayerDirection>
        {
            PlayerDirection.Up,
            PlayerDirection.Down,
            PlayerDirection.Left,
            PlayerDirection.Right
        };
        for(int i = 0; i < instances; i++)
        {
            PlayerDirection playerDirection = playerDirections.ElementAt(Random.Range(0, playerDirections.Count));
            ProjectileDirections projectileDirections = AbilitiesUtilities.GetDirectionsForFrontalProjectiles(0, 1, playerDirection).ElementAt(0);
            projectileDirections.direction /= 2;
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
            obj.GetComponent<SonicWaveInstance>().UseInstance(projectileDirections);
            obj.transform.position = GameManager.Instance.Player.transform.position;
            obj.SetActive(true);
            playerDirections.Remove(playerDirection);
        }
    }

    public override void DestroyAbilityInstanceAndRenewInstanceOnAbilityModification()
    {
        //AbilitiesUtilities.DestroyAbilitiesInstances(abilityInstances);
    }

}
