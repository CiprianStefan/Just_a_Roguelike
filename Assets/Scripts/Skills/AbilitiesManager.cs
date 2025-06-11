using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilitiesManager : MonoBehaviour
{
    public static AbilitiesManager Instance;

    private const int _MAX_ABILITIES = 6;
    
    private AbilityHolder [] abilityHolders = new AbilityHolder[_MAX_ABILITIES];
    private int abilitiesCount = 0;
    private int evolvedCount = 0;

    public bool AbilitiesLimitReached { get {return abilitiesCount == _MAX_ABILITIES;}}
    public bool EvolutionsLimitReached { get {return evolvedCount == _MAX_ABILITIES;}}
    public List<string> OwnedAbilitiesNames { 
        get { 
            List<string> abilitiesNames = new(); 
            foreach(AbilityHolder abilityHolder in abilityHolders) 
                if(abilityHolder != null)
                    abilitiesNames.Add(abilityHolder.skillContainer.Name);
            return abilitiesNames;
        }
    }

    public EventHandler<Tuple<int, SkillContainer>> OnAbilityUpdate;

    protected void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }
 
    public void AddAbility(GameObject abilityHolder)
    {
        for(int i = 0; i < abilityHolders.Length; i++)
            if(abilityHolders[i] == null)
            {
                abilityHolders[i] = Instantiate(abilityHolder).GetComponent<AbilityHolder>();
                OnAbilityUpdate?.Invoke(this, Tuple.Create(i, abilityHolders[i].skillContainer));
                break;
            }
        abilitiesCount++;
    }
    public void EvolveAbility(GameObject abilityHolder, string replaceAbilityName)
    {
        int replacedAbilityIndex = -1;
        foreach(AbilityHolder ablHld in abilityHolders)
            if( ablHld.skillContainer.Name == replaceAbilityName )
            { 
                replacedAbilityIndex = Array.IndexOf(abilityHolders, ablHld);
                break;
            }
        Destroy(abilityHolders[replacedAbilityIndex].gameObject);
        abilityHolders[replacedAbilityIndex] = Instantiate(abilityHolder).GetComponent<AbilityHolder>();
        OnAbilityUpdate?.Invoke(this, Tuple.Create(replacedAbilityIndex, abilityHolders[replacedAbilityIndex].skillContainer));
        evolvedCount++;
    }
}
