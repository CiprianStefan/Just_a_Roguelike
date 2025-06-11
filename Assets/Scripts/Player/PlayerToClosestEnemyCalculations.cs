using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerToClosestEnemyCalculations : MonoBehaviour
{
    public static PlayerToClosestEnemyCalculations Instance;

    [SerializeField]
    private LayerMask collisionLayerMask;

    private GameObject Player {get{return GameManager.Instance.Player;}}
    
    public Vector3 ClosestEnemyPosition
    {
        get
        {
            GameObject closestEnemy = ClosestEnemySet();
            return closestEnemy == null ? Player.transform.position + new Vector3(Random.Range(-3, 3), Random.Range(-3, 3)) : closestEnemy.transform.position;
        }
    }

    protected void Start()
    {
        if(Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    private GameObject ClosestEnemySet()
    {
        GameObject closestEnemy = null;
        Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(Player.transform.position, 8, collisionLayerMask);
        foreach(Collider2D collider2D in collider2Ds)
        {
            GameObject gObj = collider2D.gameObject;
            if(gObj == null || gObj == gameObject)
                continue;
            if(closestEnemy == null)
                closestEnemy = gObj;
            if(Vector3.Distance(gObj.transform.position, Player.transform.position) < Vector3.Distance(closestEnemy.transform.position, Player.transform.position))
                closestEnemy = gObj;
        }
        return closestEnemy;
    }

    
}
