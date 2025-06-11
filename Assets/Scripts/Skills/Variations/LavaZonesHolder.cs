using UnityEngine;
using Random = UnityEngine.Random;

public class LavaCratersHolder : MultiInstanceAbilityHolder
{
    private readonly float distanceCalibration = 1.5f;
    
    public override void ActivateAbility()
    {
        Vector3 randomDirection;
        do
        {
            randomDirection = new Vector3(Random.Range(-1,2), Random.Range(-1,2),0);
        } while(randomDirection == Vector3.zero);
        if(randomDirection.x == 0)
            randomDirection.y = randomDirection.y > 0 ? distanceCalibration : -distanceCalibration;
        if(randomDirection.y == 0)
            randomDirection.x = randomDirection.x > 0 ? distanceCalibration : -distanceCalibration;
        Vector3 lastPosition = GameManager.Instance.Player.transform.position;
        for(int i = 0; i < 3; i++)
        {
            lastPosition += randomDirection;
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
            obj.transform.position = lastPosition;
            obj.SetActive(true);
        }
    }

    public override void DestroyAbilityInstanceAndRenewInstanceOnAbilityModification()
    {
        //AbilitiesUtilities.DestroyAbilitiesInstances(abilityInstances);
    }
}
