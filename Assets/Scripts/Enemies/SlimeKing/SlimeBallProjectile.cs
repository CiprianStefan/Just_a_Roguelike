using UnityEngine;

public class SlimeBallProjectile : MonoBehaviour
{
    private int attackMode;
    private float speed;
    private Vector3 direction;
    private Vector3 originPotision;
    private int rotationDirection;
    private float distanceFromOrigin;
    public bool destroyed = false;
    public bool isdestroying = false;
    private float activeTime;

    [SerializeField]
    private Sprite sprite;

    private SpriteRenderer spriteRenderer;
    private Animator anim;
    private EnemyCombatEntity enemyCombatEntity;

    protected void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    protected void Update()
    {
        if(!isdestroying)
            switch(attackMode)
            {
                case 1:
                    transform.position = AbilitiesUtilities.UpdateOrbitalProjectilePosition(originPotision, transform.position, speed, distanceFromOrigin, rotationDirection);
                    break;
                case 2:
                    transform.position += speed * Time.deltaTime * direction;
                    break;
            }
        activeTime -= Time.deltaTime;
        if(activeTime <= 0)
        {
            isdestroying = true;
            anim.Play("DestroySlimeball");
        }
        if(destroyed)
            Destroy(gameObject);
    }

    protected void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.TryGetComponent<PlayerCombatEntity>(out var combatEntity))
        {
            combatEntity.ApplyDamage(enemyCombatEntity.PhysicalDamage, enemyCombatEntity);
            isdestroying = true;
            anim.Play("DestroySlimeball");
        }
    }

    public void Init(ProjectileDirections _projectileDirections, float _speed, EnemyCombatEntity _enemyCombatEntity, float _activeTime)
    {
        enemyCombatEntity = _enemyCombatEntity;
        spriteRenderer.sprite = sprite;
        destroyed = false;
        isdestroying = false;
        attackMode = 2;
        speed = _speed;
        transform.rotation = Quaternion.Euler(_projectileDirections.rotation);
        direction = _projectileDirections.direction * speed;
        activeTime = _activeTime;
    }

    public void Init(Vector3 _position, Vector3 _originPosition, float _speed, EnemyCombatEntity _enemyCombatEntity, float _distanceFromOrigin, float _activeTime, int _rotationDirection)
    {
        enemyCombatEntity = _enemyCombatEntity;
        spriteRenderer.sprite = sprite;
        destroyed = false;
        isdestroying = false;
        attackMode = 1;
        speed = _speed;
        transform.position = _position;
        originPotision = _originPosition;
        distanceFromOrigin = _distanceFromOrigin;
        activeTime = _activeTime;
        rotationDirection = _rotationDirection;
    }
}
