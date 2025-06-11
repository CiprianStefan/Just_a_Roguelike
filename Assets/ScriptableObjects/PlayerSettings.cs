using UnityEngine;


[CreateAssetMenu(fileName = "PlayerSettings", menuName = "PlayerSettings", order = 1)]
public class PlayerSettings : ScriptableObject
{
    public float SFxVolume = 0.5f;
    public float MusicVolume = 0.5f;
    public bool SFxEnabled = true;
    public bool MusicEnabled = true;
}
