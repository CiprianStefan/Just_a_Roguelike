using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyMisc : MonoBehaviour
{
    public EnemyContainer enemyContainer;
    public bool isDead = false;
    
    public EnemyCombatEntity combatEntity;
    protected Animator anim;
    protected Collider2D coll;

    public static EventHandler OnBossSpawn;
    public static EventHandler<EnemyContainer> OnEnemyDeathStatic;
    public EventHandler OnObjectUsedFromPool;

    protected virtual void Awake()
    {
        combatEntity = GetComponent<EnemyCombatEntity>();
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
        combatEntity.OnEnemyDeath += OnEnemyDeath;
        EnemyGenerationManager.OnWaveCleanUp += OnEnemyCleanUp;
        
    }

    protected virtual void Start()
    {
        if(enemyContainer.enemyType == EnemyType.Boss)
            OnBossSpawn?.Invoke(this, EventArgs.Empty);
    }

    private void OnEnemyDeath(object sender, EventArgs e)
    {
        DisableEnemyRoutine();
        PlayerLevelManager.Instance.AddExperience(enemyContainer.baseExperience * GameManager.Instance.ExperienceRate);
        anim.Play("Death");
        OnEnemyDeathStatic?.Invoke(this, enemyContainer);
        if(enemyContainer.enemyType == EnemyType.Boss)
        {
            Destroy(gameObject, 0.5f);
        }
        else
        {
            StartCoroutine(DisableEnemy());
            if(Random.Range(0, 100) <= 5)
                Instantiate(GameManager.Instance.RegenHealthPickupPrefab, transform.position, Quaternion.identity);
        }
    }

    private void OnEnemyCleanUp(object sender, EventArgs e)
    {
        if(!gameObject.activeSelf)
            return;
        DisableEnemyRoutine();
        anim.Play("Death");
        if(enemyContainer.enemyType == EnemyType.Boss || enemyContainer.enemyType == EnemyType.Elite)
        {
            Destroy(gameObject, 0.5f);
        }
        else
        {
            StartCoroutine(DisableEnemy());
        }
    }

    protected void DisableEnemyRoutine()
    {
        isDead = true;
        coll.enabled = false;
    }

    private IEnumerator DisableEnemy()
    {
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
    }
    
    protected virtual void OnDestroy()
    {
        combatEntity.OnEnemyDeath -= OnEnemyDeath;
        EnemyGenerationManager.OnWaveCleanUp -= OnEnemyCleanUp;
    }

    public void UseObjectFromPool()
    {
        isDead = false;
        coll.enabled = true;
        GetComponent<EntityFXs>().ResetEntityFXs();
        combatEntity.UseObjectFromPool();
        OnObjectUsedFromPool?.Invoke(this, EventArgs.Empty);
    }
}
