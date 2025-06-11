using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthBarHolderUI : MonoBehaviour
{
    [SerializeField]
    GameObject bossHealthBarPrefab;

    Dictionary<EnemyCombatEntity, GameObject> bossHealthBars;

    protected void Awake()
    {
        EnemyMisc.OnBossSpawn += CreateBossHealthBar;
        EnemyCombatEntity.OnBossDamageTaken += UpdateBossHealthBar;
        bossHealthBars = new Dictionary<EnemyCombatEntity, GameObject>();
    }

    private void CreateBossHealthBar(object sender, System.EventArgs e)
    {
        EnemyMisc enemyMisc = sender as EnemyMisc;
        EnemyCombatEntity bossCombatEntity = enemyMisc.combatEntity;
        GameObject bossHealthBar = Instantiate(bossHealthBarPrefab, transform);
        bossHealthBar.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = enemyMisc.enemyContainer.enemyName;
        bossHealthBar.transform.GetChild(1).transform.GetChild(0).GetComponent<Image>().fillAmount = 1;
        bossHealthBars.Add(bossCombatEntity, bossHealthBar);
    }

    private void UpdateBossHealthBar(object sender, EventArgs e)
    {
        EnemyCombatEntity bossCombatEntity = sender as EnemyCombatEntity;
        float healthPercentage;
        if(bossCombatEntity == null)
            healthPercentage = 0;
        else
            healthPercentage = (float)Math.Round(bossCombatEntity.CurrentHealth / bossCombatEntity.Health,2);
        if(bossHealthBars.ContainsKey(bossCombatEntity))
        {
            bossHealthBars[bossCombatEntity].transform.GetChild(1).transform.GetChild(0).GetComponent<Image>().fillAmount = healthPercentage;
            if(healthPercentage <= 0)
            {
                Destroy(bossHealthBars[bossCombatEntity]);
                bossHealthBars.Remove(bossCombatEntity);
            }
        }
    }

    protected void OnDestroy()
    {
        EnemyMisc.OnBossSpawn -= CreateBossHealthBar;
        EnemyCombatEntity.OnBossDamageTaken += UpdateBossHealthBar;
    }
}
