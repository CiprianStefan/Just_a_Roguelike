using System;
using UnityEngine;

public class RangedEnemyAttack : MonoBehaviour
{
    protected float AttackCooldown {get {return enemyMisc.enemyContainer.attackCooldown;}}
    
    private float waitTime = 0;
    
    [SerializeField]
    private GameObject projectile;
    
    private EnemyMisc enemyMisc;
    private EnemyCombatEntity enemyCombatEntity;
    private Animator anim;

    protected void Awake()
    {
        enemyCombatEntity = GetComponent<EnemyCombatEntity>();
        anim = GetComponent<Animator>();
        enemyMisc = GetComponent<EnemyMisc>();
        GetComponent<EnemyMovement>().OnPlayerIsInAttackRange += Attack;
    }

    private void Attack(object sender, EventArgs e)
    {
        if(enemyMisc.isDead)
            return;
        if(waitTime <= 0)
        {
            ProjectileDirections projectileDirections = AbilitiesUtilities.GetDirectionsForTargetedProjectiles(transform.position, GameManager.Instance.Player.transform.position);
            GameObject projectileInstance = Instantiate(projectile, transform.position, Quaternion.identity);
            projectileInstance.GetComponent<EnemyProjectile>().Init(projectileDirections, enemyCombatEntity);
            anim.Play((projectileDirections.direction.x < 0 ? 
                    "Left" : 
                    "Right")
                    + "_Attack");
            waitTime = AttackCooldown;
        }
        else
            waitTime -= Time.deltaTime;
    }

    protected void OnDestroy()
    {
        GetComponent<EnemyMovement>().OnPlayerIsInAttackRange -= Attack;
    }
}
