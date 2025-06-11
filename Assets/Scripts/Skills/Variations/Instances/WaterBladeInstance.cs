using UnityEngine;

public class WaterBladeInstance : AbilityInstance
{

    protected override void Update()
    {
        base.Update();
        transform.position = AbilitiesUtilities.UpdateOrbitalProjectilePosition(GameManager.Instance.Player.transform.position, transform.position, ((SkillShotContainer)skillContainer).ProjectileSpeed);
    }

    protected void OnTriggerEnter2D(Collider2D other)
    {
        if(other.TryGetComponent<EnemyCombatEntity>(out var enemyCombatEntity))
        {
            enemyCombatEntity.ApplyDamage(GameManager.Instance.playerCombatEntity, skillContainer);
            enemyCombatEntity.ApplyDebuff(enemyCombatEntity, DebuffType.Freeze);
            gameObject.SetActive(false);
        }
    }

}
