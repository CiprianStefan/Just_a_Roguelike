using UnityEngine;

public class HealthRegenPickup : MonoBehaviour
{
    [SerializeField]
    private bool pickedUp;
    private Animator anim;

    protected void Start()
    {
        pickedUp = false;
        anim = GetComponent<Animator>();
    }

    protected void Update()
    {
        if (pickedUp)
            Destroy(gameObject);
    }

    protected void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<PlayerCombatEntity>(out var playerCombatEntity) && !pickedUp)
        {
            playerCombatEntity.RegenHealth(playerCombatEntity.Health*0.1f);
            anim.Play("HealthRegenPickupTake");
        }
    }
}
