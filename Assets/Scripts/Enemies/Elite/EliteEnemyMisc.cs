using UnityEngine;
using TMPro;
using System.Collections;

public class EliteEnemyMisc : EnemyMisc
{
    private int elitePrimaryType;
    private int eliteSecondaryType;

    [SerializeField]
    private TextMeshProUGUI eliteName;

    public int ElitePrimaryType { get => elitePrimaryType; }
    public int EliteSecondaryType { get => eliteSecondaryType; }

    public void OnInit(int primaryType, int secondaryType)
    {
        elitePrimaryType = primaryType;
        eliteSecondaryType = secondaryType;
        eliteName.text = "Elite" + 
            (primaryType switch
            {
                1 => " Giant",
                2 => " Sprinter",
                3 => " Crab",
                4 => " Immortal",
                5 => " Shifter",
                6 => " Refresher",
                _ => ""
            });
    }

    protected override void Start()
    {
        base.Start();
        StartCoroutine(DestroyTimer());
    }

    private IEnumerator DestroyTimer()
    {
        yield return new WaitForSeconds(60f);
        DisableEnemyRoutine();
        anim.Play("Death");
        Destroy(gameObject, 0.5f);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        StopAllCoroutines();
    }

}
