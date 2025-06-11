using UnityEngine;

public class CleaveInstance : AbilityInstance
{
    [SerializeField]
    private bool destroy;

    protected override void Update()
    {
        if(destroy)
            gameObject.SetActive(false);
        transform.position = GameManager.Instance.Player.transform.position;
    }

    protected void OnTriggerEnter2D(Collider2D other)
    {
        if(other.TryGetComponent<EnemyCombatEntity>(out var enemyCombatEntity))
            enemyCombatEntity.ApplyDamage(GameManager.Instance.playerCombatEntity, skillContainer);
    }

    public override void Init(SkillContainer _skillContainer)
    {
        base.Init(_skillContainer);
        destroy = false;
    }

    public override void UseInstance()
    {
        base.UseInstance();
        destroy = false;
    }
}
