using UnityEngine;

public class SonicWaveInstance : AbilityInstance
{
    [SerializeField]
    protected bool destroy;
    protected ProjectileDirections projectileDirections;

    protected override void Update()
    {
        if(destroy)
            gameObject.SetActive(false);
        transform.position = GameManager.Instance.Player.transform.position + projectileDirections.direction;
    }

    protected void OnTriggerEnter2D(Collider2D other)
    {
        if(other.TryGetComponent<EnemyCombatEntity>(out var enemyCombatEntity))
        {
            enemyCombatEntity.ApplyDamage(GameManager.Instance.playerCombatEntity, skillContainer);
            enemyCombatEntity.ApplyDebuff(enemyCombatEntity, DebuffType.Freeze);
        }
    }

    public override void Init(SkillContainer _skillContainer)
    {
        base.Init(_skillContainer);
        destroy = false;
    }

    public void UseInstance(ProjectileDirections _projectileDirections)
    {
        base.UseInstance();
        projectileDirections = _projectileDirections;
        destroy = false;
        transform.SetPositionAndRotation(GameManager.Instance.Player.transform.position + projectileDirections.direction, Quaternion.Euler(projectileDirections.rotation));
    }
}
