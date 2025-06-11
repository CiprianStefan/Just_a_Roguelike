using System.Collections.Generic;
using UnityEngine;

public class SlimeKingEnemyAttack : MonoBehaviour
{
    [SerializeField]
    private float attackCooldown = 5f;
    [SerializeField]
    private float attack1BaseWaitTime = 1f;
    [SerializeField]
    private float attack2BaseWaitTime = 0.4f;
    
    private float waitTime = 0;
    private int attack1Charges = 0;
    private float attack1WaitTime = 0;
    private int attack2Charges = 0;
    private float attack2WaitTime = 0;

    [SerializeField]
    private GameObject slimeBallPrefab;

    private Animator anim;
    private EnemyMisc enemyMisc;
    private EnemyCombatEntity enemyCombatEntity;

    protected void Start()
    {
        anim = GetComponent<Animator>();
        enemyMisc = GetComponent<EnemyMisc>();
        enemyCombatEntity = GetComponent<EnemyCombatEntity>();
        waitTime = attackCooldown;
    }

    protected void Update()
    {
        if(enemyMisc.isDead)
            return;
        if(waitTime <= 0)
        {
            Attack(Random.Range(0, 3));
            waitTime = attackCooldown;
        }
        else
            waitTime -= Time.deltaTime;
        HandleAttack1();
        HandleAttack2();
    }

    private void Attack(int attackType)
    {
        switch(attackType)
        {
            case 0:
                anim.Play("Attack1");
                int rotationDirection = -1;
                for (int i = 3; i <= 12; i+=3)
                {
                    rotationDirection *= -1;
                    List<Vector3> directionsAttack1 = AbilitiesUtilities.GetOrbitalProjectilesSpawnLocation(transform.position, 12, i);
                    for (int j = 0; j < directionsAttack1.Count; j++)
                    {
                        GameObject slimeBall = Instantiate(slimeBallPrefab, directionsAttack1[j], Quaternion.identity);
                        slimeBall.GetComponent<SlimeBallProjectile>().Init(directionsAttack1[j], transform.position, 0.2f * i/2, enemyCombatEntity, i, 4.5f, rotationDirection);
                    }
                }
                break;
            case 1:
                attack1Charges = -1;
                attack1WaitTime = 0;
                break;
            case 2:
                attack2Charges = -1;
                attack2WaitTime = attack2BaseWaitTime;
                break;
        }
    }

    private void HandleAttack1()
    {
        if(attack1Charges < 0 && attack1Charges >= -3)
        {
            if(attack1WaitTime <= 0)
            {
                anim.Play("Attack2");
                List<ProjectileDirections> directionsAttack2 = AbilitiesUtilities.GetDirectionsForMultipleTargetedProjectiles(transform.position, GameManager.Instance.Player.transform.position, 5);
                for (int i = 0; i < directionsAttack2.Count; i++)
                {
                    GameObject slimeBall = Instantiate(slimeBallPrefab, transform.position, Quaternion.identity);
                    slimeBall.GetComponent<SlimeBallProjectile>().Init(directionsAttack2[i], 3f, enemyCombatEntity, 5);
                }
                attack1Charges--;
                attack1WaitTime = attack1BaseWaitTime;
            }
            else
                attack1WaitTime -= Time.deltaTime;
        }
        else if(attack1Charges == -4)
            attack1Charges = 0;
    }

    private void HandleAttack2()
    {
        if(attack2Charges < 0 && attack2Charges >= -3)
        {
            if(attack2WaitTime <= 0)
            {
                anim.Play("Attack2");
                List<ProjectileDirections> directionsAttack3 = AbilitiesUtilities.GetOrbitalProjectilesSpawnDirections(6 * Mathf.Abs(attack2Charges));
                for (int j = 0; j < directionsAttack3.Count; j++)
                {
                    GameObject slimeBall = Instantiate(slimeBallPrefab, transform.position + directionsAttack3[j].direction, Quaternion.identity);
                    slimeBall.GetComponent<SlimeBallProjectile>().Init(directionsAttack3[j], 4f + attack2Charges/1.2f, enemyCombatEntity, 5);
                }
                attack2Charges--;
                attack2WaitTime = attack2BaseWaitTime;
            }
            else
                attack2WaitTime -= Time.deltaTime;
        }
        else if(attack2Charges == -4)
            attack2Charges = 0;
    }

}
