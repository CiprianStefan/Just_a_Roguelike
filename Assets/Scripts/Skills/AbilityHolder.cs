using UnityEngine;
using Random = UnityEngine.Random;

public abstract class AbilityHolder : MonoBehaviour
{
    public SkillContainer skillContainer;
    private float RemainingCooldown;
    private float RemaningDuration;
    private bool MulticastTriggered = false;

    public float NumberOfProjectiles
    {
        get
        {
            return skillContainer != null && skillContainer is SkillShotContainer skillShotContainer ? skillShotContainer.NumberOfProjectiles + PlayerStatsManager.Instance.Projectiles : 0;
        }
    }

    public float Cooldown
    {
        get
        {
            return (float)(skillContainer != null && skillContainer.Cooldown != -1 ? skillContainer.Cooldown - (skillContainer.Cooldown * 
                GameManager.Instance.playerCombatEntity.AbilityCooldown / 100) : -1);
        }
    }
    public bool IsOnCooldown
    {
        get
        {
            return RemainingCooldown > 0;
        }
    }

    protected virtual void Start()
    {
        Stat.OnStatValueChange += OnStatValueChange;
        RemainingCooldown = Cooldown;
        MulticastTriggered = false;
        if(Cooldown == -1)
            ActivateAbility();        
    }

    protected virtual void Update()
    {
        if (skillContainer == null)
            return;
        ManageMulticast();
        ManageCooldown();
    }

    protected void ManageCooldown()
    {
        if(Cooldown == -1)
            return;
        if (IsOnCooldown)
            RemainingCooldown -= Time.deltaTime;
        else
        {
            MulticastTriggered = Random.Range(0, 100) < PlayerStatsManager.Instance.Multicast;
            ActivateAbility();
            RemainingCooldown = Cooldown;
        }
    }
    protected void ManageMulticast()
    {
        if (!MulticastTriggered)
            return;
        ActivateAbility();
        MulticastTriggered = false;
    }

    public abstract void ActivateAbility();
    public abstract void DestroyAbilityInstanceAndRenewInstanceOnAbilityModification();

    public void OnStatValueChange(object sender, int value)
    {
        if (sender is Stat stat && stat.StatType == StatType.Area && skillContainer.Type == SkillType.AreaOfEffect)
            DestroyAbilityInstanceAndRenewInstanceOnAbilityModification();
    }

    protected virtual void OnDestroy()
    {
        Stat.OnStatValueChange -= OnStatValueChange;
    }
    
}
