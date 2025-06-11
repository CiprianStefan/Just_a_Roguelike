using UnityEngine;
using Random = UnityEngine.Random;

public class VampiricStycheInstance : AbilityInstance
{
    private ProjectileDirections projectileDirections;

    protected override void Update()
    {
        base.Update();
        transform.position += ((SkillShotContainer)skillContainer).ProjectileSpeed * Time.deltaTime * projectileDirections.direction;
    }

    protected void OnTriggerEnter2D(Collider2D other)
    {
        if(other.TryGetComponent<EnemyCombatEntity>(out var enemyCombatEntity))
        {
            enemyCombatEntity.ApplyDamage(GameManager.Instance.playerCombatEntity, skillContainer);
            if(Random.Range(1,101) > 90)
                GameManager.Instance.playerCombatEntity.RegenHealth(2);
        }
    }

    public void UseInstance(ProjectileDirections _projectileDirections)
    {
        UseInstance();
        projectileDirections = _projectileDirections;
        transform.rotation = Quaternion.Euler(projectileDirections.rotation);
    }
}
