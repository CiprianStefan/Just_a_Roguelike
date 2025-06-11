using UnityEngine;

public class HalfChimeraMovement : EnemyMovement
{
    private bool isCharging;
    [SerializeField]
    private float chargeSpeed = 20f;
    [SerializeField]
    private float chargeTime = 0.5f;
    [SerializeField]
    private float chargeChannelTime = 2f;
    private float chargeTimeRemain;
    private float chargeChannelTimeRemain;
    private Vector3 chargeDirection;

    private HalfChimeraAttack halfChimeraAttack;

    protected void Start()
    {
        isCharging = false;
        halfChimeraAttack = GetComponent<HalfChimeraAttack>();
    }

    protected override void FixedUpdate()
    {
        if(isCharging)
            Charge();
        else
            if(!halfChimeraAttack.AbilityActive)
                Move();
    }

    protected override void Move()
    {
        base.Move();
    }

    public void BeginCharge()
    {
        isCharging = true;
        chargeTimeRemain = chargeTime;
        chargeChannelTimeRemain = chargeChannelTime;
    }

    private void Charge()
    {
        if(chargeChannelTimeRemain > 0)
        {
            chargeChannelTimeRemain -= Time.deltaTime;
            rb2d.velocity = Vector2.zero;
            anim.Play("Idle");
            chargeDirection = (GameManager.Instance.Player.transform.position - transform.position).normalized;
            return;
        }
        if(chargeTimeRemain > 0)
        {
            chargeTimeRemain -= Time.deltaTime;
            rb2d.velocity = BaseSpeed * chargeSpeed * chargeDirection;
            anim.Play(chargeDirection.x > 0 ? "Right" : "Left");
            if(Mathf.Abs(transform.position.x) > 14 || Mathf.Abs(transform.position.y) > 14)
                isCharging = false;
        }
        else
            isCharging = false;
    }
}
