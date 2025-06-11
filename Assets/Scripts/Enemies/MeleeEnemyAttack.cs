using UnityEngine;
using System;

public class MeleeEnemyAttack : MonoBehaviour
{
    protected float AttackCooldown {get {return enemyMisc.enemyContainer.attackCooldown;}}
    
    private float waitTime = 0;
    
    private CombatEntity dealer;
    private Collider2D playerCollider;
    private EnemyMisc enemyMisc;

    protected void Awake()
    {
        dealer = GetComponent<CombatEntity>();
        enemyMisc = GetComponent<EnemyMisc>();
        enemyMisc.OnObjectUsedFromPool += OnObjectUsedFromPool;
    }

    protected void Update()
    {
        if(playerCollider == null || enemyMisc.isDead)
            return;
        if(waitTime <= 0)
        {
            playerCollider.gameObject.GetComponent<CombatEntity>().ApplyDamage(dealer.PhysicalDamage, dealer);
            waitTime = AttackCooldown;
        }
        else
            waitTime -= Time.deltaTime;
    }

    protected void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.TryGetComponent<CombatEntity>(out var combatEntity))
            playerCollider = other;
    }

    protected void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.TryGetComponent<CombatEntity>(out var combatEntity))
        {
            playerCollider = null;
            waitTime = 0;
        }
    }

    protected void OnObjectUsedFromPool(object sender, EventArgs e)
    {
        playerCollider = null;
        waitTime = 0;
    }

    protected void OnDestroy()
    {
        enemyMisc.OnObjectUsedFromPool -= OnObjectUsedFromPool;
    }
}
