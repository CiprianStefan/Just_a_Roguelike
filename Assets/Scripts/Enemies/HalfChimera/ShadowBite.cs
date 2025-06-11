using UnityEngine;

public class ShadowBite : MonoBehaviour
{
    public bool destroy = false;

    private Animator anim;
    private EnemyCombatEntity enemyCombatEntity;

    protected void Awake()
    {
        anim = GetComponent<Animator>();
    }

    protected void Update()
    {
        if(destroy)
            Destroy(gameObject);
    }

    protected void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.TryGetComponent<PlayerCombatEntity>(out var combatEntity))
            combatEntity.ApplyDamage(enemyCombatEntity.PhysicalDamage, enemyCombatEntity);
    }

    public void Init(EnemyCombatEntity _enemyCombatEntity)
    {
        enemyCombatEntity = _enemyCombatEntity;
        destroy = false;
        anim.Play("ShadowBite");
    }
}
