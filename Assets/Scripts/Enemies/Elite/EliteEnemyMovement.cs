using UnityEngine;
using System;
using System.Collections;
using Random = UnityEngine.Random;

public class EliteEnemyMovement : MonoBehaviour
{
    protected int ElitePrimaryType { get { return eliteEnemyMisc.ElitePrimaryType; } }
    protected float BaseSpeed { get { return ElitePrimaryType switch {
        1 => eliteEnemyMisc.enemyContainer.baseMovementSpeed * 0.7f,
        2 => eliteEnemyMisc.enemyContainer.baseMovementSpeed * 2f,
        _ => eliteEnemyMisc.enemyContainer.baseMovementSpeed
    }; }}
    protected float AttackRange { get { return eliteEnemyMisc.enemyContainer.attackRange; } }

    protected Rigidbody2D rb2d;
    protected Animator anim;
    protected EliteEnemyMisc eliteEnemyMisc;
    
    public EventHandler OnPlayerIsInAttackRange;

    protected void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        eliteEnemyMisc = GetComponent<EliteEnemyMisc>();
    }

    protected void Start()
    {
        if(ElitePrimaryType == 5)
            StartCoroutine(TeleportEliteShifter());
    }

    protected virtual void FixedUpdate()
    {
        if(ElitePrimaryType != 5)
            Move();
    }

    protected virtual void Move()
    {
        if(eliteEnemyMisc.isDead)
        {
            rb2d.velocity = Vector2.zero;
            return;
        }
        float distance = Vector2.Distance(GameManager.Instance.Player.transform.position, transform.position);
        if(distance > AttackRange)
        {
            float movementSpeedPercentageAfterSlowCalculation = (float)eliteEnemyMisc.combatEntity.MovementSpeedReducedFromDebuffs;
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

    private IEnumerator TeleportEliteShifter()
    {
        while(!eliteEnemyMisc.isDead)
        {
            yield return new WaitForSeconds(1);
            Vector2 randomPosition = new Vector2(Random.Range(-14, 14), Random.Range(-14, 14));
            transform.position = randomPosition;
        }
    }

    protected void OnDestroy()
    {
        StopCoroutine(TeleportEliteShifter());
    }
}
