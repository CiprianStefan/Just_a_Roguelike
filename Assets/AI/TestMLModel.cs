
using UnityEngine;
using Unity.Barracuda;

public class TestMLModel : MonoBehaviour
{
    public NNModel eliteEnemyModel;
    private IWorker worker;

    public float health;
    public float defence;
    public float block;
    public float dpsValue;
    public float movementSpeed;
    public float dodgeChance;
    public float abilityCooldown;


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
        float minFlatHealth = 100;
        float maxFlatHealth = 300;
        traitsBasedOnPlayerStats[0] = Mathf.Clamp(Normalization(health, minFlatHealth, maxFlatHealth), 0, 1);
        
        //Defences
        float minDefence = 10;
        float maxDefence = 340;
        float minBlock = 10;
        float maxBlock = 100;
        traitsBasedOnPlayerStats[1] = Mathf.Max(Mathf.Clamp(Normalization(defence, minDefence, maxDefence), 0, 1),
            Mathf.Clamp(Normalization(block, minBlock, maxBlock), 0, 1));

        //Evasion
        float minMovementSpeed = 80;
        float maxMovementSpeed = 200;
        float minDodgeChance = 0;
        float maxDodgeChance = 80;
        traitsBasedOnPlayerStats[2] = Mathf.Max(Mathf.Clamp(Normalization(movementSpeed, minMovementSpeed, maxMovementSpeed), 0, 1),
            Mathf.Clamp(Normalization(dodgeChance, minDodgeChance, maxDodgeChance), 0, 1));

        //DPS
        float minDPS = 1f;
        float maxDPS = 10f;
        traitsBasedOnPlayerStats[3] = Mathf.Clamp(Normalization(dpsValue, minDPS, maxDPS), 0, 1);
        
        //Ability Speed
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
