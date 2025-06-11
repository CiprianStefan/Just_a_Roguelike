using UnityEngine;

public abstract class AbilityInstance : MonoBehaviour
{
    [SerializeField]
    protected AudioClip sFxSound;
    protected SkillContainer skillContainer;
    protected float remaningDuration;
    protected Vector3 originalScale;
    
    public virtual void Init(SkillContainer _skillContainer)
    {
        skillContainer = _skillContainer;
        originalScale = transform.localScale;
    }

    protected virtual void Update()
    {
        AbilitiesUtilities.ManageDuration(gameObject, ref remaningDuration);
    }

    public virtual void UseInstance()
    {
        transform.position = GameManager.Instance.Player.transform.position;
        remaningDuration = skillContainer.Duration;
        if (skillContainer.Type == SkillType.AreaOfEffect)
            transform.localScale = originalScale * PlayerStatsManager.Instance.Area / 100;
        if (sFxSound != null)
            AudioManager.Instance.PlaySound(sFxSound);
    }
}
