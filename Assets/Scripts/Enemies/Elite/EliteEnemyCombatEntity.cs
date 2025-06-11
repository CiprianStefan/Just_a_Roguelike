using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EliteEnemyCombatEntity : EnemyCombatEntity
{
    protected EliteEnemyMisc EliteEnemyMisc => (EliteEnemyMisc) enemyMisc;
    protected int ElitePrimaryType => EliteEnemyMisc.ElitePrimaryType;

    public override double Health { get{return ElitePrimaryType switch 
    {
        1 => Mathf.RoundToInt(enemyMisc.enemyContainer.baseHealth * (1 + enemyEmpowerment / 2.5f)) * 10,
        3 => 100,
        _ => Mathf.RoundToInt(enemyMisc.enemyContainer.baseHealth * (1 + enemyEmpowerment / 2.5f))
    }; }}

    public override double Block { get{return ElitePrimaryType switch 
    {
        4 => 100 * Mathf.Max(GameManager.Instance.GameTimeInMinutes,1),
        _ => enemyMisc.enemyContainer.baseBlock
    }; }}

    protected override void Start()
    {
        base.Start();
        if(ElitePrimaryType == 6)
            StartCoroutine(RegenFullHealthRefresher());
    }

    public override DamageInstance ApplyDamage(double damageValue, CombatEntity dealer = null, bool canBeDodged = false, bool canBeCrit = false)
    {
        if(ElitePrimaryType == 3)
        {
            damageValue = 1;
            DamageInstance damageInstance = new DamageInstance(1);
            HealthAlterationQ.Enqueue(damageInstance);
            if(dealer != null)
                dealer.RegenHealth(damageInstance);
            return damageInstance;
        }
        return base.ApplyDamage(damageValue, dealer, canBeDodged, canBeCrit);
    }

    public override DamageInstance ApplyDamage(CombatEntity dealer, SkillContainer skillContainer)
    {
        if(ElitePrimaryType == 3)
        {
            DamageInstance damageInstance = new DamageInstance(1);
            HealthAlterationQ.Enqueue(damageInstance);
            dealer.RegenHealth(damageInstance);
            return damageInstance;
        }
        return base.ApplyDamage(dealer, skillContainer);
    }

    private IEnumerator RegenFullHealthRefresher()
    {
        while(true)
        {
            yield return new WaitForSeconds(2);
            RegenHealth(Health);
        }
    }

    public override bool DamageIsBlocked(float damage)
    {
        if(ElitePrimaryType == 4)
            return damage < Block;
        return base.DamageIsBlocked(damage);
    }

    protected void OnDestroy()
    {
        if(ElitePrimaryType == 6)
            StopCoroutine(RegenFullHealthRefresher());
    }
}
