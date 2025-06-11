using System;
using UnityEngine;

public class EntityFXs : MonoBehaviour
{
    [SerializeField]
    private AudioClip damageTakenSound;
    private ParticleSystem debuffParticles;
    private CombatEntity combatEntity;
    private SpriteRenderer spriteRenderer;
    private readonly float spriteRendererColorFadeDuration = 0.2f;
    private float spriteRendererColorFade;

    protected void Awake()
    {
        spriteRendererColorFade = spriteRendererColorFadeDuration;
        debuffParticles = GetComponentInChildren<ParticleSystem>();
        combatEntity = GetComponent<CombatEntity>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        combatEntity.OnEntityTakeDamage += OnEntityTakeDamage;
        combatEntity.OnCycleDebuff += OnCycleDebuff;
    }

    protected void Update()
    {
        ManageDamageTakeFade();
    }
    
    private void OnEntityTakeDamage(object sender, DamageInstance damageInstance)
    {
        spriteRendererColorFade = 0;
        DisplayDamageManagerUI.Instance.DisplayDamage(transform.position, damageInstance);
        AudioManager.Instance.PlaySound(damageTakenSound);
    }

    protected void OnDestroy()
    {
        combatEntity.OnEntityTakeDamage -= OnEntityTakeDamage;
        combatEntity.OnCycleDebuff += OnCycleDebuff;
    }

    private void ManageDamageTakeFade()
    {
        if(spriteRendererColorFade < spriteRendererColorFadeDuration)
        {
            spriteRenderer.color = Color.Lerp(Color.red, Color.white, spriteRendererColorFade);
            spriteRendererColorFade += Time.deltaTime;
            if(spriteRendererColorFade > spriteRendererColorFadeDuration)
                spriteRenderer.color = Color.white;
        }
    }

    private void OnCycleDebuff(object sender, EventArgs e)
    {
        DebuffInstance debuffInstance = (DebuffInstance)sender;
        Color particleColor = debuffInstance.debuffType switch {
            DebuffType.Bleed => new Color(1, 0, 0, 0.75f),
            DebuffType.Poison => new Color(0.08f, 0.45f, 0.07f, 0.75f),
            DebuffType.Burn => new Color(0.75f, 0.23f, 0, 0.75f),
            DebuffType.Freeze => new Color(0, 0.33f, 0.44f, 0.75f),
            _ => Color.white
        };
        var mainModule = debuffParticles.main;
        mainModule.startColor = particleColor;
        debuffParticles.Clear();
        debuffParticles.Play();
    }

    public void ResetEntityFXs()
    {
        spriteRenderer.color = Color.white;
        spriteRendererColorFade = spriteRendererColorFadeDuration;
    }

}
