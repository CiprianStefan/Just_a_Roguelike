using UnityEngine;

public class GrimmStycheInstance : AbilityInstance
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
            enemyCombatEntity.ApplyDebuff(enemyCombatEntity, DebuffType.Bleed);
            enemyCombatEntity.ApplyDebuff(enemyCombatEntity, DebuffType.Burn);
        }
    }

    public void UseInstance(ProjectileDirections _projectileDirections)
    {
        UseInstance();
        projectileDirections = _projectileDirections;
        transform.rotation = Quaternion.Euler(projectileDirections.rotation);
    }
}
