using System.Collections.Generic;
using UnityEngine;

public class VampiricAuraInstance : AbilityInstance
{
    private List<EnemyCombatEntity> enemies = new List<EnemyCombatEntity>();
    private float remainingCooldown = 0.5f;

    protected override void Update()
    {
        transform.position = GameManager.Instance.Player.transform.position;
        remainingCooldown -= Time.deltaTime;
        if(remainingCooldown > 0)
            return;
        remainingCooldown = 0.5f;
        foreach(EnemyCombatEntity enemyCombatEntity in enemies)
        {
            enemyCombatEntity.ApplyDamage(GameManager.Instance.playerCombatEntity, skillContainer);
            if(Random.Range(1,101) > 90)
                GameManager.Instance.playerCombatEntity.RegenHealth(2);
        }
    }

    protected void OnTriggerEnter2D(Collider2D other)
    {
        if(other.TryGetComponent<EnemyCombatEntity>(out var enemyCombatEntity))
            enemies.Add(enemyCombatEntity);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.TryGetComponent<EnemyCombatEntity>(out var enemyCombatEntity))
            enemies.Remove(enemyCombatEntity);
    }
}
