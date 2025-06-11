using TMPro;
using UnityEngine;

public class UIWaveDisplay : MonoBehaviour
{
    private TextMeshProUGUI waveDisplay;

    protected void Start()
    {
        waveDisplay = GetComponent<TextMeshProUGUI>();
        EnemyGenerationManager.OnWaveStart += OnWaveStart;
    }

    private void OnWaveStart(object sender, System.EventArgs e)
    {
        waveDisplay.text = "WAVE " + EnemyGenerationManager.Instance.waveNumber;
    }

    protected void OnDestroy()
    {
        EnemyGenerationManager.OnWaveStart -= OnWaveStart;
    }
}
