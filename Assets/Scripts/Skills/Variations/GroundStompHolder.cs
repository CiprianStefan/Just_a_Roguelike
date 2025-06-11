using UnityEngine;

public class GroundStompHolder : MultiInstanceAbilityHolder
{
    public override void ActivateAbility()
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
        obj.transform.position = GameManager.Instance.Player.transform.position + new Vector3(0, -0.5f,0);
        obj.SetActive(true);
    }

    public override void DestroyAbilityInstanceAndRenewInstanceOnAbilityModification()
    {
        //AbilitiesUtilities.DestroyAbilitiesInstance(abilityInstance);
    }
}
