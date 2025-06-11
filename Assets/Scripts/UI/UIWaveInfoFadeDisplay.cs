using TMPro;
using UnityEngine;

public class UIWaveInfoFadeDisplay : MonoBehaviour
{
    private TextMeshProUGUI waveDisplay;
    private Animator waveDisplayAnimator;

    protected void Start()
    {
        waveDisplay = GetComponent<TextMeshProUGUI>();
        waveDisplayAnimator = GetComponent<Animator>();
        EnemyGenerationManager.OnWaveStart += OnWaveStart;
    }

    private void OnWaveStart(object sender, System.EventArgs e)
    {
        if(EnemyGenerationManager.Instance.WaveType == WaveType.Normal)
        {
            waveDisplay.color = Color.white;
            waveDisplay.text = "WAVE " + EnemyGenerationManager.Instance.waveNumber;
        }
        else
        {
            waveDisplay.color = Color.red;
            waveDisplay.text = "BOSS WAVE";
        }
        waveDisplayAnimator.Play("WaveInfoFadeDisplay");
    }

    protected void OnDestroy()
    {
        EnemyGenerationManager.OnWaveStart -= OnWaveStart;
    }
}
