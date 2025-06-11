using UnityEngine;

public class EliteEnemyAttackScript : MonoBehaviour
{
    private EliteEnemyMisc eliteEnemyMisc;
    [SerializeField]
    private GameObject AOEPrefab;

    private int EliteSecondaryType { get => eliteEnemyMisc.EliteSecondaryType; }

    void Start()
    {
        eliteEnemyMisc = GetComponent<EliteEnemyMisc>();
        InitializeAOE();
    }

    public void InitializeAOE()
    {
        GameObject AOE = Instantiate(AOEPrefab, Vector3.zero, Quaternion.identity, gameObject.transform);
        AOE.GetComponent<EliteAOEDebuff>().SetEliteDebuffType(EliteSecondaryType);
    }

}
