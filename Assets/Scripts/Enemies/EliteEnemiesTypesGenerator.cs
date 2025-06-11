using UnityEngine;
using Unity.Barracuda;


public class EliteEnemiesTypesGenerator : MonoBehaviour
{
    public static EliteEnemiesTypesGenerator Instance;
    public NNModel eliteEnemyModel;
    private IWorker worker;

    protected void Awake()
    {
        if(Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        var model = ModelLoader.Load(eliteEnemyModel);
        worker = WorkerFactory.CreateWorker(WorkerFactory.Type.ComputePrecompiled, model);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            float[] playerStats = ReturnInputsBasedOnPlayerStats();

            Debug.Log(playerStats[0] + " " + playerStats[1] + " " + playerStats[2] + " " + playerStats[3] + " " + playerStats[4]);
            Debug.Log(this.PredictEliteEnemy(playerStats));
       }
    }

    public (int primaryType, int secondaryType) GetEliteEnemyTypes()
    {
        float[] playerStats = ReturnInputsBasedOnPlayerStats();
        var enemyTypes = PredictEliteEnemy(playerStats);

        Debug.Log(playerStats[0] + " " + playerStats[1] + " " + playerStats[2] + " " + playerStats[3] + " " + playerStats[4]);
        Debug.Log(enemyTypes);

        return enemyTypes;
    }

    void OnDestroy()
    {
        worker.Dispose();
    }

    public (int primaryType, int secondaryType1) PredictEliteEnemy(float[] playerStats)
    {
        if (playerStats.Length != 5)
        {
            throw new System.ArgumentException("Input must have exactly 5 features.");
        }

        // Create input tensor
        var inputTensor = new Tensor(1, 5); // 1 batch, 12 features
        for (int i = 0; i < 5; i++)
        {
            inputTensor[0, i] = playerStats[i];
        }

        // Execute the model
        worker.Execute(inputTensor);

        // Extract outputs
        //var primaryOutput = worker.PeekOutput("13"); // Output node name from your model
        //var secondary1Output = worker.PeekOutput("14");
        var primaryOutput = worker.PeekOutput("primary_output");
        var secondary1Output = worker.PeekOutput("secondary_output");

        Debug.Log(primaryOutput);
        Debug.Log(secondary1Output);

        // Get predicted classes
        int primaryType = GetMaxIndex(primaryOutput) + 1;
        int secondaryType1 = GetMaxIndex(secondary1Output) + 1;

        // Dispose input tensor
        inputTensor.Dispose();

        return (primaryType, secondaryType1);
    }

    private int GetMaxIndex(Tensor tensor)
    {
        float maxValue = float.MinValue;
        int maxIndex = 0;

        for (int i = 0; i < tensor.length; i++)
        {
            if (tensor[i] > maxValue)
            {
                maxValue = tensor[i];
                maxIndex = i;
            }
        }

        return maxIndex;
    }

    private float[] ReturnInputsBasedOnPlayerStats()
    {
        float[] traitsBasedOnPlayerStats = new float[5];
        PlayerStatsManager statsManager = PlayerStatsManager.Instance;
        GameManager gameManager = GameManager.Instance;

        //Flat Health
        float health = statsManager.stats[StatType.Health].StatValue;
        float minFlatHealth = 100;
        float maxFlatHealth = 300;
        traitsBasedOnPlayerStats[0] = Mathf.Clamp(Normalization(health, minFlatHealth, maxFlatHealth), 0, 1);
        
        //Defences
        float defence = statsManager.stats[StatType.Defence].StatValue;
        float minDefence = 10;
        float maxDefence = 340;
        float block = statsManager.stats[StatType.Block].StatValue;
        float minBlock = 10;
        float maxBlock = 100;
        traitsBasedOnPlayerStats[1] = Mathf.Max(Mathf.Clamp(Normalization(defence, minDefence, maxDefence), 0, 1),
            Mathf.Clamp(Normalization(block, minBlock, maxBlock), 0, 1));

        //Evasion
        float movementSpeed = statsManager.stats[StatType.MovementSpeed].StatValue;
        float minMovementSpeed = 80;
        float maxMovementSpeed = 200;
        float dodgeChance = statsManager.stats[StatType.DodgeChance].StatValue;
        float minDodgeChance = 0;
        float maxDodgeChance = 80;
        traitsBasedOnPlayerStats[2] = Mathf.Max(Mathf.Clamp(Normalization(movementSpeed, minMovementSpeed, maxMovementSpeed), 0, 1),
            Mathf.Clamp(Normalization(dodgeChance, minDodgeChance, maxDodgeChance), 0, 1));

        //DPS
        float physicalDamage = statsManager.stats[StatType.PhysicalDamage].StatValue;
        float magicalDamage = statsManager.stats[StatType.MagicalDamage].StatValue;
        float elementalDamage = statsManager.stats[StatType.ElementalDamage].StatValue;
        float criticalDamage = statsManager.stats[StatType.CriticalDamage].StatValue;
        float criticalChance = statsManager.stats[StatType.CriticalChance].StatValue;
        float projectiles = statsManager.stats[StatType.Projectiles].StatValue;
        float gameTimeInMinutes = Mathf.RoundToInt(gameManager.GameTimeInMinutes);
        float dpsValue = Mathf.Max(physicalDamage, magicalDamage, elementalDamage) * 
            (1 + criticalDamage / 100) * (1 + criticalChance / 100) * (1 + projectiles * 0.2f) / 
            (5 * gameTimeInMinutes);
        float minDPS = 1f;
        float maxDPS = 10f;
        traitsBasedOnPlayerStats[3] = Mathf.Clamp(Normalization(dpsValue, minDPS, maxDPS), 0, 1);
        
        //Ability Speed
        float abilityCooldown = statsManager.stats[StatType.AbilityCooldown].StatValue;
        float minAbilityCooldown = 0;
        float maxAbilityCooldown = 80;
        traitsBasedOnPlayerStats[4] = Mathf.Clamp(Normalization(abilityCooldown, minAbilityCooldown, maxAbilityCooldown), 0, 1);

        return traitsBasedOnPlayerStats;
    }

    private float Normalization(float value, float min, float max)
    {
        return (value - min) / (max - min);
    }
}
