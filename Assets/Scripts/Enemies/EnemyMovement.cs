using System;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    protected float BaseSpeed { get { return enemyMisc.enemyContainer.baseMovementSpeed; } }
    protected float AttackRange { get { return enemyMisc.enemyContainer.attackRange; } }
    
    protected Rigidbody2D rb2d;
    protected Animator anim;
    protected EnemyMisc enemyMisc;
    
    public EventHandler OnPlayerIsInAttackRange;

    protected void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        enemyMisc = GetComponent<EnemyMisc>();
    }

    protected virtual void FixedUpdate()
    {
        Move();
    }

    protected virtual void Move()
    {
        if(enemyMisc.isDead)
        {
            rb2d.velocity = Vector2.zero;
            return;
        }
        float distance = Vector2.Distance(GameManager.Instance.Player.transform.position, transform.position);
        if(distance > AttackRange)
        {
            float movementSpeedPercentageAfterSlowCalculation = (float)enemyMisc.combatEntity.MovementSpeedReducedFromDebuffs;
            float movementSpeedMultiplier = (100 + (100 * GameManager.Instance.GameTimeInMinutes / 40)) /100;
            float appliedSpeedMultiplier = movementSpeedMultiplier - (movementSpeedMultiplier * movementSpeedPercentageAfterSlowCalculation / 100);
            Vector2 velocity = appliedSpeedMultiplier * BaseSpeed * (GameManager.Instance.Player.transform.position - transform.position).normalized; 
            rb2d.velocity = velocity;
            anim.Play(velocity.x > 0 ? "Right" : "Left");
        }
        else
        {
            rb2d.velocity = Vector2.zero;
            OnPlayerIsInAttackRange?.Invoke(this, EventArgs.Empty);
        }
    }
    
}
