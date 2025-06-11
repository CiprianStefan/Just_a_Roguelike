using UnityEngine;

public class VulcanoInstance : AbilityInstance
{
    protected void OnTriggerEnter2D(Collider2D other)
    {
        if(other.TryGetComponent<EnemyCombatEntity>(out var enemyCombatEntity))
        {
        enemyCombatEntity.ApplyDamage(GameManager.Instance.playerCombatEntity, skillContainer);
        enemyCombatEntity.ApplyDebuff(enemyCombatEntity, DebuffType.Burn);
        }
    }
}
