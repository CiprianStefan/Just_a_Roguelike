using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class HalfChimeraAttack : MonoBehaviour
{
    private Animator anim;
    private EnemyMisc enemyMisc;
    private EnemyCombatEntity enemyCombatEntity;
    private HalfChimeraMovement halfChimeraMovement;

    private Collider2D playerCollider;

    [SerializeField]
    private float attackCooldown = 4f;
    private float waitTime = 0;

    [SerializeField]
    private float normalAttackCooldown = 1;
    private float normalAttackWaitTime = 0;

    [SerializeField]
    private GameObject clawAttackPrefab;
    [SerializeField]
    private float clawAttackCooldownBetweenAttacks = 0.1f;
    [SerializeField]
    private int maxClawAttackCount = 3;
    [SerializeField]
    private float clawAttackCooldown = 1f;
    private  bool clawAttack = false;
    private float clawAttackWaitTime = 0;
    private int clawAttackCount = 0;
    private float clawAttackCooldownTime = 0;
    private Vector3 targetPositionForClawAttack;

    [SerializeField]
    private GameObject shadowBitePrefab;
    [SerializeField]
    private float shadowBiteCastTime = 0.4f;
    private bool shadowBite = false;
    private float shadowBiteWaitTime = 0;
    private Vector3 shadowBiteTargetPosition;

    public bool AbilityActive { get => clawAttack || shadowBite; }


    protected void Start()
    {
        anim = GetComponent<Animator>();
        enemyMisc = GetComponent<EnemyMisc>();
        enemyCombatEntity = GetComponent<EnemyCombatEntity>();
        halfChimeraMovement = GetComponent<HalfChimeraMovement>();
        halfChimeraMovement.OnPlayerIsInAttackRange += BeginClawAttack;
        waitTime = attackCooldown;
    }

    protected void Update()
    {
        if(enemyMisc.isDead)
            return;
        if(waitTime <= 0)
        {
            if (Random.Range(1, 101) > 50)
                halfChimeraMovement.BeginCharge();
            else
                BeginShadowBite();
            waitTime = attackCooldown;
        }
        else
            waitTime -= Time.deltaTime;
        if(playerCollider != null)
            if(normalAttackWaitTime <= 0)
            {
                playerCollider.gameObject.GetComponent<CombatEntity>().ApplyDamage(enemyCombatEntity.PhysicalDamage, enemyCombatEntity);
                normalAttackWaitTime = normalAttackCooldown;
            }
            else
                normalAttackWaitTime -= Time.deltaTime;
        if(clawAttackCooldownTime > 0)
            clawAttackCooldownTime -= Time.deltaTime;
        ClawAttack();
        ShadowBite();
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
            normalAttackWaitTime = 0;
        }
    }

    private void BeginShadowBite()
    {
        shadowBite = true;
        shadowBiteWaitTime = shadowBiteCastTime;
        shadowBiteTargetPosition = GameManager.Instance.Player.transform.position;
        Vector3 bossPosition;
        do{
            bossPosition = shadowBiteTargetPosition + new Vector3(Random.Range(-3f,3f), Random.Range(-3f,3f), 0);
        }while(Mathf.Abs(bossPosition.x) > 14 || Mathf.Abs(bossPosition.y) > 14 || Vector2.Distance(bossPosition, GameManager.Instance.Player.transform.position) < 1.5f);
        transform.position = bossPosition;
        GetComponent<SpriteRenderer>().color = new Color(1,1,1,0.5f);
    }

    private void ShadowBite()
    {
        if(shadowBite)
            if(shadowBiteWaitTime > 0)
                shadowBiteWaitTime -= Time.deltaTime;
            else
            {
                GameObject shadowBiteInstance = Instantiate(shadowBitePrefab, shadowBiteTargetPosition, Quaternion.identity);
                shadowBiteInstance.GetComponent<ShadowBite>().Init(enemyCombatEntity);
                GetComponent<SpriteRenderer>().color = new Color(1,1,1,1);
                shadowBiteWaitTime = 0;
                shadowBite = false;
            }
    }

    private void BeginClawAttack(object sender, EventArgs e)
    {
        if(clawAttackCooldownTime > 0 || shadowBite)
            return;
        clawAttack = true;
        clawAttackWaitTime = 0;
        clawAttackCooldownTime = clawAttackCooldown;
        targetPositionForClawAttack = GameManager.Instance.Player.transform.position;
        anim.Play(((GameManager.Instance.Player.transform.position - transform.position).normalized.x > 0 ? "Right" : "Left") +"_Attack");
    }

    private void ClawAttack()
    {
        if(clawAttack)
            if(clawAttackCount < maxClawAttackCount)
            {
                if(clawAttackWaitTime <= 0)
                {
                    ProjectileDirections direction = AbilitiesUtilities.GetDirectionsForTargetedProjectiles(transform.position, targetPositionForClawAttack, 2 + clawAttackCount * 0.5f);  
                    direction.rotation.z += 180 * Mathf.Pow(-1, clawAttackCount);
                    GameObject clawAttackInstance = Instantiate(clawAttackPrefab, transform.position, Quaternion.identity);
                    clawAttackInstance.GetComponent<ClawAttack>().Init(enemyCombatEntity, direction);
                    clawAttackWaitTime = clawAttackCooldownBetweenAttacks;
                    clawAttackCount++;
                }
                else
                    clawAttackWaitTime -= Time.deltaTime;
            }
            else
            {
                clawAttack = false;
                clawAttackCount = 0;
            }
    }

    protected void OnDestroy()
    {
        halfChimeraMovement.OnPlayerIsInAttackRange -= BeginClawAttack;
    }
}
