using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    [SerializeField]
    float projectileSpeed = 5;
    [SerializeField]
    private float timeRemain = 10f;
    
    private ProjectileDirections directions;
    private EnemyCombatEntity enemyCombatEntity;

    protected void Update()
    {
        if(timeRemain <= 0)
            Destroy(gameObject);
        timeRemain -= Time.deltaTime;
        transform.position += projectileSpeed * Time.deltaTime * directions.direction;
    }

    public void Init(ProjectileDirections _directions, EnemyCombatEntity _enemyCombatEntity)
    {
        directions = _directions;
        enemyCombatEntity = _enemyCombatEntity;
        transform.rotation = Quaternion.Euler(directions.rotation);
    }

    protected void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.TryGetComponent<CombatEntity>(out var combatEntity))
            combatEntity.ApplyDamage(enemyCombatEntity.PhysicalDamage, enemyCombatEntity);
        Destroy(gameObject);
    }


}
