using UnityEngine;

public class PlayerMisc : MonoBehaviour
{
    public static PlayerMisc Instance;

    [SerializeField]
    private SoulContainerDatabase soulContainerDatabase;
    [SerializeField]
    private SoulContainer baseSoulContainer;
    public SoulContainer soulContainer;


    protected void Awake()
    {
        if(Instance == null)
            Instance = this;
        else
            Destroy(this);
        if(PlayerPrefs.HasKey("SelectedSoul") && soulContainerDatabase.soulContainers.Exists(x => x.Name == PlayerPrefs.GetString("SelectedSoul")))
        {
            var selectedSoul = PlayerPrefs.GetString("SelectedSoul");
            soulContainer = soulContainerDatabase.soulContainers.Find(x => x.Name == selectedSoul);
        }
        else
            soulContainer = baseSoulContainer;
    }
}
