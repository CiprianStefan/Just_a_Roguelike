using UnityEngine;

public enum PlayerDirection
{
    Up,
    Down,
    Left,
    Right,
}

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement Instance;

    [SerializeField]
    private float baseSpeed = 1.5f;
    
    private Animator animator;
    private Rigidbody2D rb2d;
    
    public PlayerDirection playerDirection = PlayerDirection.Up;
    private string soulName = "Wizard_";
    
    private float MovementSpeedMultiplier{ get {return (float)((float)PlayerStatsManager.Instance.stats[StatType.MovementSpeed].StatValue/100 * (1 - GameManager.Instance.playerCombatEntity.MovementSpeedReducedFromDebuffs)); } }

    protected void Start()
    {
        if(Instance == null)
            Instance = this;
        else
            Destroy(this);
        animator = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        soulName = PlayerMisc.Instance.soulContainer.Name == "Base" ? "Base_" : PlayerMisc.Instance.soulContainer.Name + "_";
    }

    protected void FixedUpdate()
    {
        Move();
    }

    protected void Move()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        Vector2 movement = new(y != 0 ? x / Mathf.Sqrt(2) : x,
                                    x != 0 ? y / Mathf.Sqrt(2) : y);

        rb2d.MovePosition(rb2d.position + (float)(baseSpeed * MovementSpeedMultiplier * Time.deltaTime) * movement);

        if(x < 0)
        {
            animator.Play(soulName + "Walk_Left");
            playerDirection = PlayerDirection.Left;
        }
        else if(x > 0)
        {
            animator.Play(soulName + "Walk_Right");
            playerDirection = PlayerDirection.Right;
        }
        else if(y < 0)
        {
            animator.Play(soulName + "Walk_Down");
            playerDirection = PlayerDirection.Down;
        }
        else if(y > 0)
        {
            animator.Play(soulName + "Walk_Up");
            playerDirection = PlayerDirection.Up;
        }
        else
            animator.Play(soulName + "Idle");
    }
}
