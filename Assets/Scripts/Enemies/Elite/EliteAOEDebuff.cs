using UnityEngine;

public class EliteAOEDebuff : MonoBehaviour
{
    private int eliteDebuffType;
    private PlayerCombatEntity playerCombatEntity;
    private SpriteRenderer spriteRenderer;

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        playerCombatEntity = collision.GetComponent<PlayerCombatEntity>();
        if (playerCombatEntity != null)
        {
            playerCombatEntity.ApplyEliteDebuff(eliteDebuffType);
        }
    }

    protected void OnTriggerExit2D(Collider2D collision)
    {
        playerCombatEntity = collision.GetComponent<PlayerCombatEntity>();
        if (playerCombatEntity != null)
        {
            playerCombatEntity.RemoveEliteDebuff(eliteDebuffType);
        }
    }
    
    public void SetEliteDebuffType(int debuffType)
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        transform.localPosition = Vector3.zero;
        eliteDebuffType = debuffType;
        switch (eliteDebuffType)
        {
            case 1://Electric field
                spriteRenderer.color = new Color(0.12f, 0.65f, 0.12f, 0.72f);
                break;
            case 2://Drain field
                spriteRenderer.color = new Color(0.72f, 0.12f, 0.12f, 0.7f);
                break;
            case 3://Static field
                spriteRenderer.color = new Color(0.45f, 0.45f, 0.45f, 0.72f);
                break;
            case 4://Lethal field
                spriteRenderer.color = new Color(0.45f, 0.15f, 0.30f, 0.72f);
                break;
            default:
                break;
        }
    }
}
