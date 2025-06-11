using UnityEngine;

public class BlackNovaArtileryInstance : AbilityInstance
{
    private bool explode = false;
    
    [SerializeField]
    private bool destroy = false;
    
    [SerializeField]
    private AudioClip explosionSFx;
    [SerializeField]
    private Sprite projectileSprite;

    private Animator anim;
    private SpriteRenderer spriteRenderer;
    private ProjectileDirections projectileDirections;
    
    protected override void Update()
    {
        base.Update();
        if(explode)
        {
            anim.Play("BlackNovaArtilery");
            AudioManager.Instance.PlaySound(explosionSFx);
        }
        else
            transform.position += ((SkillShotContainer)skillContainer).ProjectileSpeed * Time.deltaTime * projectileDirections.direction;
        if(destroy)
            gameObject.SetActive(false);
    }

    protected void OnTriggerEnter2D(Collider2D other)
    {
        if(other.TryGetComponent<EnemyCombatEntity>(out var enemyCombatEntity))
            enemyCombatEntity.ApplyDamage(GameManager.Instance.playerCombatEntity, skillContainer);
        explode = true;
    }

    public override void Init(SkillContainer _skillContainer)
    {
        base.Init(_skillContainer);
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void UseInstance(ProjectileDirections _projectileDirections)
    {
        UseInstance();
        spriteRenderer.sprite = projectileSprite;
        projectileDirections = _projectileDirections;
        transform.rotation = Quaternion.Euler(projectileDirections.rotation);
        explode = false;
        destroy = false;
    }
}
