using UnityEngine;

public class SwordWaveInstance : AbilityInstance
{
    private float timeRemain;
    private ProjectileDirections projectileDirections;
    private Vector3 startSize;
    private Vector3 endSize;

    protected override void Update()
    {
        timeRemain -= Time.deltaTime;
        transform.position += ((SkillShotContainer)skillContainer).ProjectileSpeed * Time.deltaTime * projectileDirections.direction;
        transform.localScale = Vector3.Lerp(startSize, endSize, 1 - (timeRemain / skillContainer.Duration));
        if(timeRemain <= 0)
            gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.TryGetComponent<EnemyCombatEntity>(out var enemyCombatEntity))
            enemyCombatEntity.ApplyDamage(GameManager.Instance.playerCombatEntity, skillContainer);
    }

    public override void Init(SkillContainer _skillContainer)
    {
        base.Init(_skillContainer);
        startSize = transform.localScale;
        endSize = transform.localScale * 2;
    }

    public override void UseInstance()
    {
        base.UseInstance();
        projectileDirections = AbilitiesUtilities.GetDirectionsForTargetedProjectiles(transform.position, GameManager.Instance.TargetPosition, 0.2f);
        transform.SetPositionAndRotation(GameManager.Instance.Player.transform.position + projectileDirections.direction, Quaternion.Euler(projectileDirections.rotation + new Vector3(0,0,90)));
        timeRemain = skillContainer.Duration;
    }
}
